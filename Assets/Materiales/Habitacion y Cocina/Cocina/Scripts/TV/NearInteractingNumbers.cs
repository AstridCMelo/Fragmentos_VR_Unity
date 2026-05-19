using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NearInteractingNumbers : MonoBehaviour
{
    private XRGrabInteractable DialSelected => GetComponent<XRGrabInteractable>();
    [SerializeField] Material matSelectedNumber;
    [SerializeField] Material matDesactiveNumber;
    private Collider numberCol;
    public static bool numberSelected;

    private void OnEnable()
    {
        DialSelected.selectEntered.AddListener(GrabbedBy);
        DialSelected.selectExited.AddListener(GrabbedEnd);
    }

    private void OnDisable()
    {
        DialSelected.selectEntered.RemoveListener(GrabbedBy);
        DialSelected.selectExited.RemoveListener(GrabbedEnd);
    }

    private void GrabbedEnd(SelectExitEventArgs arg0)
    {
        if (numberSelected)
        {
            if (numberCol.gameObject.CompareTag("NumberPhoneSelected"))
            {
                Renderer rendNumber = numberCol.gameObject.GetComponent<Renderer>();
                rendNumber.material = matDesactiveNumber;
                numberCol.gameObject.tag = "Number";
                numberSelected = false;
                PhoneRotate.grabNumber = false;
            }
        }
    }

    private void GrabbedBy(SelectEnterEventArgs args)
    {
        var interactor = args.interactorObject;
        Transform contactPoint = interactor.GetAttachTransform(null);

        Collider[] colliders = Physics.OverlapSphere(contactPoint.position, 0.004f);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("Number"))
            {
                numberCol = col;
                numberCol.gameObject.tag = "NumberPhoneSelected";

                Renderer rendNumber = numberCol.gameObject.GetComponent<Renderer>();
                rendNumber.material = matSelectedNumber;
                numberSelected = true;
                PhoneRotate.grabNumber = true;
                break;
            }
        }
    }
}
