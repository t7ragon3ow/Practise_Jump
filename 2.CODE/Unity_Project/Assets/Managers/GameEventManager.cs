using UnityEngine;
using System.Collections;

public static class GameEventManager
{

    public delegate void GameEvent();

    public static event GameEvent GameStart, GameOver, GamePause, GameResume;

    public static void TriggerGameStart()
    {
        if (GameStart != null)
        {
            GameStart();
        }
    }

    public static void TriggerGameOver()
    {
        if (GameOver != null)
        {
            GameOver();
        }
    }

    public static void TriggerGamePause()
    {
        if (GamePause != null)
        {
            GamePause();
        }
    }

    public static void TriggerGameResume()
    {
        if (GameResume != null)
        {
            GameResume();
        }
    }
}