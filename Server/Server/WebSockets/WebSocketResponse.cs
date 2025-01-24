using Shared;

namespace Server.WebSockets;

public class WebSocketResponse
{ 
    public string ErrorMessage { get; set; } = string.Empty;

    public Protocol Protocol { get; set; }
    
    public object[] Payload { get; set; } = [];
}