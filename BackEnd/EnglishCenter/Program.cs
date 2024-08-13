using EnglishCenter.Extensions;
using EnglishCenter.Extensions.Database;
using EnglishCenter.Extensions.Identity;
using EnglishCenter.Extensions.Repository;
using Microsoft.CodeAnalysis.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSystemServices(builder);
builder.Services.AddDatabaseConfiguration(builder);
builder.Services.AddRepositories();
builder.Services.AddIdentityConfiguration(builder);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
