
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayControllerUI : MonoBehaviour
{
    [SerializeField] private Gameplayer _gameplayer;
    [SerializeField] private ColorsDatabase _colorsDatabase;

    [Header("UI representers")]
    [SerializeField] private Image _currentColorImage;
    [SerializeField] private TextMeshProUGUI _countdownTMP;

    private void Awake()
    {
        _gameplayer.OnColorSet += Gameplayer_OnColorSet;
        _gameplayer.OnCountdownChange += Gameplayer_OnCountdownChange;
        _gameplayer.OnGameStop += Gameplayer_OnGameStop;
    }

    private void OnDestroy()
    {
        _gameplayer.OnColorSet -= Gameplayer_OnColorSet;
        _gameplayer.OnCountdownChange -= Gameplayer_OnCountdownChange;
        _gameplayer.OnGameStop -= Gameplayer_OnGameStop;
    }

    private void Gameplayer_OnColorSet(int obj)
    {
        _currentColorImage.color = _colorsDatabase.GetColor(obj).color;
    }

    private void Gameplayer_OnCountdownChange(int obj)
    {
        _countdownTMP.text = obj.ToString();
    }

    private void Gameplayer_OnGameStop()
    {
        _currentColorImage.color = Color.white;
    }
}
