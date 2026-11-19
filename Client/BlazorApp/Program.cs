using BlazorApp.Auth;
using BlazorApp.Components;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authentication services
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHttpClient<SimpleAuthProvider>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5164/");
});

builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<SimpleAuthProvider>());

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

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();