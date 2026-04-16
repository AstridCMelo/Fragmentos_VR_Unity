using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjetoMemoria : MonoBehaviour
{
    private XRGrabInteractable grab;
    private bool yaActivado = false;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (yaActivado) return;

        yaActivado = true;

        Debug.Log("Fragmento obtenido");

        GameManager.instancia.AgregarFragmento();

        gameObject.SetActive(false);
    }
}