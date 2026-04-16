//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit.Interactables;
//using UnityEngine.XR.Interaction.Toolkit.Interactors;
//public class Number : MonoBehaviour
//{
//    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

//    private void OnEnable()
//    {
//        grabInteractor.selectEntered.AddListener(GrabbedBy);
//        grabInteractor.selectExited.AddListener(GrabbedEnd);
//    }

//    private void OnDisable()
//    {
//        grabInteractor.selectEntered.RemoveListener(GrabbedBy);
//        grabInteractor.selectExited.RemoveListener(GrabbedEnd);

//    }
//    private void GrabbedEnd(SelectExitEventArgs arg0)
//    {
//        PhoneRotate.grabNumber = false;
//        PhoneRotate.returnPosition = true;
//    }

//    private void GrabbedBy(SelectEnterEventArgs args)
//    {
//        IXRSelectInteractor interactor = GetComponent<XRGrabInteractable>().firstInteractorSelecting;
//        // PhoneRotate.returnPosition = false;
//        PhoneRotate.interactor = interactor;
//        PhoneRotate.grabNumber = true;
//    }



//}
