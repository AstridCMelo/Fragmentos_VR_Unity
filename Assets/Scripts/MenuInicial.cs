using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] private float retardoClickAntesDeCambiar = 0.25f;
    [SerializeField] private float retardoClickAntesDeSalir = 0.2f;
    [SerializeField] private GameObject panelOpciones;
    [SerializeField] private GameObject[] objetosMenuPrincipal;

    private bool accionEnCurso;
    private GameSettingsManager settingsManager;

    private void Start()
    {
        settingsManager = GameSettingsManager.EnsureExists();
        ConfigurarOpciones();
        MostrarMenuPrincipal();
    }

    public void Jugar()
    {
        if (accionEnCurso)
        {
            return;
        }

        accionEnCurso = true;
        StartCoroutine(CambiarEscenaDespuesDelClick());
    }

    public void Salir()
    {
        if (accionEnCurso)
        {
            return;
        }

        accionEnCurso = true;
        StartCoroutine(SalirDespuesDelClick());
    }

    public void MostrarOpciones()
    {
        SetMenuPrincipalVisible(false);

        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
        }
    }

    public void MostrarMenuPrincipal()
    {
        SetMenuPrincipalVisible(true);

        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    public void GuardarOpciones()
    {
        settingsManager.SaveSettings();
        MostrarMenuPrincipal();
    }

    private IEnumerator CambiarEscenaDespuesDelClick()
    {
        yield return new WaitForSecondsRealtime(retardoClickAntesDeCambiar);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator SalirDespuesDelClick()
    {
        yield return new WaitForSecondsRealtime(retardoClickAntesDeSalir);
        Debug.Log("Salir...");
        Application.Quit();
    }

    private void SetMenuPrincipalVisible(bool visible)
    {
        foreach (GameObject objeto in objetosMenuPrincipal)
        {
            if (objeto != null)
            {
                objeto.SetActive(visible);
            }
        }
    }

    private void ConfigurarOpciones()
    {
        if (panelOpciones == null)
        {
            return;
        }

        Slider sliderBrillo = BuscarEnPanel<Slider>("Slider Brillo");
        Slider sliderVolumenGeneral = BuscarEnPanel<Slider>("Slider Volumen General");
        Slider sliderMusica = BuscarEnPanel<Slider>("Slider Musica");

        ConfigurarSlider(sliderBrillo, 0.35f, 1.35f, settingsManager.Brightness, settingsManager.SetBrightness);
        ConfigurarSlider(sliderVolumenGeneral, 0f, 1f, settingsManager.MasterVolume, settingsManager.SetMasterVolume);
        ConfigurarSlider(sliderMusica, 0f, 1f, settingsManager.MusicVolume, settingsManager.SetMusicVolume);

        ConfigurarBoton("Boton Bajo", () => settingsManager.SetQualityPreset(0));
        ConfigurarBoton("Boton Medio", () => settingsManager.SetQualityPreset(1));
        ConfigurarBoton("Boton Alto", () => settingsManager.SetQualityPreset(2));
        ConfigurarBoton("Boton Ultra", () => settingsManager.SetQualityPreset(3));
        ConfigurarBoton("Boton Guardar", GuardarOpciones);
    }

    private void ConfigurarSlider(Slider slider, float valorMinimo, float valorMaximo, float valorInicial, UnityEngine.Events.UnityAction<float> callback)
    {
        if (slider == null)
        {
            return;
        }

        slider.minValue = valorMinimo;
        slider.maxValue = valorMaximo;
        slider.wholeNumbers = false;
        slider.SetValueWithoutNotify(Mathf.Clamp(valorInicial, valorMinimo, valorMaximo));
        slider.onValueChanged.AddListener(callback);
    }

    private void ConfigurarBoton(string nombre, UnityEngine.Events.UnityAction callback)
    {
        Button boton = BuscarEnPanel<Button>(nombre);

        if (boton == null)
        {
            return;
        }

        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(callback);
    }

    private T BuscarEnPanel<T>(string nombre) where T : Component
    {
        T[] componentes = panelOpciones.GetComponentsInChildren<T>(true);

        foreach (T componente in componentes)
        {
            if (componente.name == nombre)
            {
                return componente;
            }
        }

        return null;
    }
}
