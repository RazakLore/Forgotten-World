using UnityEngine;

public static class UIStateController
{
    public static UIState CurrentState = UIState.Gameplay;

    public static bool IsGameplay => CurrentState == UIState.Gameplay;
    public static bool IsBattle => CurrentState == UIState.Battle;
    public static bool IsPaused => CurrentState == UIState.Paused;
    public static bool IsDialogue => CurrentState == UIState.Dialogue;

}