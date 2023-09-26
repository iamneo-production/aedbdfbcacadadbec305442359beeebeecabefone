using BookStoreDBFirst.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CourseEnquiryDbContext>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnString"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAnyOrigin",
        builder =>
        {
            builder
                .AllowAnyOrigin() // Allow requests from any origin
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowAnyOrigin");
app.MapControllers();

app.Run();
