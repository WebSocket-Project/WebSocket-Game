using System.Net.WebSockets;
using Shared;

namespace Server.WebSockets;

public class WebSocketContext
{
    public WebSocket WebSocket { get; set; }
    public WebSocketRequest Request { get; set; }
    
    public WebSocketContext(WebSocket webSocket, WebSocketRequest request)
    {
        WebSocket = webSocket;
        Request = request;
    }
}