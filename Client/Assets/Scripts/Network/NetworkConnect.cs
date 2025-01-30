using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkConnect : MonoBehaviour
{
    [SerializeField] private TMP_InputField wsAdress;
    [SerializeField] private TMP_InputField userName;

    [SerializeField] private Button webSocketConnectBTN;
    [SerializeField] private Button loginBTN;

    private void Start()
    {
        webSocketConnectBTN.onClick.AddListener(Connect);
        loginBTN.onClick.AddListener(Login);
    }

    private void OnEnable()
    {
        NetworkManager.Instance.OnWebSocketOpen(Login);
    }

    private void Connect()
    {
        //ex :
        //ws://localhost:5184
        NetworkManager.Instance.InitializeWebSocket(wsAdress.text);
    }

    private void Login()
    {
        if(ulong.TryParse(userName.text, out ulong number))
        {
            NetworkManager.Instance.Login(number);
        }
    }
}