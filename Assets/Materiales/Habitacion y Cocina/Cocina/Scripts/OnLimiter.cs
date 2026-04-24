using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class OnLimiter : MonoBehaviour
{
    private int selectedNumber = 0;
    [SerializeField] private GameObject dialPhone;

    Collider coli;

    void Start()
    {
        coli = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(NearInteractingNumbers.numberSelected)
        {
            //El numero seleccionado se calcula a partir de las colisiones con el trigger del limitador
            selectedNumber++;
            Debug.Log(selectedNumber + "numero paso por trigger");

            if (other.CompareTag("NumberPhoneSelected"))
            {
                //registerNumber = true;
                coli.isTrigger = false;
                OnReleased();

                XRGrabInteractable GrabNumber = other.GetComponentInParent<XRGrabInteractable>(); ;
                if (GrabNumber != null && GrabNumber.isSelected && GrabNumber.firstInteractorSelecting != null)
                {
                    StartCoroutine(ReleaseAfterDelay(GrabNumber, 1f));
                    //IXRSelectInteractor interactor = GrabNumber.firstInteractorSelecting;
                    //GrabNumber.interactionManager.SelectExit(interactor, GrabNumber);
                    //Debug.Log("SoltoEntro");
                }
            }
        }
        else
        {
            selectedNumber = 0;
        }
    }

    IEnumerator ReleaseAfterDelay(XRGrabInteractable GrabNumber, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (GrabNumber != null && GrabNumber.isSelected && GrabNumber.firstInteractorSelecting != null)
        {
            IXRSelectInteractor interactor = GrabNumber.firstInteractorSelecting;
            GrabNumber.interactionManager.SelectExit(interactor, GrabNumber);
            Debug.Log("Soltó después de segundo");
        }
    }

    public void OnReleased()
    {
        //Se actualiza la interfaz

        PhoneRotate.grabNumber = false;
        //countRegisterNumbers++;
        if(selectedNumber == 10)
        {
            selectedNumber = 0;
        }

        Debug.Log("Registrar Numero " + selectedNumber);
        if (TryGetComponent<IRegisterNumber>(out IRegisterNumber dial))
            dial.RegisterNumber(selectedNumber);

        if (dialPhone.TryGetComponent<IRegisterNumber>(out var phoneDial))
        {
            phoneDial.RegisterNumber(selectedNumber);
        }

        selectedNumber = 0;
        coli.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
