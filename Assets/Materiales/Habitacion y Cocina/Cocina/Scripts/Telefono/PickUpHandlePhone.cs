using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static UnityEngine.Rendering.GPUSort;
public class PickUpHandlePhone : MonoBehaviour
{
    [SerializeField] AudioSource telephoneAudioSource;
    [SerializeField] AudioClip hangUp;
    [SerializeField] float hangUpStart;
    [SerializeField] AudioClip PickUp;
    [SerializeField] float PickUpStart;
    [SerializeField] AudioClip phoneTune;
    [SerializeField] float phoneTuneStart;
    private bool isGrabbed = false;

    [SerializeField] Transform fixedPositionGrab;
    [SerializeField] Transform fixPosition;
    [SerializeField] XRSocketInteractor socketInteractor;

    [SerializeField] float strenghtReturn = 20f;

    public UIController numberPanelController;
    public MiniGamePhone MiniGamePhone;
    private IXRSelectInteractable HandleSelected => GetComponent<XRGrabInteractable>(); 

    private void OnEnable()
    {
        HandleSelected.selectEntered.AddListener(GrabbedBy);
        HandleSelected.selectExited.AddListener(GrabbedEnd);
    }

    public void OnDisable()
    {
        HandleSelected.selectEntered.RemoveListener(GrabbedBy);
        HandleSelected.selectExited.RemoveListener(GrabbedEnd);
    }

    private void GrabbedEnd(SelectExitEventArgs arg0)
    {
        //Sonido hangup
        //isGrabbed=false;
        //Sound(hangUp, false, hangUpStart);

        IXRSelectInteractor interactor = arg0.interactorObject;

        Debug.Log("Here handle");
        Debug.Log(interactor);

        if ((interactor is not XRSocketInteractor))
        {
            ReleasePhone();

        }
    }

    private void Update()
    {
        if(isGrabbed == true)
        {
            StayGrabbed();
        }
    }

    private void Sound(AudioClip soundPhone, bool loop, float start, float pitch)
    {
        telephoneAudioSource.clip = soundPhone;
        telephoneAudioSource.time = start;
        telephoneAudioSource.pitch = pitch;
        telephoneAudioSource.loop = loop;
        telephoneAudioSource.Play();
    }

    private void GrabbedBy(SelectEnterEventArgs args)
    {
        IXRSelectInteractor interactor = args.interactorObject;
        Debug.Log(isGrabbed);

        if((interactor is not XRSocketInteractor) && isGrabbed == false)
        {
            FixedPositionGrab();
            //Sonido pickup
            telephoneAudioSource.volume = 1f;
            float pitch = Random.Range(0.9f, 1.1f);
            Sound(PickUp, false, PickUpStart,pitch);
            isGrabbed = true;
            Debug.Log("isGrabbed");
            numberPanelController.ShowImage();
        }
        else if(interactor is XRSocketInteractor)
        {
            if(isGrabbed == true)
            {
                ReleasePhone();
                telephoneAudioSource.volume = 1f;
                float pitch = Random.Range(0.9f, 1.1f);
                Sound(hangUp, false, hangUpStart,pitch);
                //Revisar si ya se coloco la fecha
                //Borrar números
                MiniGamePhone.EraseNumbers();
                isGrabbed = false;
            }
        }
    }

    private void StayGrabbed()
    {
        //Sonido tono
        if (!telephoneAudioSource.isPlaying)
        {
            float pitch = 1.0f;
            telephoneAudioSource.volume = 0.5f;
            Sound(phoneTune, true, phoneTuneStart, pitch);
        }
    }

    public void FixedPositionGrab()
    {
        HandleSelected.transform.localRotation = fixedPositionGrab.localRotation;
        HandleSelected.transform.localPosition = fixedPositionGrab.localPosition;
    }

    public void ReleasePhone()
    {
        HandleSelected.transform.localRotation = fixPosition.localRotation;
        HandleSelected.transform.localPosition = fixPosition.localPosition;
        socketInteractor.interactionManager.SelectEnter(socketInteractor, HandleSelected);
    }

}
