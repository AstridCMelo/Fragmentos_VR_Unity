using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RadioKnobIndicator : MonoBehaviour
{
    [SerializeField] private float angleTolerance = 0.2f;
    [SerializeField] private Transform linkedDial;
    [SerializeField] private Transform indicatorRadio;

    [SerializeField] private Transform maxLimitTransform;
    [SerializeField] private Transform minLimitTransform;

    private float maxIndicator = 1.4f;
    private float minIndicator = -1f;

    [SerializeField] private float snapRotation = 0.5f;
    private float startAngle;

    private IXRSelectInteractor interactor;

    private bool requiresStartAngle = true;

    private bool shouldGetControlRotation = false;

    private float indicatorLimits = 0f;

    [SerializeField] private float stepIndicator = 0.05f;

    private bool movementsense = true;

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

        indicatorLimits = indicatorRadio.transform.localPosition.z;
    }

    private void Start()
    {
        minIndicator = minLimitTransform.localPosition.z;
        maxIndicator = maxLimitTransform.localPosition.z;
    }
    void Update()
    {
        if (shouldGetControlRotation)
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
        if (indicatorLimits != maxIndicator)
        {
            linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x,
                                                  linkedDial.localEulerAngles.y,
                                                  linkedDial.localEulerAngles.z - snapRotation);

            MoveIndicatorForward();
        }
    }

    private void MoveIndicatorForward()
    {
        indicatorLimits = indicatorRadio.transform.localPosition.z + stepIndicator;

        IndicatorLimits();

        indicatorRadio.transform.localPosition = new Vector3(indicatorRadio.transform.localPosition.x,
                                                indicatorRadio.transform.localPosition.y,
                                                indicatorLimits);

        movementsense = true;

        if (TryGetComponent<IChangeTrack>(out IChangeTrack changeTrack))
        {
            changeTrack.MovementSense(movementsense, indicatorLimits);
        }

    }

    private void IndicatorLimits()
    {
        //Debug.Log(indicatorLimits);
        if (indicatorLimits >= minIndicator && indicatorLimits <= maxIndicator)
        {
            Debug.Log("Moviendo Indicador");
        }
        else if(indicatorLimits < minIndicator)
        {
            indicatorLimits = minIndicator;
            
        }
        else if(indicatorLimits > maxIndicator)
        {
            indicatorLimits = maxIndicator;
        }
      
    }

    private void MoveIndicatorBackward()
    {
        indicatorLimits = indicatorRadio.transform.localPosition.z - stepIndicator;

        IndicatorLimits();

        indicatorRadio.transform.localPosition = new Vector3(indicatorRadio.transform.localPosition.x,
                                                indicatorRadio.transform.localPosition.y,
                                                indicatorLimits);

        movementsense = false;


        if (TryGetComponent<IChangeTrack>(out IChangeTrack changeTrack))
        {
            changeTrack.MovementSense(movementsense, indicatorLimits);
        }

    }

    private void RotateDialAntiClockwise()
    {
        if(indicatorLimits != minIndicator)
        {
            linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x,
                                          linkedDial.localEulerAngles.y,
                                          linkedDial.localEulerAngles.z + snapRotation);

            MoveIndicatorBackward();
        }
    }

    private void GetRotationDistance(float currentAngle)
    {
        snapRotation = Mathf.Abs(Mathf.DeltaAngle(startAngle, currentAngle));

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