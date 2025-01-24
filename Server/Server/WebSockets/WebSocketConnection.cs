namespace Server.WebSockets;

public class WebSocketConnection
{
    public ulong PlayerId { get; set; }
    
    public WebSocketConnection(ulong playerId)
    {
        PlayerId = playerId;
    }
}