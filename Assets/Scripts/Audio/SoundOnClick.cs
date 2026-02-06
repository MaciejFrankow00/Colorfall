using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SoundOnClick : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        GlobalButtonSound.Play();
    }
}
