using BookRental.BusinessLayer;
using BookRental.BusinessLayer.BackGroundJobs;
using BookRental.BusinessLayer.Interfaces;
using BookRental.BusinessLayer.Repositories;
using BookRental.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.AddSqlServerDbContext<BookRentalDBContext>("DefaultConnection");

builder.Services.AddDbContext<BookRentalDBContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<HostOptions>(options =>
{
    options.ServicesStartConcurrently = true;
    options.ServicesStopConcurrently = true;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
// Configure services
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<RentalService>();
//builder.Services.AddHostedService<RentalOverdueHosterService>();

//builder.Services.AddHostedService<Recorentjob>();




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
