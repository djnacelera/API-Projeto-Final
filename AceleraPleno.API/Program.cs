using AceleraPleno.API.Data;
using AceleraPleno.API.Interface;
using AceleraPleno.API.Models;
using AceleraPleno.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IRepository<Cliente>, ClienteRepository>();

builder.Services.AddScoped<IRepositoryPrato<Prato>, PratoRepository>();

builder.Services.AddScoped<IRepositoryMesa<Mesa>, MesaRepository>();

builder.Services.AddScoped<IRepositoryPedido<Pedido>, PedidoRepository>();

builder.Services.AddScoped<IRepository<Pedido>, PedidoRepository>();

builder.Services.AddScoped<IRepositoryLog<Log>, LogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x.AllowAnyHeader()
.AllowAnyMethod()
.AllowAnyOrigin()
);

app.MapControllers();

app.Run();