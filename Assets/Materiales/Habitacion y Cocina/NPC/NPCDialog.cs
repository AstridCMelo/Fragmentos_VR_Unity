using UnityEngine;
using UnityEngine.UIElements;
using static NPCDialog;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPCDialogue")]

public class NPCDialog: ScriptableObject
{
    public enum DialogueBlockState
    {
        start,
        loop,
        end
    };

    public string npcName;
    public string playerName;

    public Sprite npcPortrait;
    public Sprite playerPortrait;

    public DialogueBlock[] dialogueblockNPC;

    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;

    public float typingSpeed = 0.05f;
    public AudioClip voiceeSound;
    public float voicePitch = 1f;
}

[System.Serializable]
public class DialogueBlock
{
    public string[] dialogueLinesNPC;
    public bool[] NPCorPlayerDialogLine;
    public DialogueBlockState dialogueBlockState;
}