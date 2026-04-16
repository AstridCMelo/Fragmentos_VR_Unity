using UnityEngine;

public class TVGlitch : MonoBehaviour
{
    public Light luzTV;
    public Renderer pantalla;

    private float tiempo;

    void Update()
    {
        tiempo += Time.deltaTime;

        // Parpadeo de luz
        if (luzTV != null)
        {
            luzTV.intensity = Random.Range(0.5f, 2f);
        }

        // Cambio de color (simula glitch)
        if (pantalla != null)
        {
            Color color = new Color(
                Random.Range(0.7f, 1f),
                Random.Range(0.7f, 1f),
                Random.Range(0.7f, 1f)
            );

            pantalla.material.color = color;
        }
    }
}