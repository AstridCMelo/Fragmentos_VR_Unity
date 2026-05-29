using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    [Header("Motion")]
    public float hoverScale = 1.06f;
    public float pressedScale = 0.96f;
    public Vector2 hoverOffset = new Vector2(14f, 0f);
    public float animationTime = 0.12f;

    [Header("Color")]
    public bool useColorTint = false;
    public bool disableButtonColorTint = true;
    public Color hoverTint = new Color(1f, 0.78f, 0.42f, 1f);
    public Color pressedTint = new Color(1f, 0.52f, 0.28f, 1f);
    public Color accentColor = new Color(1f, 0.64f, 0.24f, 0.85f);
    public bool showAccent = false;

    private RectTransform rectTransform;
    private Button button;
    private Graphic targetGraphic;
    private Image accentImage;
    private Vector3 baseScale;
    private Vector2 basePosition;
    private Color baseColor;
    private Coroutine animationRoutine;
    private bool isHovering;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        targetGraphic = button?.targetGraphic;

        if (targetGraphic == null)
        {
            targetGraphic = GetComponent<Graphic>();
        }

        if (button != null && disableButtonColorTint)
        {
            button.transition = Selectable.Transition.None;
        }

        baseScale = rectTransform.localScale;
        basePosition = rectTransform.anchoredPosition;
        baseColor = targetGraphic != null ? targetGraphic.color : Color.white;

        if (showAccent)
        {
            CreateAccent();
            SetAccent(0f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        AnimateTo(hoverScale, hoverOffset, hoverTint, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        AnimateTo(1f, Vector2.zero, baseColor, 0f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AnimateTo(pressedScale, hoverOffset * 0.5f, pressedTint, 1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHovering)
        {
            AnimateTo(hoverScale, hoverOffset, hoverTint, 1f);
            return;
        }

        AnimateTo(1f, Vector2.zero, baseColor, 0f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        isHovering = true;
        AnimateTo(hoverScale, hoverOffset, hoverTint, 1f);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isHovering = false;
        AnimateTo(1f, Vector2.zero, baseColor, 0f);
    }

    public void SetAccentVisible(bool visible)
    {
        showAccent = visible;

        if (!showAccent)
        {
            if (accentImage != null)
            {
                Destroy(accentImage.gameObject);
                accentImage = null;
            }

            return;
        }

        if (accentImage == null)
        {
            CreateAccent();
        }

        SetAccent(0f);
    }

    private void AnimateTo(float scaleMultiplier, Vector2 offset, Color color, float accentAlpha)
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }

        animationRoutine = StartCoroutine(AnimateRoutine(scaleMultiplier, offset, color, accentAlpha));
    }

    private IEnumerator AnimateRoutine(float scaleMultiplier, Vector2 offset, Color color, float accentAlpha)
    {
        Vector3 startScale = rectTransform.localScale;
        Vector3 targetScale = baseScale * scaleMultiplier;
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = basePosition + offset;
        Color startColor = targetGraphic != null ? targetGraphic.color : Color.white;
        float startAccentAlpha = accentImage != null ? accentImage.color.a : 0f;
        float elapsed = 0f;

        while (elapsed < animationTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / animationTime);
            t = 1f - Mathf.Pow(1f - t, 3f);

            rectTransform.localScale = Vector3.LerpUnclamped(startScale, targetScale, t);
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(startPosition, targetPosition, t);

            if (targetGraphic != null && useColorTint)
            {
                targetGraphic.color = Color.LerpUnclamped(startColor, color, t);
            }

            SetAccent(Mathf.Lerp(startAccentAlpha, accentAlpha, t));
            yield return null;
        }

        rectTransform.localScale = targetScale;
        rectTransform.anchoredPosition = targetPosition;

        if (targetGraphic != null && useColorTint)
        {
            targetGraphic.color = color;
        }

        SetAccent(accentAlpha);
        animationRoutine = null;
    }

    private void CreateAccent()
    {
        GameObject accentObject = new GameObject("Hover Accent", typeof(RectTransform), typeof(Image));
        RectTransform accentRect = accentObject.GetComponent<RectTransform>();
        accentRect.SetParent(rectTransform, false);
        accentRect.anchorMin = new Vector2(0f, 0.18f);
        accentRect.anchorMax = new Vector2(0f, 0.82f);
        accentRect.pivot = new Vector2(0.5f, 0.5f);
        accentRect.anchoredPosition = new Vector2(-16f, 0f);
        accentRect.sizeDelta = new Vector2(5f, 0f);

        accentImage = accentObject.GetComponent<Image>();
        accentImage.raycastTarget = false;
        accentImage.color = accentColor;
    }

    private void SetAccent(float alpha)
    {
        if (accentImage == null)
        {
            return;
        }

        Color color = accentColor;
        color.a *= alpha;
        accentImage.color = color;
    }
}
