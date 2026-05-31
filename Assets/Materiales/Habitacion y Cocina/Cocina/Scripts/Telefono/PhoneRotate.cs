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

    private float angleControl = 0f;

    //public static IXRSelectInteractor interactor;

    private IXRSelectInteractor interactor;

    private float lastAngleControl = 0f;
    private float currentAngleDial = 0f;
    private float initialAngleDial = 0f;
    private float lastAccumulatedDegrees = 0f;

    private float accumulatedDegrees = 0f;

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

        float anglerad = AngleControl();

        //ángulo del control - Se pasa a grados
        lastAngleControl = anglerad * Mathf.Rad2Deg;

        accumulatedDegrees = currentAngleDial;
    }


    public void Start()
    {
        initialRotation = linkedDial.transform.localEulerAngles;
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
            Debug.Log("Girar Dial");
            //var currentPositionControl = GetInteractorPosition();
            RotateClockwise();
        }
    }

    public Vector3 GetInteractorPosition() => interactor.transform.position;

    public float AngleControl()
    {
        //---------- Para hallar el ángulo del control respecto al centro a partir de la posicion de este ---------------//

        //Resta para hallar cateto opuesto y adyacente, control - centro
        Vector3 interactorPosition = GetInteractorPosition();

        float catetoOpuesto = interactorPosition.y - linkedDial.position.y;
        float catetoAdyacente = interactorPosition.x - linkedDial.position.x;


        //Vector3 dirRotation = GetInteractorPosition() - linkedDial.position;

        //ángulo respecto al ejex positivo en radianes con signo y cuadrante correcto
        float anglerad = Mathf.Atan2(catetoOpuesto, catetoAdyacente);

        Debug.DrawLine(linkedDial.position, interactorPosition, Color.red);

        return anglerad;

        //-------------------------//
    }

    //private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;
    private void RotateClockwise()
    {
        lastAccumulatedDegrees = accumulatedDegrees;
        float anglerad = AngleControl();
        
        //Vector3 dirRotation = currentPositionControl - linkedDial.localPosition;

        //var anglerad = Mathf.Atan2(dirRotation.x, dirRotation.y); //Angulo del dial a partir de la distancia del controller al centro del dial
        angleControl = anglerad * Mathf.Rad2Deg;

        //diferencia entre el angulo del control actual y el anterior
        float delta = -Mathf.DeltaAngle(lastAngleControl, angleControl);


        //Tolerancia
        if (Mathf.Abs(delta) < 0.5f)
            return;

        Debug.Log(lastAngleControl);
        Debug.Log(angleControl);

        //Grados acumulados es igual a la sumatorio de los grados que se movio el control en cada iteracion
        accumulatedDegrees += delta;

        if(accumulatedDegrees > lastAccumulatedDegrees)
        {
            accumulatedDegrees = lastAccumulatedDegrees;
        }

        //currentAngleDial += delta; // Del dial

        //Teniendo en cuenta que los números máximo van a -343 por como esta construido el modelo del telefono,
        //Para determinar la rotación del dial, los grados acumulados dados por el control se normalizan a la escala de 0 a -343 que son los limites de rotación del dial 

        currentAngleDial = Mathf.Clamp(accumulatedDegrees, -360, 0);

        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, currentAngleDial);



        // Debug.Log("Rotating");

        lastAngleControl = angleControl; //Del controlador
    }
    private void ReturnAntiClockwise()
    {
        grabNumber = false;

        //Va avanzando de a pocos en cada iteración segun la velocidad de retorno y el tiempo entre frames
        currentAngleDial = Mathf.MoveTowards(currentAngleDial, initialAngleDial, returnVelocity * Time.deltaTime);

        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x,
                                                  linkedDial.localEulerAngles.y,
                                                  currentAngleDial);

        if (currentAngleDial >= initialAngleDial)
        {
            currentAngleDial = initialAngleDial;
            returnPosition = false;
            grabNumber = false;
            audioManager.reproduced = true;
            accumulatedDegrees = 0f;
        }
    }
}

