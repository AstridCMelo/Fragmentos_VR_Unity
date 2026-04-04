using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private string titulo;
    private char initialLetter => titulo[0];
    public char GetInitialLetter ()
    { 
        return initialLetter; 
    }

}
