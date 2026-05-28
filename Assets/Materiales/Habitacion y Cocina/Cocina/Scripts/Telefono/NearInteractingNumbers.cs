using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEngine.Rendering.GPUSort;

public class NearInteractingNumbers : MonoBehaviour
{
    private XRGrabInteractable DialSelected => GetComponent<XRGrabInteractable>();
    [SerializeField] Material matSelectedNumber;
    [SerializeField] Material matDesactiveNumber;
    private Collider numberCol;
    public static bool numberSelected = false;
    private bool hoverNumber = false;

    [SerializeField] private Material hovermaterial;
    [SerializeField] private Material Nonhovermaterial;

    private void OnEnable()
    {
        DialSelected.selectEntered.AddListener(GrabbedBy);
        DialSelected.selectExited.AddListener(GrabbedEnd);
    }

    public void OnDisable()
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

        float largo_f = 0.08f;

        Vector3 eje = contactPoint.forward;
        Vector3 p1 = contactPoint.position;
        Vector3 p2 = contactPoint.position + eje * largo_f;

        Collider[] colliders = Physics.OverlapCapsule(p1, p2, 0.004f);
        // Collider[] colliders = Physics.OverlapSphere(contactPoint.position, 0.004f);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("Number"))
            {
                numberCol = col;
                numberCol.gameObject.tag = "NumberPhoneSelected";

                Renderer rendNumber = numberCol.gameObject.GetComponent<Renderer>();
                rendNumber.material = matSelectedNumber;
                numberSelected = true;
                //PhoneRotate.grabNumber = true;
                break;
            }
        }
    }
}
