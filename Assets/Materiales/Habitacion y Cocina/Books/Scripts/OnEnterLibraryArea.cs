using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class OnEnterLibraryArea : MonoBehaviour
{
    public bool interactingMinigame = false;
    public bool minigameCompleted = false;
    public TeleportPlayer anchorEndMinigame;
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor anchor;

    //public GameObject xrOrigin;
    //public GameObject continuousTurn;
    //public GameObject teleport;
    //public GameObject continuousMove;
    //public GameObject snapTurn;
    //public GameObject leftController;
    //public Transform xrOrigin;
    //private float xrOriginAngle;
    //private float xrOriginAngleInicial;

    private void Start()
    {
        //xrOriginAngleInicial = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Colision");

            if(interactingMinigame == false && minigameCompleted == false)
            {
                // EnterInteraction();
                interactingMinigame = true;
            }
        }
    }
    public void Update()
    {
        //xrOriginAngle = xrOrigin.rotation.eulerAngles.y;
        //ExitInteraction();
    }
    public void EnterInteraction()
    {
        //Bloquear movimiento mientras interactua
        //continuousTurn.SetActive(false);
        //snapTurn.SetActive(true);
        //continuousMove.SetActive(false);
        //leftController.SetActive(false);
        //interactingMinigame = true;
        //xrOriginAngleInicial = xrOrigin.rotation.eulerAngles.y;
    }
    public void ExitInteraction()
    {
        //continuousTurn.SetActive(true);
        //snapTurn.SetActive(false);
        //continuousMove.SetActive(true);
        //leftController.SetActive(true);
        interactingMinigame = false;
        Debug.Log("Salio de restricciones");
        anchorEndMinigame.Teleport(anchor);
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactingMinigame = false;
            Debug.Log("Salio completamente");
        }

    }

}
