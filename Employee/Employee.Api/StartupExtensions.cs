using Elasticsearch.Net;
using Employee.Api.Middleware;
using Employee.Application;
using Employee.Infrastructure;
using Employee.Infrastructure.Kafka;
using Employee.Persistence;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Models;
using Nest;


namespace Employee.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(
        this WebApplicationBuilder builder)
        {
            AddSwagger(builder.Services);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            // Elasticsearch connection
            var pool = new SingleNodeConnectionPool(new Uri("http://elasticsearch:9200"));
            var settings = new ConnectionSettings(pool).DefaultIndex("employee-index");
            var client = new ElasticClient(settings);
            builder.Services.AddSingleton(client);

            //kafka config
            const string topicName = "employee-topic";
            const string producerName = "employee-producer";
            const string broker = "kafka:9092";
            const string groupId = "employee-group";

            //kafka
            builder.Services.AddKafka(
                kafka => kafka
                    .UseConsoleLog()
                    .AddCluster(
                        cluster => cluster
                            .WithBrokers(new[] { broker })
                            .CreateTopicIfNotExists(topicName, 1, 1)
                            .AddProducer(
                                producerName,
                                producer => producer
                                    .DefaultTopic(topicName)
                                    .AddMiddlewares(m =>
                                        m.AddSerializer<JsonCoreSerializer>()
                                        )
                            )
                            .AddConsumer(consumer => consumer
                                .Topic(topicName)
                                .WithGroupId(groupId)
                                .WithBufferSize(100)
                                .WithWorkersCount(3)
                                .WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Earliest)
                                .AddMiddlewares(middlewares => middlewares
                                    .AddDeserializer<JsonCoreDeserializer>()
                                    .AddTypedHandlers(h => h.AddHandler<MessageHandler>())
                        )
                    )
                )
            );

            return builder.Build();

        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API");
                });
            }

            app.UseHttpsRedirection();

            //app.UseRouting();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseCors("Open");

            app.MapControllers();

            return app;

        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Employee Management API",

                });
            });
        }

        public static async Task ResetDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetService<EmployeeDbContext>();
                if (context != null)
                {
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
