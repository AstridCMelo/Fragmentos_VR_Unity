using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject Button;
    public UnityEvent OnPress;
    public UnityEvent OnRelease;

    AudioSource sound;
    public bool isPressed;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
    }

    public void OnSelected()
    {
        Button.transform.localPosition = new Vector3(Button.transform.localPosition.x, -0.06f, Button.transform.localPosition.z);
        OnPress.Invoke();
        sound.Play();
    }

    public void OnRealease()
    {
        Button.transform.localPosition = new Vector3(Button.transform.localPosition.x, 0, Button.transform.localPosition.z);
        OnRelease.Invoke();
        isPressed = false;

    }

}
