using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsUI : MonoBehaviour
{
    [Header("Data objects")]
    [SerializeField] private Gameplayer _gameplayer;
    [SerializeField] private Image _winnerImage;

    [Header("UI objects")]
    [SerializeField] private GameObject _uiContainer;
    [SerializeField] private ColorsDatabase _colorsDatabase;
    [SerializeField] private TextMeshProUGUI _winnerTMP;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _retryButton;

    [Header("Text config")]
    [SerializeField] private string _winnerPrefix = "The winner is";
    [SerializeField] private string _winnerSuffix = "!";
    [SerializeField] private string _noWinnersText = "No winners - skill issue";

    private void Awake()
    {
        _uiContainer.SetActive(false);

        _gameplayer.OnWinner += Gameplayer_OnWinner;
        _menuButton.onClick.AddListener(MenuButton_OnClick);
        _retryButton.onClick.AddListener(RetryButton_OnClick);
    }


    private void OnDestroy()
    {
        _gameplayer.OnWinner -= Gameplayer_OnWinner;
        _menuButton.onClick.RemoveListener(MenuButton_OnClick);
        _retryButton.onClick.AddListener(RetryButton_OnClick);
    }

    private void Gameplayer_OnWinner(Player player)
    {
        _uiContainer.SetActive(true);
        if(player == null)
        {
            _winnerImage.gameObject.SetActive(false);
            _winnerTMP.text = _noWinnersText;
        }
        else
        {
            _winnerImage.gameObject.SetActive(true);
            _winnerImage.color = _colorsDatabase.GetColor(player.ColorId).color;
            _winnerTMP.text = $"{_winnerPrefix} {_colorsDatabase.GetColor(player.ColorId).colorName} {_winnerSuffix}";
        }
    }

    private void MenuButton_OnClick()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }
    }

    private void RetryButton_OnClick()
    {
        _uiContainer.SetActive(false);
    }
}
