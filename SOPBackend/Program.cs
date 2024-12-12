using EasyNetQ;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SOPBackend;
using SOPBackend.Filters;
using SOPBackend.Hubs;
using SOPBackend.MappingProfiles;
using SOPBackend.Repositories;
using SOPBackend.Repositories.Interfaces;
using SOPBackend.Services;
using SOPBackend.Services.Utils;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var bus = RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("AutoRabbitMQ"));
builder.Services.AddSingleton<IBus>(bus);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<KafkaProducer>();

builder.Services.AddScoped<IUserService, UserService>(); 
builder.Services.AddScoped<IOrderService, OrderService>(); 
builder.Services.AddScoped<IMenuItemService, MenuItemService>(); 
builder.Services.AddScoped<IOrderItemService, OrderItemService>(); 
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ExceptionHandlers>();
    });

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); 
    options.OperationFilter<SwaggerOperationsFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost8080",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;
    });


var app = builder.Build();
app.UseCors("AllowLocalhost8080");



app.MapHub<OrderHub>("/orderhub");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.DisplayOperationId();
        c.ShowExtensions();
        c.EnableValidator();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization(); 

app.MapControllers();

app.Run();