using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TapetePortalVR : MonoBehaviour
{
    [Header("Cambio de Escena")]
#if UNITY_EDITOR
    public SceneAsset escenaDestino;
#endif

    [HideInInspector]
    [SerializeField] private string nombreEscenaGuardada;

    [Header("Pruebas en Editor")]
    public bool activarTeleportYa = false;

    private bool yaSeUso = false;

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
        // Truco para probar rápido con el checkbox
        if (activarTeleportYa)
        {
            activarTeleportYa = false;
            RegresarAEscena();
        }
    }

    // Este método lo llamará el XR Simple Interactable
    public void RegresarAEscena()
    {
        if (!yaSeUso)
        {
            yaSeUso = true;

            if (!string.IsNullOrEmpty(nombreEscenaGuardada))
            {
                SceneManager.LoadScene(nombreEscenaGuardada);
            }
            else
            {
                Debug.LogWarning("¡No has arrastrado la escena de regreso al Inspector del tapete!");
                yaSeUso = false;
            }
        }
    }
}