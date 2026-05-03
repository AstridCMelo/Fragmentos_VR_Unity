using System.Collections.Generic;
using UnityEngine;

public class MinigameDialControl : MonoBehaviour, IChangeChannel
{
    private AudioTVManager audioManager;
    [SerializeField] float RightChannel = 270; //Grados que debe avanzar
    public UIController panel;
   // [SerializeField] 

    void Start()
    {
        audioManager = GetComponent<AudioTVManager>();
    }

    public void ChannelChanged(float dialvalue)
    {
        audioManager.PlaySound = true;

        Debug.Log("Channel " + dialvalue);

        if(dialvalue == RightChannel)
        {
            UnlockFragment();
        }
    }

    public void UnlockFragment()
    {
        Debug.Log("Interfaz Pista Limpia");
        panel.ShowImage();
    }
}
