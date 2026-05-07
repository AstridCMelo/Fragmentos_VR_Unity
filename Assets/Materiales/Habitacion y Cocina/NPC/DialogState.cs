using UnityEngine;

public class DialogState : MonoBehaviour
{
    public enum DialogueBlockState
    {
        start,
        loop,
        end
    };

    public int dialogBlockIndex = 0;

    public bool startBlockPlayed = false;
    public bool minigameCompleted = false;

    public int loopBlockIndex = 0;
    public DialogueBlockState StateGame = DialogueBlockState.start;
    public NPCDialog dialogueData;

    public static string[] dialogueLinesNPCActive;

}
