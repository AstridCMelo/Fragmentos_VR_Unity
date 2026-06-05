using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class SaltarCinematica : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    [SerializeField] private GameObject botonSkip;
    [SerializeField] private float retardoMostrarBoton = 3f;
    [SerializeField] private float duracionPopBoton = 0.32f;
    [SerializeField] private float escalaPopBoton = 1.14f;

    private RectTransform botonSkipRect;
    private CanvasGroup botonSkipCanvasGroup;
    private Vector3 escalaOriginalBoton;
    private bool cambiandoEscena;

    void Start()
    {
        PrepararBotones();

        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += FinDelVideo;
            videoPlayer.Play();
        }

        StartCoroutine(MostrarBotonSkipConRetardo());
    }

    void FinDelVideo(VideoPlayer vp)
    {
        CargarSiguienteEscena();
    }

    public void SkipVideo()
    {
        CargarSiguienteEscena();
    }

    private void PrepararBotones()
    {
        Button[] botones = FindObjectsOfType<Button>(true);

        foreach (Button boton in botones)
        {
            boton.transition = Selectable.Transition.None;

            ButtonFeedback feedback = boton.GetComponent<ButtonFeedback>();
            if (feedback == null)
            {
                feedback = boton.gameObject.AddComponent<ButtonFeedback>();
            }

            feedback.useColorTint = false;
            feedback.disableButtonColorTint = true;
            feedback.showAccent = false;
            feedback.hoverOffset = Vector2.zero;
            feedback.hoverScale = 1.08f;
            feedback.pressedScale = 0.95f;
            feedback.animationTime = 0.1f;

            if (botonSkip == null)
            {
                botonSkip = boton.gameObject;
            }
        }

        if (botonSkip == null)
        {
            return;
        }

        botonSkipRect = botonSkip.GetComponent<RectTransform>();
        botonSkipCanvasGroup = botonSkip.GetComponent<CanvasGroup>();

        if (botonSkipCanvasGroup == null)
        {
            botonSkipCanvasGroup = botonSkip.AddComponent<CanvasGroup>();
        }

        if (botonSkipRect != null)
        {
            escalaOriginalBoton = botonSkipRect.localScale;
        }

        botonSkip.SetActive(false);
    }

    private IEnumerator MostrarBotonSkipConRetardo()
    {
        yield return new WaitForSecondsRealtime(retardoMostrarBoton);

        if (cambiandoEscena || botonSkip == null)
        {
            yield break;
        }

        botonSkip.SetActive(true);
        yield return AnimarEntradaBotonSkip();
    }

    private IEnumerator AnimarEntradaBotonSkip()
    {
        if (botonSkipRect == null || botonSkipCanvasGroup == null)
        {
            yield break;
        }

        float tiempo = 0f;
        Vector3 escalaEntrada = escalaOriginalBoton * 0.75f;
        Vector3 escalaMaxima = escalaOriginalBoton * escalaPopBoton;

        botonSkipCanvasGroup.alpha = 0f;
        botonSkipRect.localScale = escalaEntrada;

        while (tiempo < duracionPopBoton)
        {
            tiempo += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(tiempo / duracionPopBoton);
            botonSkipCanvasGroup.alpha = 1f - Mathf.Pow(1f - t, 3f);

            if (t < 0.55f)
            {
                float crecer = Mathf.Clamp01(t / 0.55f);
                crecer = 1f - Mathf.Pow(1f - crecer, 3f);
                botonSkipRect.localScale = Vector3.LerpUnclamped(escalaEntrada, escalaMaxima, crecer);
            }
            else
            {
                float volver = Mathf.Clamp01((t - 0.55f) / 0.45f);
                volver = 1f - Mathf.Pow(1f - volver, 3f);
                botonSkipRect.localScale = Vector3.LerpUnclamped(escalaMaxima, escalaOriginalBoton, volver);
            }

            yield return null;
        }

        botonSkipCanvasGroup.alpha = 1f;
        botonSkipRect.localScale = escalaOriginalBoton;
    }

    private void CargarSiguienteEscena()
    {
        if (cambiandoEscena)
        {
            return;
        }

        cambiandoEscena = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
