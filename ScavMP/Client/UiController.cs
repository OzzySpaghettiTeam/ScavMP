using LiteNetLib;
using ScavMP.Client;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ScavMP.Client;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiObject;

    [SerializeField]
    private ClientLogic _clientLogic;

    [SerializeField]
    private InputField _ipField;

    [SerializeField]
    private InputField _nameField;

    [SerializeField]
    private Text _disconnectInfoField;

    string ip;
    int port;

    private void Awake()
    {
        _ipField.text = NetUtils.GetLocalIp(LocalAddrType.IPv4);
    }

    public void OnHostClick()
    {
        SceneManager.LoadScene(
            "Server",
            new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics2D)
        );
        OnConnectClick();
    }

    private void OnDisconnected(DisconnectInfo info)
    {
        _uiObject.SetActive(true);
        _disconnectInfoField.text = info.Reason.ToString();
    }

    public void OnConnectClick()
    {
        _uiObject.SetActive(false);
        ParseAddress();
        _clientLogic.SetName(_nameField.text);
        _clientLogic.Connect(ip, port, OnDisconnected);
    }

    private void ParseAddress()
    {
        string text = _ipField.text;
        int index = text.LastIndexOf(':');

        if (index > 0)
        {
            ip = text.Substring(0, index);
            int.TryParse(text.Substring(index + 1), out port);
            Debug.Log($"IP: {ip} | Port: {port}");
        }
    }
}
