using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour, IInteractableNPC
{
    public NPCDialog dialogueData;
    public GameObject dialogPanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImageNPC;

    public GameObject interactionPanel;

    private int dialogueIndex;
    private bool PlayerTalking;

    private bool isTyping, isDialogueActive = false;

    private string[] activedialogueLinesNPC;
    private bool[] activeNPCorPlayerDialogLine;
    private AudioClip[] activeNPCorPlayerDialogClip;

    public int dialogBlockIndex = 0;

    public bool startBlockPlayed = false;
    public bool minigameCompleted = false;

    private int loopBlockIndex = 0;
    private int maxloopIndex = 1;
    private int minloopIndex = 1;

    private bool validInteraction = false;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Canvas InteractuaVer
            interactionPanel.SetActive(true);
            validInteraction = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Canvas InteractuaVer
            interactionPanel.SetActive(true);
            //validInteraction = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Canvas InteractuaDesaparece
            interactionPanel.SetActive(false);
            validInteraction = false;
        }
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        //Si no está activo ya un dialogo
        if ((dialogueData == null && !isDialogueActive) || validInteraction == false)
            return;


        interactionPanel.SetActive(false);

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            startDialogue();
        }
        
    }

    void startDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        ActiveBlock();
        UpdateNamePortrait();

        dialogPanel.SetActive(true);

        //Typing
        StartCoroutine(TypeLine());

    }

    void UpdateNamePortrait()
    {
        if (activeNPCorPlayerDialogLine[dialogueIndex])
        {
            nameText.SetText(dialogueData.npcName);
            portraitImageNPC.sprite = dialogueData.npcPortrait;
            // SoundEffectManager.PlayNpc(activeNPCorPlayerDialogClip[dialogueIndex], Random.Range(0.9f, 1.1f));
        }
        else
        {
            nameText.SetText(dialogueData.playerName);
            portraitImageNPC.sprite = dialogueData.playerPortrait;
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach( char letter in activedialogueLinesNPC[dialogueIndex])
        {
            dialogueText.text += letter;
            //SoundEffectManager.PlayNpc(dialogueData.voiceeSound, Random.Range(0.9f, 1.1f));
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            UpdateNamePortrait();
            dialogueText.SetText(activedialogueLinesNPC[dialogueIndex]);
            isTyping = false;
        }
        else if(++dialogueIndex < activedialogueLinesNPC.Length)
        {
            UpdateNamePortrait();
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        SoundEffectManager.StopNpc();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialogPanel.SetActive(false);
        //Canvas InteractuaDesaparece
        interactionPanel.SetActive(true);
    }


    public void ActiveBlock()
    {
        bool endStart = false;

        foreach (DialogueBlock block in dialogueData.dialogueblockNPC)
        {
            if (block.dialogueBlockState != NPCDialog.DialogueBlockState.loop)
            {
                endStart = true;
                loopBlockIndex = 1;
            }
        }

        if (endStart)
        {
            int lastIndex = dialogueData.dialogueblockNPC.Length - 1;
            minloopIndex = 1;
            maxloopIndex = lastIndex - 1;

            if (!startBlockPlayed)
            {
                activedialogueLinesNPC = dialogueData.dialogueblockNPC[0].dialogueLinesNPC;
                activeNPCorPlayerDialogLine = dialogueData.dialogueblockNPC[0].NPCorPlayerDialogLine;
                activeNPCorPlayerDialogClip = dialogueData.dialogueblockNPC[0].dialogueVoicesNPC;
                startBlockPlayed = true;
            }
            else if (minigameCompleted)
            {
                activedialogueLinesNPC = dialogueData.dialogueblockNPC[lastIndex].dialogueLinesNPC;
                activeNPCorPlayerDialogLine = dialogueData.dialogueblockNPC[lastIndex].NPCorPlayerDialogLine;
                activeNPCorPlayerDialogClip = dialogueData.dialogueblockNPC[lastIndex].dialogueVoicesNPC;
            }
            else
            {
                if (loopBlockIndex > maxloopIndex)
                {
                    loopBlockIndex = minloopIndex;
                }

                activedialogueLinesNPC = dialogueData.dialogueblockNPC[loopBlockIndex].dialogueLinesNPC;
                activeNPCorPlayerDialogLine = dialogueData.dialogueblockNPC[loopBlockIndex].NPCorPlayerDialogLine;
                activeNPCorPlayerDialogClip = dialogueData.dialogueblockNPC[loopBlockIndex].dialogueVoicesNPC;
                startBlockPlayed = true;

                loopBlockIndex++;
            }
        }
        else
        {
            minloopIndex = 0;
            maxloopIndex = dialogueData.dialogueblockNPC.Length-1;

            if (loopBlockIndex > maxloopIndex)
            {
                loopBlockIndex = minloopIndex;
            }

            activedialogueLinesNPC = dialogueData.dialogueblockNPC[loopBlockIndex].dialogueLinesNPC;
            activeNPCorPlayerDialogLine = dialogueData.dialogueblockNPC[loopBlockIndex].NPCorPlayerDialogLine;
            activeNPCorPlayerDialogClip = dialogueData.dialogueblockNPC[loopBlockIndex].dialogueVoicesNPC;

            loopBlockIndex++;
        }
    }
}

//Evitar que se pueda mover hasta que acabe de interactuar con el NPC
//Hacer que alternen los prites del jugador y con quien habla


 