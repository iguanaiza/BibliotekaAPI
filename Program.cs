using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using BibliotekaAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }); //dodaje pominiêcie pêtli odniesieñ np. book->author->books

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger wraz z walidacja required
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes(); // dla [Required] w Swaggerze
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

app.MapControllers();

app.Run();
// dotnet tool install --global dotnet-ef
//dotnet ef migrations add InitialCreate
// dotnet ef database update