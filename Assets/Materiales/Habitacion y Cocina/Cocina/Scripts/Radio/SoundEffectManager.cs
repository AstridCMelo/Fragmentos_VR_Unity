using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;

    private static AudioSource npcAudioSource;

    private void Start()
    {
        npcAudioSource = GetComponent<AudioSource>();
    }

    public static void PlayNpc(AudioClip audioNpc, float pitch = 1f)
    {
        npcAudioSource.Stop();
        npcAudioSource.pitch = pitch;
        // npcAudioSource.PlayOneShot(audioNpc);
        npcAudioSource.clip = audioNpc;
        npcAudioSource.Play();
    }

    public static void StopNpc()
    {
        npcAudioSource.Stop();
    }
}
