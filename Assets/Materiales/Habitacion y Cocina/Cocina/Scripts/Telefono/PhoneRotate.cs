using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PhoneRotate : MonoBehaviour
{
    [SerializeField] private Transform linkedDial;
    [SerializeField] private float returnVelocity = 150f;
    private AudioPhoneManager audioManager;

    //Se coloca true si se agarra un n?mero
    public static bool grabNumber = false;
    //public static bool returnPosition = false;

    //private bool grabNumber = false;
    private bool returnPosition = false;

    private float angleControl;

    //public static IXRSelectInteractor interactor;

    private IXRSelectInteractor interactor;

    private float lastAngleControl;
    private float currentAngleDial;
    private float initialAngleDial;

    private float accumulatedDegrees = 0;

    Vector3 initialRotation;
    Vector3 startPosition;

    //public static void SetGrab(bool state)
    //{
    //    grabNumber = state;
    //}

    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

    private void OnEnable()
    {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabbedEnd);
    }

    private void OnDisable()
    {
        grabInteractor.selectEntered.RemoveListener(GrabbedBy);
        grabInteractor.selectExited.RemoveListener(GrabbedEnd);
    }
    private void GrabbedEnd(SelectExitEventArgs arg0)
    {
        grabNumber = false;
        returnPosition = true;
    }

    private void GrabbedBy(SelectEnterEventArgs args)
    {
        audioManager = GetComponent<AudioPhoneManager>();
        interactor = args.interactorObject;

        //interactor = GetComponent<XRGrabInteractable>().firstInteractorSelecting;
        // PhoneRotate.returnPosition = false;
        grabNumber = true;

        Vector3 dirRotation = GetInteractorPosition() - linkedDial.position;
        float anglerad = Mathf.Atan2(dirRotation.x, dirRotation.y);
        lastAngleControl = anglerad * Mathf.Rad2Deg;

        accumulatedDegrees = currentAngleDial;
    }


    public void Start()
    {
        initialRotation = linkedDial.transform.eulerAngles;
        initialAngleDial = linkedDial.localEulerAngles.z;
        currentAngleDial = linkedDial.localEulerAngles.z;
    }

    void Update()
    {
        if (returnPosition)
        {
            if (audioManager.reproduced == false)
            {
                audioManager.PlaySound = true;
                audioManager.reproduced = true;
            }
            ReturnAntiClockwise();

        }
        else if (grabNumber == true && interactor != null && NearInteractingNumbers.numberSelected == true)
        {
            var currentPosition = GetInteractorPosition();
            RotateClockwise(currentPosition);
        }
    }

    public Vector3 GetInteractorPosition() => interactor.transform.position;

    //private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;
    private void RotateClockwise(Vector3 currentPosition)
    {
        Vector3 dirRotation = currentPosition - linkedDial.position;

        var anglerad = Mathf.Atan2(dirRotation.x, dirRotation.y); //Angulo del dial a partir de la distancia del controller al centro del dial
        angleControl = anglerad * Mathf.Rad2Deg;

        float delta = Mathf.DeltaAngle(lastAngleControl, angleControl);


        accumulatedDegrees += delta;

        //currentAngleDial += delta; // Del dial

        currentAngleDial = Mathf.Clamp(accumulatedDegrees, -343, 0);

        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, currentAngleDial);

        // Debug.Log("Rotating");

        lastAngleControl = angleControl; //Del controlador
    }
    private void ReturnAntiClockwise()
    {
        grabNumber = false;
        currentAngleDial = Mathf.MoveTowards(currentAngleDial, initialAngleDial, returnVelocity * Time.deltaTime);

        linkedDial.localEulerAngles = new Vector3(0f,
                                                  0f,
                                                  currentAngleDial);

        if (currentAngleDial >= initialAngleDial)
        {
            currentAngleDial = initialAngleDial;
            returnPosition = false;
            grabNumber = false;
            audioManager.reproduced = true;
        }
    }
}

