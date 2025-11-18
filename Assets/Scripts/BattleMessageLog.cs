using System.Collections;
using TMPro;
using UnityEngine;

public class BattleMessageLog : MonoBehaviour
{
    public static BattleMessageLog Instance;

    [SerializeField] private TMP_Text messageText;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator ShowMessage(string msg, float delay = 0.5f)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(delay);
    }
}
