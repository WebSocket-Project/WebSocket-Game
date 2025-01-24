using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Shared;

namespace Server.WebSockets;

public class WebSocketHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WebSocketHandler> _logger;
    private readonly WebSocketConnectionProvider _webSocketConnectionProvider;
    private static readonly Dictionary<Protocol, Type> ProtocolTable = RegisterProtocols();

    public WebSocketHandler(IServiceProvider serviceProvider, ILogger<WebSocketHandler> logger,
                            WebSocketConnectionProvider webSocketConnectionProvider)
    {
        _serviceProvider = serviceProvider;
        _webSocketConnectionProvider = webSocketConnectionProvider;
        _logger = logger;
    }

    public async Task LoopAsync(WebSocket webSocket)
    {
        while (webSocket.State.HasFlag(WebSocketState.Open))
        {
            ArraySegment<byte> buffer = WebSocket.CreateServerBuffer(1024 * 4);

            try
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer.Array ?? [], 0, result.Count);

                if (string.IsNullOrEmpty(message))
                    continue;
                
                WebSocketRequest? request = JsonSerializer.Deserialize<WebSocketRequest>(message);
                if (request == null)
                    continue;

                if (ProtocolTable.TryGetValue(request.Protocol, out Type? handlerType))
                {
                    _logger.LogInformation("Received protocol: {Protocol}", request.Protocol);

                    if (_serviceProvider.GetRequiredService(handlerType) is not WebSocketRequestHandler handlerInstance)
                        throw new Exception();
                    
                    await handlerInstance.HandleAsync(new WebSocketContext(webSocket, request));
                }
                else
                    _logger.LogError("Unknown protocol: {Protocol}", request.Protocol);
            }
            catch (Exception e)
            {
                if (e is not WebSocketException)
                    await HandleCommandExceptionAsync(webSocket, e);
                
                _logger.LogError("An error occured while processing websocket. {Message}", e.Message);
            }
        }
        
        await CloseAsync(webSocket);
    }
    
    private async Task CloseAsync(WebSocket webSocket)
    {
        if (webSocket.State is not WebSocketState.Closed and not WebSocketState.Aborted)
        {
            try
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket closed", CancellationToken.None);
                _logger.LogError("WebSocket connection closed");
            }
            catch (Exception e)
            {
                _logger.LogError("An error occured while closing websocket. Message: {Message}", e.Message);
            }
            finally
            {
                await _webSocketConnectionProvider.DisconnectAsync(webSocket);
            }
        }
    }
    
    private async Task HandleCommandExceptionAsync(WebSocket webSocket, Exception e)
    {
        if (webSocket.State is not WebSocketState.Closed and not WebSocketState.Aborted)
        {
            WebSocketResponse response = new() { ErrorMessage = e.Message };

            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response))), 
                                      WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    
    private static Dictionary<Protocol, Type> RegisterProtocols()
    {
        Dictionary<Protocol, Type> protocolTable = new();
        
        IEnumerable<Type> assemblyTypes = Assembly.GetExecutingAssembly().ExportedTypes;
        IEnumerable<Type> handlerTypes = assemblyTypes.Where(type => type.GetTypeInfo().BaseType == typeof(WebSocketRequestHandler));

        foreach (Type handlerType in handlerTypes)
        {
            ProtocolAttribute? protocolAttribute = handlerType.GetCustomAttribute<ProtocolAttribute>();
            if (protocolAttribute is null)
                throw new Exception($"Handler type {handlerType.FullName} has no {nameof(ProtocolAttribute)}");

            if (protocolTable.TryAdd(protocolAttribute.Protocol, handlerType) == false)
                throw new Exception($"Protocol {protocolAttribute.Protocol} already registered");
        }
        
        return protocolTable;
    }
}