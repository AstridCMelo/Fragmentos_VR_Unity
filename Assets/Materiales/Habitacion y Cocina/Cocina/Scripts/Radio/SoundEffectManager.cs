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
        npcAudioSource.pitch = pitch;
        npcAudioSource.PlayOneShot(audioNpc);
    }
}
