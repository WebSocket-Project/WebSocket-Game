using System.Text.Json;
using Server.WebSockets;
using Shared;
using Shared.Dtos;
using WebSocketContext = Server.WebSockets.WebSocketContext;

namespace Server.RequestHandlers;

[Protocol(Protocol.Login)]
public class LoginHandler : WebSocketRequestHandler
{
    public LoginHandler(WebSocketConnectionProvider webSocketConnectionProvider)
        : base(webSocketConnectionProvider)
    {
    }

    public override async Task HandleAsync(WebSocketContext webSocketContext)
    {
        LoginRequest? loginRequest = webSocketContext.Request.Payload.Deserialize<LoginRequest>();
        await WebSocketConnectionProvider.ConnectAsync(webSocketContext.WebSocket, loginRequest!.PlayerId);
        LoginResponse loginResponse = new() { Message = "Login successful!" }; 
        await SendAsync(webSocketContext.WebSocket, webSocketContext.Request.Protocol, loginResponse);
    }
}