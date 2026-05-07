using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioTVManager : MonoBehaviour
{
    public float inicio = 0f;
    public AudioSource audioSource;
    public bool PlaySound;
    public bool StopSound;
    public float fadeInTime = 2f;
    public bool fadethatIn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Referencia al audiosource
        audioSource = GetComponent<AudioSource>();
        //audioSource.PlayDelayed(0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlaySound)
        {
            RandomizeBeforePlaying();
            PlaySound = false;
        }
        else if (StopSound)
        {
            StopPlaying();
            StopSound = false;
        }
        else if (fadethatIn)
        {
            StartCoroutine(FadeIn());
            fadethatIn = false;
        }
    }

    //Cambios aleatorios en el sonido para evitar que se canse el oido
    void RandomizeBeforePlaying()
    {
        audioSource.pitch = Random.Range(.97f, 1.03f);
        audioSource.Play();
    }

    void StopPlaying()
    {
        audioSource.Stop();
    }

    //Corutina para FadeIn y FadeOut
    IEnumerator FadeIn()
    {
        for (decimal i = 0m; i <= 1; i += 0.1m)
        {
            audioSource.volume = (float)i;
            yield return new WaitForSeconds(fadeInTime / 100);
        }
    }
}
