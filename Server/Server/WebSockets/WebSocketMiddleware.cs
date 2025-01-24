using System.Net.WebSockets;

namespace Server.WebSockets;

public class WebSocketMiddleware : IMiddleware
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WebSocketMiddleware> _logger;

    public WebSocketMiddleware(IServiceProvider serviceProvider, ILogger<WebSocketMiddleware> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            try
            {
                WebSocketHandler webSocketHandler = _serviceProvider.GetRequiredService<WebSocketHandler>();
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                await webSocketHandler.LoopAsync(webSocket);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occured while handling websocket. {Message}", e.Message);
            }
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
}