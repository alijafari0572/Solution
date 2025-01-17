using System.Reflection;
using Asp.Versioning;
using IDP.Application.Handler.Command;
using IDP.Application.Handler.Command.User;
using IDP.Application.Helper;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Query;
using IDP.Infra.Data;
using IDP.Infra.Repository.Command;
using IDP.Infra.Repository.Query;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(UserHandler).GetTypeInfo().Assembly);
builder.Services.AddScoped<IOtpRedisRepository, OtpRedisRepository>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();
builder.Services.AddScoped<IUserCommandRepository, UserCommandRepository>();
builder.Services.AddScoped<QueryDBConnection>();
builder.Services.AddScoped<CommandDBContext>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddMvc() // This is needed for controllers
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetValue<string>("CashSetting:RedisUrl");
});

Auth.Extensions.AddJwt(builder.Services, builder.Configuration);
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