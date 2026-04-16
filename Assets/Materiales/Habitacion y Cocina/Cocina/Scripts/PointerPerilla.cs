using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PointerPerilla : MonoBehaviour
{
    [SerializeField] private float angleTolerance = 5;
    [SerializeField] private Transform linkedDial;
    [SerializeField] private float snapRotation = 30;
    private float startAngle;
   
    private IXRSelectInteractor interactor;

    private bool requiresStartAngle = true;

    private bool shouldGetControlRotation = false;

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
        shouldGetControlRotation = false;
        requiresStartAngle = true;
    }

    private void GrabbedBy(SelectEnterEventArgs args)
    {
        interactor = GetComponent<XRGrabInteractable>().firstInteractorSelecting;
        shouldGetControlRotation = true;
        startAngle = 0f;
    }

    void Update()
    {
        if(shouldGetControlRotation)
        {
            var currentAngle = GetInteractorRotation();
            //Debug.Log("CurrentAngle" + currentAngle);
            GetRotationDistance(currentAngle);
        }
    }

    public float GetInteractorRotation() => interactor.transform.eulerAngles.z;

    private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;
    private void RotateDialClockwise()
    {
        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x,
                                                  linkedDial.localEulerAngles.y,
                                                  linkedDial.localEulerAngles.z - snapRotation);

        if (TryGetComponent<IChangeChannel>(out IChangeChannel dial))
            dial.ChannelChanged(linkedDial.localEulerAngles.z);
    }

    private void RotateDialAntiClockwise()
    {
        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x,
                                                  linkedDial.localEulerAngles.y,
                                                  linkedDial.localEulerAngles.z + snapRotation);

        if (TryGetComponent<IChangeChannel>(out IChangeChannel dial))
            dial.ChannelChanged(linkedDial.localEulerAngles.z);
    }

    private void GetRotationDistance(float currentAngle)
    {
        var angleDifference = Mathf.Abs(startAngle - currentAngle);

        if (!requiresStartAngle)
        {
            if (angleDifference > angleTolerance)
            {
                if (angleDifference > 270f) //Para cambios pequeños, con diferencia grande (pasar de 359° a 1°)
                {
                    float angleCheck;

                    if (startAngle < currentAngle)
                    {
                        angleCheck = CheckAngle(currentAngle, startAngle);

                        if (angleCheck < angleTolerance)
                            return;
                        else
                        {
                            RotateDialClockwise();
                            startAngle = currentAngle;
                        }
                    }
                    else if (startAngle > currentAngle)
                    {
                        angleCheck = CheckAngle(currentAngle, startAngle);

                        if (angleCheck < angleTolerance)
                            return;
                        else
                        {
                            RotateDialAntiClockwise();
                            startAngle = currentAngle;
                        }
                    }
                }
                else
                {
                    if (startAngle < currentAngle)
                    {
                        RotateDialAntiClockwise();
                        startAngle = currentAngle;
                    }
                    else if (startAngle > currentAngle)
                    {
                        RotateDialClockwise();
                        startAngle = currentAngle;
                    }
                }
            }
        }
        else
        {
            requiresStartAngle = false;
            startAngle = currentAngle;
        }
        //Código para rotación de Dial de https://www.youtube.com/watch?v=vIrgCMNsE3s&t=1169s
    }
}