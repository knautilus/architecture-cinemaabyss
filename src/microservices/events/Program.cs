using Confluent.Kafka;
using EventsService;
using EventsService.Consumers;
using EventsService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<KafkaClientHandle>();
builder.Services.AddSingleton<KafkaDependentProducer<string, string>>();
builder.Services.AddSingleton<KafkaDependentProducer<string, string>>();
builder.Services.AddSingleton<KafkaDependentProducer<string, string>>();
builder.Services.AddHostedService<MovieConsumer>();
builder.Services.AddHostedService<UserConsumer>();
builder.Services.AddHostedService<PaymentConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run("http://*:8082");
