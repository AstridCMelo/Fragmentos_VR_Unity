using UnityEngine;

public class BookSoundManager : MonoBehaviour
{
    private AudioSource audioBook;

    [SerializeField] private AudioClip dropBook;
    [SerializeField] private AudioClip grabBook;
    public void Start()
    {
        audioBook = GetComponent<AudioSource>();
    }


    public void EnterSocket()
    {

    }

    public void ExitSocket()
    {

    }


}
