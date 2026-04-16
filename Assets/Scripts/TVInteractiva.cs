using UnityEngine;

public class TVInteractiva : MonoBehaviour
{
    public GameObject pantallaEncendida;
    public GameObject vhsInsertadoVisual;

    public GameObject efectoGlitch; // 🔥 NUEVO
    public Light luzTV;             // 🔥 NUEVO
    public GameObject uiRecolectar;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("VHS"))
        {
            activado = true;

            Debug.Log("VHS insertado");

            // Desactivar VHS real
            other.gameObject.SetActive(false);

            // Mostrar VHS en el televisor
            vhsInsertadoVisual.SetActive(true);

            // Encender pantalla
            pantallaEncendida.SetActive(true);

            // 🔥 ACTIVAR GLITCH
            if (efectoGlitch != null)
                efectoGlitch.SetActive(true);

            // 🔥 ACTIVAR LUZ
            if (luzTV != null)
                luzTV.intensity = 1.5f;

            uiRecolectar.SetActive(true);

        }
    }
}