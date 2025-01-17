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
using MassTransit;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(UserHandler).GetTypeInfo().Assembly);

#region Services

builder.Services.AddScoped<IOtpRedisRepository, OtpRedisRepository>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();
builder.Services.AddScoped<IUserCommandRepository, UserCommandRepository>();

#endregion Services

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

#region CAP

builder.Services.AddCap(options =>
{
    options.UseEntityFramework<CommandDBContext>();
    options.UseDashboard(path => path.PathMatch = "/cap");
    options.UseRabbitMQ(op =>
    {
        op.ConnectionFactoryOptions = op =>
        {
            op.Ssl.Enabled = false;
            op.HostName = "localhost";
            op.UserName = "guest";
            op.Password = "guest";
            op.Port = 5672;
        };
    });
    options.FailedRetryCount = 10;
    options.FailedRetryInterval = 5;
});

#endregion CAP

#region Masstransiate

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddEntityFrameworkOutbox<CommandDBContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(30);
        o.UseSqlServer().UseBusOutbox();
    });
    busConfig.SetKebabCaseEndpointNameFormatter();
    busConfig.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration.GetValue<string>("Rabbit:Host")), h =>
        {
            h.Username(builder.Configuration.GetValue<string>("Rabbit:username"));
            h.Username(builder.Configuration.GetValue<string>("Rabbit:password"));
        });
        cfg.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));
        cfg.ConfigureEndpoints(context);
    });
});

#endregion Masstransiate

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