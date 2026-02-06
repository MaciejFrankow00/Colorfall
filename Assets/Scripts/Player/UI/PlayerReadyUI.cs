using System;
using TMPro;
using UnityEngine;

public class PlayerReadyUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _readyTMP;
    [SerializeField] private ColorsDatabase _colorsDatabase;

    [SerializeField] private string _readyText = "Ready";
    [SerializeField] private string _notReadyText = "Not ready";

    void Start()
    {
        _player.OnPlayerChange += Player_OnPlayerChange;

        Gameplayer.Instance.OnGameStart += Gameplayer_OnGameStart;
        Gameplayer.Instance.OnGameStop += Gameplayer_OnGameStop;

        Player_OnPlayerChange();
    }

    void OnDestroy()
    {
        _player.OnPlayerChange -= Player_OnPlayerChange;

        Gameplayer.Instance.OnGameStart -= Gameplayer_OnGameStart;
        Gameplayer.Instance.OnGameStop -= Gameplayer_OnGameStop;
    }

    private void Player_OnPlayerChange()
    {
        _readyTMP.text = _player.isReady ? _readyText : _notReadyText;
        _readyTMP.color = _player.isReady ? _colorsDatabase.GetColor(_player.ColorId).color : Color.black;
    }

    private void Gameplayer_OnGameStart()
    {
        _readyTMP.gameObject.SetActive(false);
    }

    private void Gameplayer_OnGameStop()
    {
        _readyTMP.gameObject.SetActive(true);
    }
}
