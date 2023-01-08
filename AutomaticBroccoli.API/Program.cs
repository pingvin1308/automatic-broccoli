using AutomaticBroccoli.API.Controllers;
using AutomaticBroccoli.DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "AutomaticBroccoli.API.xml");
    options.IncludeXmlComments(filePath);
});

builder.Services.AddDbContext<AutomaticBroccoliDbContext>(options =>
{
    options
        // .UseLazyLoadingProxies()
#if DEBUG
        .EnableSensitiveDataLogging()
#endif
        .UseNpgsql(builder.Configuration.GetConnectionString(nameof(AutomaticBroccoliDbContext)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAttachments();
if (!Directory.Exists(Attachments.Path))
{
    Directory.CreateDirectory(Attachments.Path);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Attachments.Path),
    RequestPath = "/inbox/attachments"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }