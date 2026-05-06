using UnityEngine;

public class RadioController : MonoBehaviour
{

    [SerializeField] private RadioTrack[] audioTracks;
    private int trackIndex;

    private AudioSource radioAudioSource;

    void Start()
    {
        radioAudioSource = GetComponent<AudioSource>();

        trackIndex = 0;
        radioAudioSource.clip = audioTracks[trackIndex].trackAudioClip;
    }
    public void UpdateTrack(int index)
    {
        radioAudioSource.clip = audioTracks[index].trackAudioClip;
    }

    public void PlayAudio()
    {
        radioAudioSource.Play();
    }

    public void StopAudio()
    {
        radioAudioSource.Stop();
    }

    public void ForwardTrack()
    {
        trackIndex++;
        UpdateTrack(trackIndex);
    }

    public void BackTrack()
    {
        trackIndex--;
        UpdateTrack(trackIndex);
    }

}
