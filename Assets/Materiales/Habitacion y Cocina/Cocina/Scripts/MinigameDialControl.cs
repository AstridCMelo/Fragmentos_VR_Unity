using UnityEngine;

public class MinigameDialControl : MonoBehaviour, IChangeChannel
{ 
    [SerializeField] float RightChannel = 270; //Grados que debe avanzar
    public UIController panel;
    public void ChannelChanged(float dialvalue)
    {
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
