using Unity.VisualScripting;
using UnityEngine;

public class MiniGameBook
{
    int completedWords = 0;
    int numberWordsToComplete = 2; 

    public void CompletedWords(bool verifyResult)
    {
        if (verifyResult == true)
        {
            completedWords++;
            UnlockFragmento();
        }
    }

    public void UnlockFragmento()
    {
        if (completedWords == numberWordsToComplete)
        {
            //Cambiar interfaz
        }
    }
}
