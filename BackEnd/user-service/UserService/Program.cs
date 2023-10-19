using UserService.Infrastructure;
using UserService.Middleware;
using UserService.Service;
using Microsoft.EntityFrameworkCore;
using UserService.Service.Interface;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using UserService.Attribute;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();
//Add Database Service
builder.Services.AddInfrastruction(builder.Configuration);

//Add Redis, Mongo
builder.Services.AddSingleton<IRedisRepository, RedisRepository>();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault;
    options.JsonSerializerOptions.Converters.Add(new IntNullableJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new DatetimeNullableJsonConverter());
});
var redis = (builder.Configuration["UseEnv"] ?? "0") == "0" ? builder.Configuration["RedisConnection"] : $"{Environment.GetEnvironmentVariable("REDIS_URL")}:{Environment.GetEnvironmentVariable("REDIS_PORT")},password={Environment.GetEnvironmentVariable("REDIS_PASS")}";
var multiplexer = ConnectionMultiplexer.Connect(redis);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
var iDb = multiplexer.GetDatabase();
builder.Services.AddSingleton<IDatabase>(iDb);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());


//Config Apikey
app.UseMiddleware<ApiKeyJwtMiddleware>();

app.UsePathBase("/user-service");
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();