using UnityEngine;
using TMPro;

public class Book : MonoBehaviour
{
    private string titulo;
    private TextMeshProUGUI textTitle;
    private char initialLetter => titulo[0];
    void Start()
    {
        textTitle = GetComponentInChildren<TextMeshProUGUI>();
        titulo = textTitle.text;
    }
    public char GetInitialLetter ()
    { 
        return initialLetter; 
    }

}
