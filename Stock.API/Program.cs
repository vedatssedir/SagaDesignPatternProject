using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MainDbContext>(options => options.UseInMemoryDatabase("StockDb"));

builder.Services.AddMassTransit(opt =>
{
    opt.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
    });
});

using (var serviceProvider = builder.Services.BuildServiceProvider())
{
    var context = serviceProvider.GetRequiredService<MainDbContext>();
    context.StockItems.Add(new StockItem() { Count = 100, Id = 1, ProductId = 1 });
    context.SaveChanges();
}



var app = builder.Build();







if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
