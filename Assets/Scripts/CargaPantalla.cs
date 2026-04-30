using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CargarPantalla : MonoBehaviour
{
    [SerializeField] private Slider sliderProgreso;
    [SerializeField] private float tiempoMinimoCarga = 3f;

    private void Start()
    {
        StartCoroutine(Carga());
    }

    private IEnumerator Carga()
    {
        sliderProgreso.gameObject.SetActive(true);

        AsyncOperation operacionCarga =
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        operacionCarga.allowSceneActivation = false;

        float tiempoTranscurrido = 0f;

        while (!operacionCarga.isDone)
        {
            tiempoTranscurrido += Time.deltaTime;

            float progresoReal =
                Mathf.Clamp01(operacionCarga.progress / 0.9f);

            float progresoTiempo =
                Mathf.Clamp01(tiempoTranscurrido / tiempoMinimoCarga);

            sliderProgreso.value =
                Mathf.Min(progresoReal, progresoTiempo);

            if (progresoReal >= 1f && progresoTiempo >= 1f)
                break;

            yield return null;
        }

        sliderProgreso.value = 1f;

        yield return new WaitForSeconds(0.5f);

        operacionCarga.allowSceneActivation = true;
    }
}