using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject panel;
    public void ShowImage()
    {
        panel.SetActive(true);
    }

    public void ChangeText(TMP_Text tmpText, int number)
    {
        if(number == -1)
        {
            tmpText.text = "_";
        }
        else
        {
            tmpText.text = number.ToString();
            Debug.Log("Aqui registra");
        }

    }

    public void UpdateUi()
    {

    }
}
