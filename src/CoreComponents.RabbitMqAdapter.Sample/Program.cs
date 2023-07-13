using System.Reflection;
using CoreComponents.RabbitMQAdapter;
using CoreComponents.RabbitMQAdapter.Interfaces;
using CoreComponents.RabbitMQAdapter.Sample.Events;
using CoreComponents.RabbitMQAdapter.Sample.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<IRabbitMQProducer<OrderEvent>, RabbitMQProducer<OrderEvent>>(sp =>
    new RabbitMQProducer<OrderEvent>("amqp://myuser:mypassword@rabbitmq:5672"));

builder.Services.AddTransient<IRabbitMQConsumer<OrderEvent>, RabbitMQConsumer<OrderEvent>>(sp =>
    new RabbitMQConsumer<OrderEvent>("amqp://myuser:mypassword@rabbitmq:5672", sp.GetRequiredService<IMediator>()));

//builder.Services.AddHostedService<RabbitMQService>();

builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

