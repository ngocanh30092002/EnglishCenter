using EnglishCenter;
using EnglishCenter.Extensions;
using EnglishCenter.Extensions.Database;
using EnglishCenter.Extensions.Identity;
using EnglishCenter.Extensions.Repository;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSystemServices(builder);
builder.Services.AddDatabaseConfiguration(builder);
builder.Services.AddIdentityConfiguration(builder);
builder.Services.AddRepositories();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapHub<NotificationHub>("api/noti");

app.UseCors("AllPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
