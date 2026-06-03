using System;
using System.Collections;
using TMPro;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractableNPC
{
    public NPCDialog dialogueData;
    public GameObject dialogPanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImageNPC;

    public GameObject interactionPanel;
    [SerializeField] private SoundEffectManager npcSoundEffectManager;

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
    private int loopBlockIndexStartEnd = 1;
    private int maxloopIndex = 1;
    private int minloopIndex = 1;

    private bool validInteraction = false;

    private bool classified = false;
    private bool endStart = false;

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
            //interactionPanel.SetActive(true);
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

            EndDialogue();

        }
    }

    public void Start()
    {
        MiniGamesState.Reset();
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


        //interactionPanel.SetActive(false);

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
        //SoundEffectManager.StopNpc();

        if (activeNPCorPlayerDialogLine[dialogueIndex])
        {
            nameText.SetText(dialogueData.npcName);
            portraitImageNPC.sprite = dialogueData.npcPortrait;
            //SoundEffectManager.PlayNpc(activeNPCorPlayerDialogClip[dialogueIndex], 1.0f);
        }
        else
        {
            nameText.SetText(dialogueData.playerName);
            portraitImageNPC.sprite = dialogueData.playerPortrait;
        }

        npcSoundEffectManager.PlayNpc(activeNPCorPlayerDialogClip[dialogueIndex], 1.0f);
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach( char letter in activedialogueLinesNPC[dialogueIndex])
        {
            dialogueText.text += letter;
            // SoundEffectManager.PlayNpc(dialogueData.voiceeSound, Random.Range(0.9f, 1.1f));
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
            //UpdateNamePortrait();
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
            
            if (!startBlockPlayed)
            {
                startBlockPlayed = true;
            } 
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        npcSoundEffectManager.StopNpc();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialogPanel.SetActive(false);
    }


    public void ActiveBlock()
    {
        if(!classified)
        {
            foreach (DialogueBlock block in dialogueData.dialogueblockNPC)
            {
                if (block.dialogueBlockState != NPCDialog.DialogueBlockState.loop)
                {
                    endStart = true;
                    break;
                }
            }
        }

        if (endStart)
        {
            if (dialogueData.minigameAsociated == 1)
            {
                minigameCompleted = MiniGamesState.minigame1Completed;
            }
            else if (dialogueData.minigameAsociated == 2)
            {
                minigameCompleted = MiniGamesState.minigame2Completed;
            }
            else
            {
                minigameCompleted = MiniGamesState.minigame3Completed;
            }


            int lastIndex = dialogueData.dialogueblockNPC.Length - 1;
            minloopIndex = 1;
            maxloopIndex = lastIndex - 1;

            if (minigameCompleted)
            {
                activedialogueLinesNPC = dialogueData.dialogueblockNPC[lastIndex].dialogueLinesNPC;
                activeNPCorPlayerDialogLine = dialogueData.dialogueblockNPC[lastIndex].NPCorPlayerDialogLine;
                activeNPCorPlayerDialogClip = dialogueData.dialogueblockNPC[lastIndex].dialogueVoicesNPC;
            }
            else if (!startBlockPlayed)
            {
                activedialogueLinesNPC = dialogueData.dialogueblockNPC[0].dialogueLinesNPC;
                activeNPCorPlayerDialogLine = dialogueData.dialogueblockNPC[0].NPCorPlayerDialogLine;
                activeNPCorPlayerDialogClip = dialogueData.dialogueblockNPC[0].dialogueVoicesNPC;
            }
            else
            {
                if (loopBlockIndexStartEnd > maxloopIndex)
                {
                    loopBlockIndexStartEnd = minloopIndex;
                }

                activedialogueLinesNPC = dialogueData.dialogueblockNPC[loopBlockIndexStartEnd].dialogueLinesNPC;
                activeNPCorPlayerDialogLine = dialogueData.dialogueblockNPC[loopBlockIndexStartEnd].NPCorPlayerDialogLine;
                activeNPCorPlayerDialogClip = dialogueData.dialogueblockNPC[loopBlockIndexStartEnd].dialogueVoicesNPC;
                startBlockPlayed = true;

                loopBlockIndexStartEnd++;
            }
        }
        else
        {
            minloopIndex = 0;
            maxloopIndex = dialogueData.dialogueblockNPC.Length - 1;

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


 