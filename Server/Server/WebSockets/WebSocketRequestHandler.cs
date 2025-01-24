using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Shared;

namespace Server.WebSockets;

public abstract class WebSocketRequestHandler
{
    protected readonly WebSocketConnectionProvider WebSocketConnectionProvider;

    protected WebSocketRequestHandler(WebSocketConnectionProvider webSocketConnectionProvider)
    {
        WebSocketConnectionProvider = webSocketConnectionProvider;
    }
    
    public abstract Task HandleAsync(WebSocketContext webSocketContext);
    
    protected async Task SendAsync(WebSocket webSocket, Protocol protocol, params object[] payload)
    {
        WebSocketResponse response = new()
        {
            Protocol = protocol,
            Payload = payload
        };
        
        await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response))), 
                                  WebSocketMessageType.Text, true, CancellationToken.None);
    }
}