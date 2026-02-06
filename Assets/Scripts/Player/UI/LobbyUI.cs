using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Gameplayer _gameplayer;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _exitButton;

    void Awake()
    {
        _readyButton.onClick.AddListener(OnReadyButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _gameplayer.OnGameStart += Gameplayer_OnGameStart;
        _gameplayer.OnGameStop += Gameplayer_OnGameStop;
    }

    void OnDestroy()
    {
        _readyButton.onClick.AddListener(OnReadyButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _gameplayer.OnGameStart += Gameplayer_OnGameStart;
        _gameplayer.OnGameStop += Gameplayer_OnGameStop;
    }

    private void OnReadyButtonClick()
    {
        Player.local.CmdToggleReady();
    }

    private void OnExitButtonClick()
    {
        MainNetworkManager.singleton.StopHost();
        MainNetworkManager.singleton.StopClient();
    }

    private void Gameplayer_OnGameStart()
    {
        _readyButton.gameObject.SetActive(false);
    }

    private void Gameplayer_OnGameStop()
    {
        _readyButton.gameObject.SetActive(true);
    }
}
