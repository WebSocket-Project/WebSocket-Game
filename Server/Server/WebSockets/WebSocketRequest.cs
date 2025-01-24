using System.Text.Json.Nodes;
using Shared;

namespace Server.WebSockets;

public class WebSocketRequest
{
    public Protocol Protocol { get; set; }
    public JsonObject Payload { get; set; }
    
    public WebSocketRequest(Protocol protocol, JsonObject payload)
    {
        Protocol = protocol;
        Payload = payload;
    }
}