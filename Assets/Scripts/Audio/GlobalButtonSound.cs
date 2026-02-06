using UnityEngine;

public class GlobalButtonSound : MonoBehaviour
{
    private static GlobalButtonSound _instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void Awake()
    {
        _instance = this;
    }
    public static void Play()
    {
        _instance._audioSource.PlayOneShot(_instance._audioClip);
    }
}
