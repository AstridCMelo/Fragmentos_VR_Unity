using UnityEngine;

public class CerrarUI : MonoBehaviour
{
    public GameObject panel;

    public void Cerrar()
    {
        panel.SetActive(false);
    }
}