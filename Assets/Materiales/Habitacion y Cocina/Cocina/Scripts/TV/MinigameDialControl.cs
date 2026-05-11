using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.NativeTypes;
using static MinigameDialControl;

public class MinigameDialControl : MonoBehaviour, IChangeChannel
{
    private AudioTVManager audioManager;
    [SerializeField] float RightChannel = 270; //Grados que debe avanzar
    public UIController pantalla;

    [SerializeField] private ChannelPanel[] canales;
    private int canalIndex;
    private float LastDialValue;

    [SerializeField] private Material matNoise;

    void Start()
    {
        audioManager = GetComponent<AudioTVManager>();
        canalIndex = 0;
    }

    public void ChannelChanged(float dialvalue)
    {
        pantalla.HideImage();

        audioManager.PlaySound = true;

        //Debug.Log("Channel " + dialvalue);

        //Verificar si es un canal que muestra imagen 

        bool angleWithChannel = false;

        foreach (ChannelPanel canal in canales)
        {
            if (Mathf.Abs(dialvalue - canal.idCanal) < 0.3f)
            {
                UpdateChannel(canal);
                angleWithChannel = true;
                break;
            }
        }

        if (angleWithChannel == false)
        {
            Debug.Log("Random");
            UpdaterandomNoise();
        }

        //if (dialvalue == RightChannel)
        //{
        //    UnlockFragment();
        //}
    }

    public void UnlockFragment()
    {
        Debug.Log("Interfaz Pista Limpia");
        pantalla.ShowImage();
    }

    public void UpdaterandomNoise()
    {
        Debug.Log("Random");
        matNoise.SetFloat("_Noise Scale", Random.Range(350f, 500f));
        matNoise.SetFloat("_Noise Intensity", Random.Range(0.05f, 0.1f));
        matNoise.SetFloat("_Scanning Lines", Random.Range(1, 3));
        matNoise.SetFloat("Scanning Lines Amount", Random.Range(0.0f, 1.0f));
        matNoise.SetFloat("Scanning Lines Speed", Random.Range(0.5f, 1.5f));

    }

    public void UpdateChannel(ChannelPanel canal)
    {
        Debug.Log("cambio imagen");
        pantalla.panel = canal.panel;
        pantalla.ShowImage();
    }


    [System.Serializable]
    public class ChannelPanel
    {
        public int idCanal;
        public GameObject panel;
    }

}
