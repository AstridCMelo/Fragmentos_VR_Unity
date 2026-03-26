using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BookShelf: MonoBehaviour
{
    public UIController panel;

    [SerializeField] private string word;
    private List<char> initialLetters = new List<char>();
    private string currentword;
    private List<XRSocketInteractor> socketInteractorList = new List<XRSocketInteractor>();
    private List<bool> positionsBooks = new List<bool> ();
    private bool changeState = true;
    public void Start()
    {
        //Sockets deben estar de izquierda a derecha como se leeen
        GetComponentsInChildren<XRSocketInteractor>(socketInteractorList);
        Debug.Log(socketInteractorList.Count);

        foreach (XRSocketInteractor socket in socketInteractorList)
        {
            positionsBooks.Add(socket.hasSelection);
        }
        Debug.Log(positionsBooks.Count);


        foreach (XRSocketInteractor socket in socketInteractorList)
        {
            var socketBookInteractable = socket.interactablesSelected[0] as Component;
            var socketBook = socketBookInteractable.gameObject.GetComponent<Book>();

            initialLetters.Add(socketBook.GetInitialLetter());
        }
    }
    public void Update()
    {
        int i = 0;
        int trueOcupedPosition = 0;
        foreach (XRSocketInteractor socket in socketInteractorList)
        {
            positionsBooks[i] = socket.hasSelection;
            i++;
        }

        i = 0;
        foreach (bool positionOcuped in positionsBooks)
        {
            if (positionOcuped == false && i != (positionsBooks.Count - 1))
            {
                MoveBooks(i);
                changeState = true;
                break;
            }
            else if (positionOcuped == true)
            {
                trueOcupedPosition++;
            }
                i++;
        }

        if(changeState == true)
        {
            if (trueOcupedPosition == positionsBooks.Count)
            {
                GetCurrentWord();
                VerifyOrganizedWord();
                changeState = false;
            }
        }
    }

    public void MoveBooks(int i)
    {
        for (int j = i; j < (positionsBooks.Count - 1); j++)
        {
            if(j == (positionsBooks.Count - 2) && positionsBooks[positionsBooks.Count-1] == false)
            {

            }
            else
            {
                var currentBook = socketInteractorList[j + 1].interactablesSelected[0];
                Debug.Log(currentBook);

                socketInteractorList[j + 1].interactionManager.SelectExit(socketInteractorList[j + 1], currentBook);
                socketInteractorList[j].interactionManager.SelectEnter(socketInteractorList[j], currentBook);
            }
        }
    }

    public void GetCurrentWord()
    {
        int i = 0;

        foreach (XRSocketInteractor socket in socketInteractorList)
        {
            var socketBookInteractable = socket.interactablesSelected[0] as Component;
            var socketBook = socketBookInteractable.gameObject.GetComponent<Book>();

            initialLetters[i] = socketBook.GetInitialLetter();
            i++;
        }

        Debug.Log(initialLetters);

        currentword = string.Join("", initialLetters);

        Debug.Log(currentword);
    }

    public void VerifyOrganizedWord()
    {
        if (word == currentword)
        {
            Debug.Log("Interfaz fragmento");
            panel.ShowImage();

        }
    }

}
