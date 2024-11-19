using EnglishCenter.Presentation.Extensions;
using EnglishCenter.Presentation.Extensions.Database;
using EnglishCenter.Presentation.Extensions.Identity;
using EnglishCenter.Presentation.Extensions.Repository;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Hub;
using EnglishCenter.Presentation.Middleware;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSystemServices(builder);
builder.Services.AddDatabaseConfiguration(builder);
builder.Services.AddIdentityConfiguration(builder);
builder.Services.AddRepositories();
builder.Services.AddServicesLayer();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});
var app = builder.Build();

VideoHelper.Initialize(app.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseRouting();
app.MapHub<NotificationHub>("api/hub/notification");
app.MapHub<ChatHub>("api/hub/chats");
app.UseCors("AllPolicy");
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseMiddleware<AddClaimsMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
