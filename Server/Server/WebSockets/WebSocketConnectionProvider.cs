using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Server.WebSockets;

public class WebSocketConnectionProvider
{
    private readonly ConcurrentDictionary<WebSocket, WebSocketConnection> _connections = [];
    
    public async Task<bool> ConnectAsync(WebSocket webSocket, ulong playerId)
    {
        await Task.Yield();
        
        return _connections.TryAdd(webSocket, new WebSocketConnection(playerId));
    }

    public async Task<bool> DisconnectAsync(WebSocket webSocket)
    {
        await Task.Yield();
        
        return _connections.TryRemove(webSocket, out _);
    }

    public async Task<bool> IsConnectedAsync(ulong playerId)
    {
        await Task.Yield();
        
        return _connections.Any(kvp => kvp.Value.PlayerId == playerId);
    }

    public async Task<ulong> GetPlayerIdByWebSocket(WebSocket webSocket)
    {
        await Task.Yield();
        
        return _connections.TryGetValue(webSocket, out WebSocketConnection? connection)
            ? connection!.PlayerId
            : 0;
    }

    public async Task<WebSocket?> GetWebSocketByPlayerId(ulong playerId)
    {
        await Task.Yield();
        
        (WebSocket? webSocket, WebSocketConnection? connection) = _connections.FirstOrDefault(kvp => kvp.Value.PlayerId == playerId);
        return webSocket;
    }
}