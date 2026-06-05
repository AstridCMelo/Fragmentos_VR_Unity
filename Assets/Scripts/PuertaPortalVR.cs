using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor; // Necesario para poder usar el tipo Object en el editor
#endif

public class PuertaPortalVR : MonoBehaviour
{
    [Header("Referencias de la Puerta")]
    public GameObject objetoBisagra;
    public AudioSource audioSourcePuerta;

    [Header("Configuración de Movimiento")]
    public float anguloApertura = 90f;
    public float velocidadApertura = 2f;

    [Header("Cambio de Escena")]
    // Cambiamos el string por un Objeto para que puedas ARRASTRAR la escena
#if UNITY_EDITOR
    public SceneAsset escenaDestino;
#endif

    [HideInInspector]
    [SerializeField] private string nombreEscenaGuardada;

    [Header("Pruebas en Editor")]
    public bool activarPuertaYa = false;

    private bool yaSeAbrio = false;

    // Al guardar o cambiar datos en el inspector, aseguramos el nombre de la escena
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (escenaDestino != null)
        {
            nombreEscenaGuardada = escenaDestino.name;
        }
#endif
    }

    void Update()
    {
        if (activarPuertaYa)
        {
            activarPuertaYa = false;
            ActivarPortal();
        }
    }

    public void ActivarPortal()
    {
        if (!yaSeAbrio)
        {
            yaSeAbrio = true;

            if (audioSourcePuerta != null)
            {
                audioSourcePuerta.Play();
            }

            StartCoroutine(AbrirPuertaSuave());
        }
    }

    IEnumerator AbrirPuertaSuave()
    {
        Transform transformA_Rotar = objetoBisagra != null ? objetoBisagra.transform : transform;
        Quaternion rotacionInicial = transformA_Rotar.localRotation;

        Quaternion rotacionFinal = Quaternion.Euler(
            transformA_Rotar.localRotation.eulerAngles.x,
            transformA_Rotar.localRotation.eulerAngles.y + anguloApertura,
            transformA_Rotar.localRotation.eulerAngles.z
        );

        float tiempo = 0;

        while (tiempo < 1)
        {
            tiempo += Time.deltaTime * velocidadApertura;
            transformA_Rotar.localRotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, tiempo);
            yield return null;
        }

        // Carga la escena usando el nombre del objeto arrastrado
        if (!string.IsNullOrEmpty(nombreEscenaGuardada))
        {
            SceneManager.LoadScene(nombreEscenaGuardada);
        }
        else
        {
            Debug.LogWarning("¡No has arrastrado ninguna escena al Inspector!");
        }
    }
}