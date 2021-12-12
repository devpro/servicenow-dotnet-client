using RabbidsIncubator.ServiceNowClient.Application.DependencyInjection;
using RabbidsIncubator.ServiceNowClient.Infrastructure.ServiceNowRestApi;

// creates the builder

var builder = WebApplication.CreateBuilder(args);

// adds services to the collection

builder.Services.AddAutoMapperConfiguration();
builder.Services.AddRepositories(builder.Configuration.GetSection("ServiceNow:RestApi").Get<ServiceNowRestApiConfiguration>());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// creates the application

var app = builder.Build();

// configures the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// runs the application

app.Run();
