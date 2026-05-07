using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class RadioController : MonoBehaviour, IChangeTrack
{
    [SerializeField] private AudioClip noiseTrack;
    [SerializeField] private RadioTrack[] audioTracks;

    [SerializeField] private AudioSource noiseAudioSource;
    [SerializeField] private AudioSource radioAudioSource;

    private int trackIndex;
    private float indicatorPos;

    private float tolerace_track_max;
    [SerializeField] private float tolerace_indicator = 0.01f;
    private bool movementSense = true;

    private float lastCenter = 0f;

    void Start()
    {
        trackIndex = -1;
        radioAudioSource.clip = noiseTrack;

        noiseAudioSource.pitch = UnityEngine.Random.Range(.97f, 1.03f);
        noiseAudioSource.clip = noiseTrack;

        PlayAudio(noiseAudioSource);
        PlayAudio(radioAudioSource);

    }
    public void UpdateTrack(int index)
    {
        radioAudioSource.clip = audioTracks[index].trackAudioClip;
        PlayAudio(radioAudioSource);
    }

    public void IndicatorTrack(float center, float halfWidth)
    {
        ForwardBackMovement(center);

        tolerace_track_max = halfWidth;

        float distance = Mathf.Abs(center - indicatorPos);
        float deltaVolume = Mathf.InverseLerp(0, tolerace_track_max, distance);

        Debug.Log("distance" + distance);

        radioAudioSource.volume = 1f - deltaVolume;

        noiseAudioSource.pitch = UnityEngine.Random.Range(.97f, 1.03f);
        noiseAudioSource.volume = deltaVolume;

        lastCenter = center;
    }

    public void NoiseWithoutTrack()
    {
        noiseAudioSource.pitch = UnityEngine.Random.Range(.97f, 1.03f);
        noiseAudioSource.volume = UnityEngine.Random.Range(.9f, 1f); ;

        radioAudioSource.volume = 0f;
    }

    public void MovementSense(bool forward, float indicatorPosition)
    {
        movementSense = forward;
        indicatorPos = indicatorPosition;
    }

    public void ForwardBackMovement(float center)
    {
        if (lastCenter != center)
        {
            if (movementSense)
            {
                ForwardTrack();
            }
            else
            {
                BackTrack();
            }
        }
        else
        {

        }
    }
    

    public void PlayAudio(AudioSource audioSource)
    {
        audioSource.Play();
    }

    public void StopAudio(AudioSource audioSource)
    {
        radioAudioSource.Stop();
    }

    public void ForwardTrack()
    {
        if (trackIndex < audioTracks.Length-1)
        {
            trackIndex++;
            UpdateTrack(trackIndex);
        }
    }

    public void BackTrack()
    {
        if(trackIndex >= 1)
        {
            trackIndex--;
            UpdateTrack(trackIndex);
        }
    }

}
