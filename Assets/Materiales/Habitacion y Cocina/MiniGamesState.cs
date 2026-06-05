using UnityEngine;

public class MiniGamesState
{
    public static bool minigame1Completed = false;
    public static bool minigame2Completed = false;
    public static bool minigame3Completed = false;

    public static void Reset()
    {
        minigame1Completed = false;
        minigame2Completed = false;
        minigame3Completed = false;
    }
}
