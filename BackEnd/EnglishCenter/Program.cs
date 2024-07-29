using EnglishCenter.Extensions;
using EnglishCenter.Extensions.Database;
using EnglishCenter.Extensions.Identity;
using EnglishCenter.Extensions.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "DefaulPolicy",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:5173")
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                      });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseConfiguration(builder);
builder.Services.AddRepositories();
builder.Services.AddIdentityConfiguration(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DefaulPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
