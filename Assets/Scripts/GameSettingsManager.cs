using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingsManager : MonoBehaviour
{
    public const string MasterVolumeKey = "Settings.MasterVolume";
    public const string MusicVolumeKey = "Settings.MusicVolume";
    public const string BrightnessKey = "Settings.Brightness";
    public const string QualityKey = "Settings.Quality";

    private const float DefaultMasterVolume = 1f;
    private const float DefaultMusicVolume = 0.75f;
    private const float DefaultBrightness = 1f;

    public static GameSettingsManager Instance { get; private set; }

    public float MasterVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public float Brightness { get; private set; }

    private GameObject brightnessOverlay;
    private CanvasGroup brightnessOverlayGroup;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeSettingsManager()
    {
        EnsureExists();
    }

    public static GameSettingsManager EnsureExists()
    {
        if (Instance != null)
        {
            return Instance;
        }

        GameObject managerObject = new GameObject("Game Settings Manager");
        return managerObject.AddComponent<GameSettingsManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
        SceneManager.sceneLoaded += OnSceneLoaded;
        ApplySettings();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySettings();
    }

    public void SetMasterVolume(float value)
    {
        MasterVolume = Mathf.Clamp01(value);
        ApplyMasterVolume();
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        ApplyMusicVolume();
    }

    public void SetBrightness(float value)
    {
        Brightness = Mathf.Clamp(value, 0.35f, 1.35f);
        ApplyBrightness();
    }

    public void SetQuality(int qualityIndex)
    {
        int clampedQuality = Mathf.Clamp(qualityIndex, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(clampedQuality, true);
    }

    public void SetQualityPreset(int presetIndex)
    {
        int maxQualityIndex = QualitySettings.names.Length - 1;
        int mappedQuality = presetIndex switch
        {
            0 => 0,
            1 => Mathf.RoundToInt(maxQualityIndex * 0.33f),
            2 => Mathf.RoundToInt(maxQualityIndex * 0.66f),
            _ => maxQualityIndex
        };

        SetQuality(mappedQuality);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        PlayerPrefs.SetFloat(BrightnessKey, Brightness);
        PlayerPrefs.SetInt(QualityKey, QualitySettings.GetQualityLevel());
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, DefaultMasterVolume);
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, DefaultMusicVolume);
        Brightness = PlayerPrefs.GetFloat(BrightnessKey, DefaultBrightness);

        if (PlayerPrefs.HasKey(QualityKey))
        {
            SetQuality(PlayerPrefs.GetInt(QualityKey));
        }
    }

    private void ApplySettings()
    {
        ApplyMasterVolume();
        ApplyMusicVolume();
        ApplyBrightness();
    }

    private void ApplyMasterVolume()
    {
        AudioListener.volume = MasterVolume;
    }

    private void ApplyMusicVolume()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>(true);

        foreach (AudioSource source in sources)
        {
            if (source.name == "MusicManager")
            {
                source.volume = MusicVolume;
            }
        }
    }

    private void ApplyBrightness()
    {
        RenderSettings.ambientIntensity = Brightness;
        EnsureBrightnessOverlay();

        if (brightnessOverlayGroup == null)
        {
            return;
        }

        brightnessOverlayGroup.alpha = Brightness < 1f ? Mathf.InverseLerp(1f, 0.35f, Brightness) * 0.55f : 0f;
    }

    private void EnsureBrightnessOverlay()
    {
        if (brightnessOverlay != null)
        {
            return;
        }

        brightnessOverlay = new GameObject("Brightness Overlay", typeof(RectTransform));
        DontDestroyOnLoad(brightnessOverlay);

        Canvas canvas = brightnessOverlay.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32767;

        brightnessOverlayGroup = brightnessOverlay.AddComponent<CanvasGroup>();
        brightnessOverlayGroup.blocksRaycasts = false;
        brightnessOverlayGroup.interactable = false;

        UnityEngine.UI.Image overlayImage = brightnessOverlay.AddComponent<UnityEngine.UI.Image>();
        overlayImage.color = Color.black;
        overlayImage.raycastTarget = false;

        RectTransform rectTransform = brightnessOverlay.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
