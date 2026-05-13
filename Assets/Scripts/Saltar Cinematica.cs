using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SaltarCinematica : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += FinDelVideo;
        videoPlayer.Play();
    }

    void FinDelVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SkipVideo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}