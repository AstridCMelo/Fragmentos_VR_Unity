using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PhoneRotate : MonoBehaviour
{
    [SerializeField] private Transform linkedDial;
    [SerializeField] private float returnVelocity = 2f;

    //Se coloca true si se agarra un número
    public static bool grabNumber = false;
    public static bool returnPosition = false;

    private float angle;

    public static IXRSelectInteractor interactor;

    private float lastAngle;
    private float currentAngle;
    private float initialAngle;

    Vector3 initialRotation;
    Vector3 startPosition;

    //public static void SetGrab(bool state)
    //{
    //    grabNumber = state;
    //}

    public void Start()
    {
        initialRotation = linkedDial.transform.eulerAngles;
        initialAngle = linkedDial.localEulerAngles.z;
        currentAngle = linkedDial.localEulerAngles.z;
    }

    void Update()
    {
        if(returnPosition)
        {
            ReturnAntiClockwise();
            currentAngle = initialAngle;
            returnPosition = false;
        }
        else if (grabNumber)
        {
        if (grabNumber)
        {
            initialRotation = linkedDial.transform.eulerAngles;
            var currentPosition = GetInteractorPosition();
            RotateClockwise(currentPosition);
        }

        }
    }

    public Vector3 GetInteractorPosition() => interactor.transform.position;

    //private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;
    private void RotateClockwise(Vector3 currentPosition)
    {
        Vector3 dirRotation = currentPosition - linkedDial.position;

        var anglerad = Mathf.Atan2(dirRotation.x, dirRotation.y); //Angulo del dial a partir de la distancia del controller al centro del dial
        angle = anglerad * Mathf.Rad2Deg;

        float delta = Mathf.DeltaAngle(lastAngle, angle);

        currentAngle -= delta;

        currentAngle = Mathf.Clamp(currentAngle, 0, 180);

        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, currentAngle);

       // Debug.Log("Rotating");

        lastAngle = currentAngle;
    }
    private void ReturnAntiClockwise()
    {
        currentAngle = Mathf.MoveTowards(currentAngle, initialAngle, returnVelocity * Time.deltaTime);

        linkedDial.localEulerAngles = new Vector3(0f,
                                                  0f,
                                                  currentAngle);
    }

}