using UnityEngine;
using UnityEngine.Audio;

public class BookSoundManager : MonoBehaviour
{
    private AudioSource audioBook;

    [SerializeField] private AudioClip dropBook;
    [SerializeField] private AudioClip grabBook;
    public bool PlaySound;
    public bool StopSound;
    public float inicio = 0f;
    public void Start()
    {
        audioBook = GetComponent<AudioSource>();
    }
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
    }

    void RandomizeBeforePlaying()
    {
        audioBook.pitch = Random.Range(.97f, 1.03f);
        audioBook.Play();
    }

    void StopPlaying()
    {
        audioBook.Stop();
    }

    public void EnterSocket()
    {
        audioBook.clip = dropBook;
        PlaySound = true;
    }

    public void ExitSocket()
    {
        audioBook.clip = grabBook;
        PlaySound = true;
    }


}
