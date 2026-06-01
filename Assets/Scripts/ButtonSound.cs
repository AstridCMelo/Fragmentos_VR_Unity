using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler
{
    public AudioSource audioSource;
    public AudioClip clickClip;
    public AudioClip hoverClip;
    public bool autoWireSceneButtons = true;
    public bool autoAddButtonFeedback = true;
    public bool playHoverOnSelect = true;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (autoWireSceneButtons)
        {
            WireSceneButtons();
        }
    }

    public void PlayClick()
    {
        PlayClip(clickClip);
    }

    public void PlayHoverSound()
    {
        PlayClip(hoverClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClick();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (playHoverOnSelect)
        {
            PlayHoverSound();
        }
    }

    private void WireSceneButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (Button button in buttons)
        {
            if (autoAddButtonFeedback && button.GetComponent<ButtonFeedback>() == null)
            {
                button.gameObject.AddComponent<ButtonFeedback>();
            }

            EventTrigger trigger = button.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = button.gameObject.AddComponent<EventTrigger>();
            }

            AddTrigger(trigger, EventTriggerType.PointerEnter, _ => PlayHoverSound());
            AddTrigger(trigger, EventTriggerType.PointerClick, _ => PlayClick());
            AddTrigger(trigger, EventTriggerType.Select, _ =>
            {
                if (playHoverOnSelect)
                {
                    PlayHoverSound();
                }
            });
        }
    }

    private static void AddTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventType
        };

        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }
}
