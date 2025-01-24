using Server.WebSockets;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<WebSocketConnectionProvider>();
builder.Services.AddTransient<WebSocketHandler>();
builder.Services.AddTransient<WebSocketMiddleware>();
builder.Services.AddRequestHandlers();

WebApplication app = builder.Build();

app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();

app.Run();