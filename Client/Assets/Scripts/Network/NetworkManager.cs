using UnityEngine;
using NativeWebSocket;
using System;
using Newtonsoft.Json;
using Shared.sdf;
using Shared;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private WebSocket _webSocket;

    private event Action OnWebSocketOpenEvent;
    private event Action<object> OnWebSocketMessageEvent;

    public void OnWebSocketOpen(Action open)
    {
        OnWebSocketOpenEvent += open;
    }

    public void OnWebSocketMessage(Action<object> message)
    {
        OnWebSocketMessageEvent += message;
    }

    public async void InitializeWebSocket(string address)
    {
        _webSocket = new WebSocket(address);

        _webSocket.OnOpen += () =>
        {
            OnWebSocketOpenEvent?.Invoke();
            Debug.Log("Connection Open!");
        };

        _webSocket.OnError += (errorMessage) =>
        {
            Debug.LogError($"WebSocket Error: {errorMessage}");
        };

        _webSocket.OnClose += (e) =>
        {
            OnWebSocketOpenEvent = null;
            OnWebSocketMessageEvent = null;
            Debug.Log("Connection Closed!");
        };

        _webSocket.OnMessage += (bytes) =>
        {
            OnWebSocketMessageEvent?.Invoke(bytes);

            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
        };

        await _webSocket.Connect();
    }

    public async void Login(ulong playerID)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            Debug.LogWarning("WebSocket is not open. Cannot send login message.");
            return;
        }

        try
        {
            var loginRequest = new LoginRequest();
            loginRequest.PlayerId = playerID;

            var request = new WebSocketRequest()
            {
                Protocol = Protocol.Login,
                Payload = loginRequest
            };

            var requestJson = JsonConvert.SerializeObject(request);
            await _webSocket.Send(Encoding.UTF8.GetBytes(requestJson));
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send login message: {ex.Message}");
        }
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if(_webSocket != null)
            _webSocket.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        await _webSocket.Close();
    }
}