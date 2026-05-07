using System.Collections.Generic;
using UnityEngine;

public class MinigameDialControl : MonoBehaviour, IChangeChannel
{
    private AudioTVManager audioManager;
    [SerializeField] float RightChannel = 270; //Grados que debe avanzar
    public UIController pantalla;

    [SerializeField] private TVChannel[] canales;
    private int canalIndex;
    private float LastDialValue;

    void Start()
    {
        audioManager = GetComponent<AudioTVManager>();

        canalIndex = 0;
    }

    public void ChannelChanged(float dialvalue)
    {
        audioManager.PlaySound = true;

        Debug.Log("Channel " + dialvalue);

        //Verificar si es un canal que muestra imagen 
       // UpdateChannel(canalIndex);

        //if (dialvalue < 0)
        //{
        //    ForwardChannel();
        //}
        //else if (dialvalue > 0)
        //{
        //    BackChannel();
        //}

        if (dialvalue == RightChannel)
        {
            UnlockFragment();
        }

        LastDialValue = dialvalue;

    }

    public void UnlockFragment()
    {
        Debug.Log("Interfaz Pista Limpia");
        pantalla.ShowImage();
    }

    public void UpdateChannel(int index)
    {
        pantalla.panel = canales[index].panel;
    }

    public void ForwardChannel()
    {
        canalIndex++;
    }

    public void BackChannel()
    {
        canalIndex--;
    }
}
