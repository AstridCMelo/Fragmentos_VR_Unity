using System.Collections;
using UnityEngine;

public class PopupInstrucciones : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject botonContinuar;

    private void Start()
    {
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

        // Esperar 4 segundos reales
        yield return new WaitForSecondsRealtime(4f);

        // Mostrar botón continuar
        botonContinuar.SetActive(true);
    }

    public void Continuar()
    {
        // Ocultar popup
        popup.SetActive(false);

        // Reanudar juego
        Time.timeScale = 1f;
    }
}