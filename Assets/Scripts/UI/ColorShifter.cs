using UnityEngine;
using UnityEngine.UI;

public class ColorShifter : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _hueSpeed;
    [SerializeField] private AnimationCurve _lightnessCurve;
    [SerializeField] private float _lightnessSpeed;

    private float _color = 0;
    private float _lightness = 0;

    private void Update()
    {
        _color += _hueSpeed * Time.deltaTime;
        _color %= 1;

        _lightness += _lightnessSpeed * Time.deltaTime;
        _lightness %= 1;

        _image.color = Color.HSVToRGB(_color, 1, _lightnessCurve.Evaluate(_lightness));
    }
}
