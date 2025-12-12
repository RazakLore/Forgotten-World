using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Gameplay")]
    public float messageSpeed = 0.05f;

    [Header("Audio")]
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    [Header("Controls")]
    public KeyCode interactKey = KeyCode.Space;
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode pauseKey = KeyCode.Escape;

    private void Awake()
    {
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnMessageSpeedChanged(float val)
    {
        messageSpeed = val;
    }

    public void OnMusicVolumeChanged(float val)
    {
        musicVolume = val;
    }

    public void OnSFXVolumeChanged(float val)
    {
        sfxVolume = val;
    }

    public void SetKeybind(KeybindAction action, KeyCode newKey)
    {
        switch (action)
        {
            case KeybindAction.Interact: interactKey = newKey; break;
            case KeybindAction.MoveUp: moveUpKey = newKey; break;
            case KeybindAction.MoveDown: moveDownKey = newKey; break;
            case KeybindAction.MoveLeft: moveLeftKey = newKey; break;
            case KeybindAction.MoveRight: moveRightKey = newKey; break;
            case KeybindAction.Pause: pauseKey = newKey; break;
        }
    }
}

public enum KeybindAction
{ 
    Interact,
    MoveUp,
    MoveLeft,
    MoveDown,
    MoveRight,
    Pause
}