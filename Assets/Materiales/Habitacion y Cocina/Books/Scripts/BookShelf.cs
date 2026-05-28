using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static UnityEngine.Rendering.GPUSort;

public class BookShelf : MonoBehaviour
{
    public UIController panel;
    public OnEnterLibraryArea interaction;
    private AudioSource audioBook;

    [SerializeField] private AudioClip dropBook;
    [SerializeField] private AudioClip grabBook;

    [SerializeField] private string word;
    private List<char> initialLetters = new List<char>();
    private string currentword;

    private List<XRSocketInteractor> socketInteractorList = new List<XRSocketInteractor>();

    private List<IXRSelectInteractable> books = new List<IXRSelectInteractable>();
    private List<bool> positionsBooks = new List<bool>();
    private bool changeState = true;
    //private int pastposition = 0;
    private bool dropbook = false;
    public void Start()
    {

        audioBook = GetComponent<AudioSource>();
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

            var book = socket.interactablesSelected[0];
            book.selectExited.AddListener(DropBook);
            books.Add(book);

        }
    }
    public void Update()
    {
        if(interaction.interactingMinigame == true && interaction.minigameCompleted == false)
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
                    Debug.Log(dropbook);
                    if (dropbook == true)
                    {

                    }
                    else
                    {
                        //pastposition = i;
                        MoveBooks(i);
                        changeState = true;
                        break;
                    }
                }
                else if (positionOcuped == true)
                {
                    trueOcupedPosition++;
                }
                else if (positionOcuped == false && i == (positionsBooks.Count - 1))
                {
                    changeState = true;
                    break;
                }

                i++;
            }

            if (changeState == true)
            {
                if (trueOcupedPosition == positionsBooks.Count)
                {
                    GetPastPosition();
                    GetCurrentWord();
                    VerifyOrganizedWord();
                    changeState = false;
                }
            }
        }
    }

    public void MoveBooks(int i)
    {
        for (int j = i; j < (positionsBooks.Count - 1); j++)
        {
            if (j == (positionsBooks.Count - 2) && positionsBooks[positionsBooks.Count - 1] == false)
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

    public void GetPastPosition()
    {
        //for(int j = positionsBooks.Count - 1;  j < pastposition; j--)
        //{
        //    var socketBookInteractable = socketInteractorList[j - 1].interactablesSelected[0];
        //    books[j] = socketBookInteractable;
        //}

        //books[pastposition] =  ;

        int i = 0;
        foreach (XRSocketInteractor socket in socketInteractorList)
        {
            books[i] = socket.interactablesSelected[0];
            i++;
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

    public AudioSource audioSource;
    public AudioClip sonidoFragmento;
    public void VerifyOrganizedWord()
    {
        if (word == currentword)
        {
            Debug.Log("Interfaz fragmento");
            audioSource.PlayOneShot(sonidoFragmento);
            panel.ShowImage();
            interaction.minigameCompleted = true;
            interaction.ExitInteraction();
        }
    }

    public void DropBook(SelectExitEventArgs args)
    {
        if (!(args.interactorObject is XRSocketInteractor))
        {
            var book = args.interactableObject;
            StartCoroutine(CheckdropSocket(book));
        }
    }

    IEnumerator CheckdropSocket(IXRSelectInteractable book)
    {
        yield return null;

        if(book.interactorsSelecting.Count == 0)
        {
            Backposition();
            Debug.Log("Lo solto en el aire");
        }
        else
        {
            Debug.Log("Lo solto en el socket");
        }
    }

    public void Backposition()
    {
        dropbook = true;

        Debug.Log("Solto Libro");
        

        foreach (XRSocketInteractor socket in socketInteractorList)
        {
            if (socket.interactablesSelected.Count > 0)
            {
                var currentbook = socket.interactablesSelected[0];
                socket.interactionManager.SelectExit(socket, currentbook);
            }
        }

        int i = 0;

        foreach (XRSocketInteractor socket in socketInteractorList)
        {
            socket.interactionManager.SelectEnter(socket, books[i]);
            i++;
        }
        dropbook = false;
    }
}
