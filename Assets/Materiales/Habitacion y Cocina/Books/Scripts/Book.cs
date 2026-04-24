using UnityEngine;
using TMPro;

public class Book : MonoBehaviour
{
    private string titulo;
    private TextMeshProUGUI textTitle;
    private char initialLetter;
    void Start()
    {
        textTitle = GetComponentInChildren<TextMeshProUGUI>();
        titulo = textTitle.text;
        initialLetter = titulo[0];
    }
    public char GetInitialLetter ()
    { 
        return initialLetter; 
    }
}
