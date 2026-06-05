using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderSound : MonoBehaviour
{
    [Header("Sonido del slider")]
    public AudioSource audioSource;
    public AudioClip moveClip;
    [Range(0f, 1f)] public float volume = 0.65f;
    public float minInterval = 0.04f;

    private Slider slider;
    private float lastPlayTime;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
    }

    private void OnEnable()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        slider.onValueChanged.AddListener(PlayMoveSound);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(PlayMoveSound);
    }

    private void PlayMoveSound(float value)
    {
        if (audioSource == null || moveClip == null)
        {
            return;
        }

        if (Time.unscaledTime - lastPlayTime < minInterval)
        {
            return;
        }

        lastPlayTime = Time.unscaledTime;
        audioSource.PlayOneShot(moveClip, volume);
    }
}
