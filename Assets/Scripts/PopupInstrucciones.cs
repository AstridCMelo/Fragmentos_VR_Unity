using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupInstrucciones : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject botonContinuar;
    [SerializeField] private float duracionEntrada = 0.45f;
    [SerializeField] private float escalaInicial = 0.88f;
    [SerializeField] private float desplazamientoInicialY = -35f;
    [SerializeField] private float duracionPopBoton = 0.32f;
    [SerializeField] private float escalaPopBoton = 1.18f;
    [SerializeField] private float escalaHoverBoton = 1.08f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoAparecerPopup;
    [SerializeField] private AudioClip sonidoAparecerBoton;
    [SerializeField] private AudioClip sonidoClickBoton;
    [SerializeField, Range(0f, 1f)] private float volumenPopup = 0.55f;
    [SerializeField, Range(0f, 1f)] private float volumenBoton = 0.75f;
    [SerializeField, Range(0f, 1f)] private float volumenClickBoton = 0.8f;

    private RectTransform popupRect;
    private RectTransform botonContinuarRect;
    private CanvasGroup popupCanvasGroup;
    private CanvasGroup botonContinuarCanvasGroup;
    private Vector3 escalaOriginal;
    private Vector2 posicionOriginal;
    private Vector3 escalaOriginalBoton;

    private void Start()
    {
        PrepararPopup();

        popup.SetActive(false);

        botonContinuar.SetActive(false);

        StartCoroutine(SecuenciaPopup());
    }

    private IEnumerator SecuenciaPopup()
    {
        // Espera antes de mostrar popup
        yield return new WaitForSeconds(3f);

        // Mostrar popup
        popup.SetActive(true);

        // Pausar juego
        Time.timeScale = 0f;

        ReproducirSonido(sonidoAparecerPopup, volumenPopup);
        yield return AnimarEntradaPopup();

        // Esperar 4 segundos reales
        yield return new WaitForSecondsRealtime(4f);

        // Mostrar botón continuar
        botonContinuar.SetActive(true);
        ReproducirSonido(sonidoAparecerBoton, volumenBoton);
        yield return AnimarBotonContinuar();
    }

    public void Continuar()
    {
        // Ocultar popup
        popup.SetActive(false);

        // Reanudar juego
        Time.timeScale = 1f;
    }

    private void PrepararPopup()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        popupRect = popup.GetComponent<RectTransform>();
        popupCanvasGroup = popup.GetComponent<CanvasGroup>();
        botonContinuarRect = botonContinuar.GetComponent<RectTransform>();
        botonContinuarCanvasGroup = botonContinuar.GetComponent<CanvasGroup>();

        if (popupCanvasGroup == null)
        {
            popupCanvasGroup = popup.AddComponent<CanvasGroup>();
        }

        if (botonContinuarCanvasGroup == null)
        {
            botonContinuarCanvasGroup = botonContinuar.AddComponent<CanvasGroup>();
        }

        if (popupRect == null)
        {
            return;
        }

        escalaOriginal = popupRect.localScale;
        posicionOriginal = popupRect.anchoredPosition;

        if (botonContinuarRect != null)
        {
            escalaOriginalBoton = botonContinuarRect.localScale;
        }

        PrepararHoverBotonContinuar();
    }

    private void PrepararHoverBotonContinuar()
    {
        ButtonFeedback feedback = botonContinuar.GetComponent<ButtonFeedback>();

        if (feedback == null)
        {
            feedback = botonContinuar.AddComponent<ButtonFeedback>();
        }

        feedback.hoverScale = escalaHoverBoton;
        feedback.hoverOffset = Vector2.zero;
        feedback.pressedScale = 0.95f;
        feedback.animationTime = 0.1f;
        feedback.SetAccentVisible(false);

        EventTrigger trigger = botonContinuar.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = botonContinuar.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry clickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        clickEntry.callback.AddListener(_ => ReproducirSonido(sonidoClickBoton, volumenClickBoton));
        trigger.triggers.Add(clickEntry);
    }

    private IEnumerator AnimarEntradaPopup()
    {
        if (popupRect == null || popupCanvasGroup == null)
        {
            yield break;
        }

        float tiempo = 0f;
        Vector2 posicionInicial = posicionOriginal + new Vector2(0f, desplazamientoInicialY);
        Vector3 escalaEntrada = escalaOriginal * escalaInicial;

        popupCanvasGroup.alpha = 0f;
        popupRect.localScale = escalaEntrada;
        popupRect.anchoredPosition = posicionInicial;

        while (tiempo < duracionEntrada)
        {
            tiempo += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(tiempo / duracionEntrada);
            float suavizado = 1f - Mathf.Pow(1f - t, 3f);

            popupCanvasGroup.alpha = suavizado;
            popupRect.localScale = Vector3.LerpUnclamped(escalaEntrada, escalaOriginal, suavizado);
            popupRect.anchoredPosition = Vector2.LerpUnclamped(posicionInicial, posicionOriginal, suavizado);

            yield return null;
        }

        popupCanvasGroup.alpha = 1f;
        popupRect.localScale = escalaOriginal;
        popupRect.anchoredPosition = posicionOriginal;
    }

    private IEnumerator AnimarBotonContinuar()
    {
        if (botonContinuarRect == null || botonContinuarCanvasGroup == null)
        {
            yield break;
        }

        float tiempo = 0f;
        Vector3 escalaEntrada = escalaOriginalBoton * 0.75f;
        Vector3 escalaMaxima = escalaOriginalBoton * escalaPopBoton;

        botonContinuarCanvasGroup.alpha = 0f;
        botonContinuarRect.localScale = escalaEntrada;

        while (tiempo < duracionPopBoton)
        {
            tiempo += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(tiempo / duracionPopBoton);
            float alpha = 1f - Mathf.Pow(1f - t, 3f);

            botonContinuarCanvasGroup.alpha = alpha;

            if (t < 0.55f)
            {
                float crecer = Mathf.Clamp01(t / 0.55f);
                crecer = 1f - Mathf.Pow(1f - crecer, 3f);
                botonContinuarRect.localScale = Vector3.LerpUnclamped(escalaEntrada, escalaMaxima, crecer);
            }
            else
            {
                float volver = Mathf.Clamp01((t - 0.55f) / 0.45f);
                volver = 1f - Mathf.Pow(1f - volver, 3f);
                botonContinuarRect.localScale = Vector3.LerpUnclamped(escalaMaxima, escalaOriginalBoton, volver);
            }

            yield return null;
        }

        botonContinuarCanvasGroup.alpha = 1f;
        botonContinuarRect.localScale = escalaOriginalBoton;
    }

    private void ReproducirSonido(AudioClip clip, float volumen)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip, volumen);
    }
}
