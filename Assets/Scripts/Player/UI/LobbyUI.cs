using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Gameplayer _gameplayer;
    [SerializeField] private Button _button;

    void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
        _gameplayer.OnGameStart += Gameplayer_OnGameStart;
        _gameplayer.OnGameStop += Gameplayer_OnGameStop;
    }

    void OnDestroy()
    {
        _button.onClick.AddListener(OnButtonClick);
        _gameplayer.OnGameStart += Gameplayer_OnGameStart;
        _gameplayer.OnGameStop += Gameplayer_OnGameStop;
    }

    private void OnButtonClick()
    {
        Player.local.CmdToggleReady();
    }

    private void Gameplayer_OnGameStart()
    {
        _button.gameObject.SetActive(false);
    }

    private void Gameplayer_OnGameStop()
    {
        _button.gameObject.SetActive(true);
    }
}
