using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;

    private static AudioSource npcAudioSource;

    private float fadeTime = 0.25f;

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        npcAudioSource = GetComponent<AudioSource>();
    }

    public static void PlayNpc(AudioClip audioNpc, float pitch = 1f)
    {
        npcAudioSource.Stop();
        npcAudioSource.pitch = pitch;
        // npcAudioSource.PlayOneShot(audioNpc);


        if (audioNpc != null)
        {
            npcAudioSource.clip = audioNpc;
            npcAudioSource.Play();
        }

    }

    IEnumerator FadeOut()
    {
        if(npcAudioSource.clip != null && npcAudioSource.isPlaying)
        {
            float startVolume = npcAudioSource.volume;

            while (npcAudioSource.volume > 0)
            {
                npcAudioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            npcAudioSource.volume = 0;
            npcAudioSource.Stop();
            npcAudioSource.volume = startVolume;
        }

    }

    public static void StopNpc()
    {
        instance.StartCoroutine(instance.FadeOut());
    }

  


}
