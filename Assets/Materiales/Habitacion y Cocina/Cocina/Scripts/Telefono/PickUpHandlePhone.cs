using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
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

    public UIController numberPanelController;
    public MiniGamePhone MiniGamePhone;
    [SerializeField] Collider wireArea;
    private Rigidbody rb;
    private XRGrabInteractable HandleSelected => GetComponent<XRGrabInteractable>(); 

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
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        VerifyPosition();
        if(isGrabbed == true)
        {
            StayGrabbed();
        }
    }

    private void Sound(AudioClip soundPhone, bool loop, float start)
    {
        telephoneAudioSource.clip = soundPhone;
        telephoneAudioSource.time = start;
        telephoneAudioSource.pitch = Random.Range(0.8f, 1.2f);
        telephoneAudioSource.loop = loop;
        telephoneAudioSource.Play();
    }

    private void GrabbedBy(SelectEnterEventArgs args)
    {
        IXRSelectInteractor interactor = args.interactorObject;
        Debug.Log(isGrabbed);

        if((interactor is not XRSocketInteractor) && isGrabbed == false)
        {
            //Sonido pickup
            Sound(PickUp, false, PickUpStart);
            isGrabbed = true;
            Debug.Log("isGrabbed");
            numberPanelController.ShowImage();
        }
        else if(interactor is XRSocketInteractor)
        {
            if(isGrabbed == true)
            {
                isGrabbed = false;
                Sound(hangUp, false, hangUpStart);
                //Revisar si ya se coloco la fecha
                //Borrar números
                MiniGamePhone.EraseNumbers();
            }
        }
    }

    private void StayGrabbed()
    {
        //Sonido tono
        if (!telephoneAudioSource.isPlaying)
        {
            Sound(phoneTune, true, phoneTuneStart);
        }
    }

    private void VerifyPosition()
    {
        float maxRadioWire = 2f;

        if(transform.position.x > maxRadioWire)
        {
            //transform.position.x = maxRadioWire;
            //rb.AddForce()


        }else if(transform.position.y > maxRadioWire)
        {

        }else if (transform.position.z > maxRadioWire)
        {

        }
    }

}
