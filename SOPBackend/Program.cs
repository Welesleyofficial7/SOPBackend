using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using SOPBackend;
using SOPBackend.MappingProfiles;
using SOPBackend.Repositories;
using SOPBackend.Repositories.Interfaces;
using SOPBackend.Services;
using SOPBackend.Services.Utils;

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

builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization(); 

app.MapControllers();

app.Run();