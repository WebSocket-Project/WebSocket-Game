using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using NativeWebSocket;
using Newtonsoft.Json;
using Shared;
using Shared.sdf;
using UnityEngine;
using UnityEngine.UI;

public class NetworkConnect : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    WebSocket websocket;

    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket("ws://localhost:5184");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
        };

        Invoke("SendLoginMessage", 1);

        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendLoginMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            var loginRequest = new LoginRequest();
            loginRequest.PlayerId = 1234;
            var loginJson = JsonConvert.SerializeObject(loginRequest);
        
            var request = new WebSocketRequest()
            {
                Protocol = Protocol.Login,
                Payload = loginRequest
            };
            var requestJson = JsonConvert.SerializeObject(request);
        
            // Sending plain text
            await websocket.SendText(requestJson);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close(); 
    }
}
