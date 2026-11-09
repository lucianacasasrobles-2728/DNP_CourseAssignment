using BlazorApp.Components;
using BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HTTP services
builder.Services.AddHttpClient<IUserService, HttpUserService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5164/");
});

builder.Services.AddHttpClient<IPostService, HttpPostService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5164/");
});

builder.Services.AddHttpClient<ICommentService, HttpCommentService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5164/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();