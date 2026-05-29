using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Progreso")]
    public int fragmentosActuales = 0;
    public int fragmentosTotales = 3;

    [Header("Interfaz")]
    public TextMeshProUGUI textoUI;
    public GameObject uiFinal;

    [Header("Configuración de Puerta")]
    public GameObject puertaCuarto; // Objeto puerta (bisagra)
    public float anguloApertura = 90f;
    public float velocidadApertura = 2f;

    void Awake()
    {
        instancia = this;
    }

    public void AgregarFragmento()
    {
        fragmentosActuales++;

        if (textoUI != null)
            textoUI.text = "Fragmentos: " + fragmentosActuales + "/" + fragmentosTotales;

        if (fragmentosActuales >= fragmentosTotales)
        {
            FinalJuego();
        }
    }

    void FinalJuego()
    {
        Debug.Log("RECUERDO COMPLETO");

        if (uiFinal != null)
            uiFinal.SetActive(true);

        if (puertaCuarto != null)
        {
            // Reproducir sonido si existe
            AudioSource audio = puertaCuarto.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }

            StartCoroutine(AbrirPuertaSuave());
            MiniGamesState.minigame1Completed = true;
        }
    }

    IEnumerator AbrirPuertaSuave()
    {
        Quaternion rotacionInicial = puertaCuarto.transform.localRotation;
        Quaternion rotacionFinal = Quaternion.Euler(0, anguloApertura, 0);
        float tiempo = 0;

        while (tiempo < 1)
        {
            tiempo += Time.deltaTime * velocidadApertura;
            puertaCuarto.transform.localRotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, tiempo);
            yield return null;
        }
    }
}