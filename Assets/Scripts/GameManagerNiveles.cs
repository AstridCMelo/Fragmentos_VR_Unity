using UnityEngine;
using TMPro; // Si usas texto para el mensaje

public class GameManagerNiveles : MonoBehaviour
{
    public int fragmentosRecogidos = 0;
    public int totalFragmentosNivel1 = 3;

    [Header("Referencias de Escena")]
    public GameObject puertaCuarto; // La puerta que se abrirá
    public TextMeshProUGUI mensajePantalla; // Para el mensaje de "Puerta Abierta"

    public void AñadirFragmento()
    {
        fragmentosRecogidos++;
        Debug.Log("Fragmentos: " + fragmentosRecogidos);

        if (fragmentosRecogidos >= totalFragmentosNivel1)
        {
            AbrirSiguienteZona();
        }
    }

    void AbrirSiguienteZona()
    {
        // 1. Mostrar mensaje en pantalla (Canvas VR)
        if (mensajePantalla != null)
        {
            mensajePantalla.text = "Recuerdo reconstruido. El cuarto está abierto...";
            mensajePantalla.gameObject.SetActive(true);
        }

        // 2. Abrir la puerta (puedes usar una animación o simplemente rotarla)
        // Aquí rotamos la puerta 90 grados en el eje Y
        puertaCuarto.transform.localRotation = Quaternion.Euler(0, 90, 0);

        // 3. Opcional: Sonido de puerta abriéndose
        AudioSource audio = puertaCuarto.GetComponent<AudioSource>();
        if (audio) audio.Play();
    }
}