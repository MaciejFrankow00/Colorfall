using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbySearchUI : MonoBehaviour
{
    [SerializeField] private NetworkDiscovery _networkDiscovery;
    [SerializeField] private GameObject _serverButtonPrefab;
    [SerializeField] private Transform _serverButtonContainer;

    private readonly Dictionary<long, ServerResponse> _discoveredServers = new();
    private readonly Dictionary<long, GameObject> _instantiatedButtons = new();

    private void Awake()
    {
        _networkDiscovery.OnServerFound.AddListener(NetworkDiscovery_OnServerFound);
    }

    private void OnDestroy()
    {
        _networkDiscovery.OnServerFound.RemoveListener(NetworkDiscovery_OnServerFound);
    }

    public void StartDiscovery()
    {
        _networkDiscovery.StartDiscovery();
    }

    public void StopDiscovery()
    {
        _networkDiscovery.StopDiscovery();
        DestroyAllButtons();
    }

    private void NetworkDiscovery_OnServerFound(ServerResponse info)
    {
        if (_discoveredServers.ContainsKey(info.serverId)) return;
        
        _discoveredServers[info.serverId] = info;
        SpawnButton(info);
    }

    private void SpawnButton(ServerResponse info)
    {
        if (!_instantiatedButtons.ContainsKey(info.serverId))
        {
            var go = Instantiate(_serverButtonPrefab, _serverButtonContainer);
            _instantiatedButtons.Add(info.serverId, go);
            go.GetComponentInChildren<TextMeshProUGUI>().text = info.uri.ToString();
            go.GetComponent<Button>().onClick.AddListener(() => ServerButton_OnClick(info));
        }
    }

    private void DestroyAllButtons()
    {
        foreach(var button in _instantiatedButtons.Values)
        {
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(button);
        }
        if(_discoveredServers != null)
        {
            _discoveredServers.Clear();
        }
        if(_instantiatedButtons != null)
        {   
            _instantiatedButtons.Clear();
        }
    }

    private void ServerButton_OnClick(ServerResponse info)
    {
        Connect(info);
    }

    private void Connect(ServerResponse info)
    {
        StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }
}
