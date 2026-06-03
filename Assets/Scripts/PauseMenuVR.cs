using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class PauseMenuVR : MonoBehaviour
{
    private const string GameplaySceneName = "0 (1)";
    private const string MainMenuSceneName = "Menu";

    private static PauseMenuVR instance;

    private GameObject canvasObject;
    private GameObject panelObject;
    private GameObject pauseButtonObject;
    private Sprite pauseButtonSprite;
    private Sprite panelSprite;
    private Sprite resumeButtonSprite;
    private Sprite mainMenuButtonSprite;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        if (instance != null)
        {
            return;
        }

        GameObject managerObject = new GameObject("Pause Menu VR");
        instance = managerObject.AddComponent<PauseMenuVR>();
        DontDestroyOnLoad(managerObject);
        instance.HandleSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        DestroyMenu();

        if (scene.name == GameplaySceneName)
        {
            StartCoroutine(CreateMenuAfterFrame());
        }
    }

    private IEnumerator CreateMenuAfterFrame()
    {
        yield return null;

        Camera camera = Camera.main;
        if (camera == null)
        {
            yield break;
        }

        LoadSprites();

        canvasObject = new GameObject("VR Pause Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(TrackedDeviceGraphicRaycaster));
        canvasObject.transform.SetParent(camera.transform, false);
        canvasObject.transform.localPosition = new Vector3(0f, 0f, 1.4f);
        canvasObject.transform.localRotation = Quaternion.identity;
        canvasObject.transform.localScale = Vector3.one * 0.0018f;

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = camera;
        canvas.sortingOrder = 50;

        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(900f, 520f);

        Button menuButton = CreateButton(canvasObject.transform, "Boton Pausa", "PAUSA", new Vector2(-450f, 260f), new Vector2(190f, 58f), pauseButtonSprite);
        pauseButtonObject = menuButton.gameObject;
        pauseButtonObject.SetActive(false);
        menuButton.onClick.AddListener(OpenPauseMenu);

        panelObject = CreatePanel(canvasObject.transform);
        panelObject.SetActive(false);

        Button resumeButton = CreateButton(panelObject.transform, "Boton Reanudar", "REANUDAR", new Vector2(0f, 48f), new Vector2(320f, 76f), resumeButtonSprite);
        resumeButton.onClick.AddListener(ResumeGame);

        Button menuPrincipalButton = CreateButton(panelObject.transform, "Boton Menu Principal", "MENU PRINCIPAL", new Vector2(0f, -58f), new Vector2(320f, 76f), mainMenuButtonSprite);
        menuPrincipalButton.onClick.AddListener(ReturnToMainMenu);

        StartCoroutine(ShowPauseButtonWhenInstructionsClose());
    }

    private void LoadSprites()
    {
        pauseButtonSprite = Resources.Load<Sprite>("Pause UI/Boton Pausa");
        panelSprite = Resources.Load<Sprite>("Pause UI/Panel Pausa");
        resumeButtonSprite = Resources.Load<Sprite>("Pause UI/Boton Reanudar");
        mainMenuButtonSprite = Resources.Load<Sprite>("Pause UI/Boton Menu Principal");
    }

    private GameObject CreatePanel(Transform parent)
    {
        GameObject panel = new GameObject("Panel Pausa", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(parent, false);

        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(640f, 380f);
        rectTransform.anchoredPosition = Vector2.zero;

        Image image = panel.GetComponent<Image>();
        image.sprite = panelSprite;
        image.preserveAspect = true;
        image.color = panelSprite != null ? Color.white : new Color(0.08f, 0.05f, 0.03f, 0.92f);

        return panel;
    }

    private Button CreateButton(Transform parent, string objectName, string label, Vector2 position, Vector2 size, Sprite sprite)
    {
        GameObject buttonObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        rectTransform.anchoredPosition = position;

        Image image = buttonObject.GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = true;
        image.color = sprite != null ? Color.white : new Color(0.62f, 0.23f, 0.12f, 0.95f);

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.None;

        ButtonFeedback feedback = buttonObject.AddComponent<ButtonFeedback>();
        feedback.useColorTint = false;
        feedback.disableButtonColorTint = true;
        feedback.showAccent = false;
        feedback.hoverOffset = Vector2.zero;
        feedback.hoverScale = 1.08f;
        feedback.pressedScale = 0.95f;
        feedback.animationTime = 0.1f;

        if (sprite != null)
        {
            return button;
        }

        GameObject textObject = new GameObject("Texto", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObject.transform.SetParent(buttonObject.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textObject.GetComponent<Text>();
        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 26;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(1f, 0.88f, 0.55f, 1f);

        return button;
    }

    private void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        panelObject.SetActive(true);
    }

    private IEnumerator ShowPauseButtonWhenInstructionsClose()
    {
        float waitForInstructionsTime = 0f;

        while (Time.timeScale > 0f && waitForInstructionsTime < 4f)
        {
            waitForInstructionsTime += Time.unscaledDeltaTime;
            yield return null;
        }

        while (Time.timeScale <= 0f)
        {
            yield return null;
        }

        if (pauseButtonObject != null)
        {
            pauseButtonObject.SetActive(true);
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        panelObject.SetActive(false);
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuSceneName);
    }

    private void DestroyMenu()
    {
        if (canvasObject != null)
        {
            Destroy(canvasObject);
            canvasObject = null;
            panelObject = null;
            pauseButtonObject = null;
        }
    }
}
