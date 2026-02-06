
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayControllerUI : MonoBehaviour
{
    [SerializeField] private Gameplayer _gameplayer;
    [SerializeField] private ColorsDatabase _colorsDatabase;

    [Header("UI representers")]
    [SerializeField] private Image[] _currentColorImages;
    [SerializeField] private TextMeshProUGUI _countdownTMP;
    [SerializeField] private TextMeshProUGUI _roundTMP;

    private void Awake()
    {
        _gameplayer.OnColorSet += Gameplayer_OnColorSet;
        _gameplayer.OnCountdownChange += Gameplayer_OnCountdownChange;
        _gameplayer.OnGameStart += Gameplayer_OnGameStart;
        _gameplayer.OnGameStop += Gameplayer_OnGameStop;
        _gameplayer.OnRoundChange += Gameplayer_OnRoundChange;

        _countdownTMP.gameObject.SetActive(false);
        _roundTMP.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameplayer.OnColorSet -= Gameplayer_OnColorSet;
        _gameplayer.OnCountdownChange -= Gameplayer_OnCountdownChange;
        _gameplayer.OnGameStart += Gameplayer_OnGameStart;
        _gameplayer.OnGameStop -= Gameplayer_OnGameStop;
    }

    private void Gameplayer_OnColorSet(int obj)
    {
        for(int i = 0;i < _currentColorImages.Length; i++)
        {
            _currentColorImages[i].color =_colorsDatabase.GetColor(obj).color;
        }
    }

    private void Gameplayer_OnCountdownChange(int obj)
    {
        _countdownTMP.text = obj.ToString();
    }
    
    private void Gameplayer_OnGameStart()
    {
        _countdownTMP.gameObject.SetActive(true);
        _roundTMP.gameObject.SetActive(true);
    }

    private void Gameplayer_OnGameStop()
    {
        _roundTMP.gameObject.SetActive(false);
        _countdownTMP.gameObject.SetActive(false);
        for(int i = 0;i < _currentColorImages.Length; i++)
        {
            _currentColorImages[i].color = Color.white;
        }
    }

    private void Gameplayer_OnRoundChange(int obj)
    {
        _roundTMP.text = obj.ToString();
    }
}
