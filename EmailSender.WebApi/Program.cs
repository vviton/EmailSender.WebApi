using EmailSender.Producer;
using EmailSender.WebApi.Configuration;
using EmailSender.WebApi.RabbitMQ;
using EmailSender.WebApi.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conf = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EmailSettings>(conf.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSendingService, SmptSendingService>();

builder.Services.AddTransient<EmailConsumer>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var hostName = configuration["RabbitMQSettings:Host"];
    var portNumber = configuration.GetValue<int>("RabbitMQSettings:Port");
    var emailSendingService = provider.GetRequiredService<IEmailSendingService>();
    return new EmailConsumer(hostName, portNumber, emailSendingService);
});
builder.Services.AddSingleton<IRabbitMqProducer<Email>, EmailProducer>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var hostName = config["RabbitMQSettings:Host"];
    var portNumber = config.GetValue<int>("RabbitMQSettings:Port");
    return new EmailProducer(hostName, portNumber);
}

);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
