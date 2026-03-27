using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public int fragmentosActuales = 0;
    public int fragmentosTotales = 3;
    public TextMeshProUGUI textoUI;
    public GameObject uiFinal;


    void Awake()
    {
        instancia = this;
    }

    public void AgregarFragmento()
    {
        fragmentosActuales++;

        textoUI.text = "Fragmentos: " + fragmentosActuales + "/3";

        if (fragmentosActuales >= fragmentosTotales)
        {
            FinalJuego();
        }
    }

    void FinalJuego()
    {
        Debug.Log("RECUERDO COMPLETO");

        if (uiFinal != null)
            uiFinal.SetActive(true);
    }
}