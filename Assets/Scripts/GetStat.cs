using TMPro;
using UnityEngine;

public class GetStat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private PlayerState playerStats;
    [SerializeField] private string classInstanceID;
    [SerializeField] private TextMeshProUGUI displayPlayerName;
    [SerializeField] private TextMeshProUGUI displayHP;
    [SerializeField] private TextMeshProUGUI displayMP;
    [SerializeField] private TextMeshProUGUI displayLVL;
    [SerializeField] private TextMeshProUGUI displayAtk;
    [SerializeField] private TextMeshProUGUI displayAgi;
    [SerializeField] private TextMeshProUGUI displayXp;
    [SerializeField] private TextMeshProUGUI displayGold;

    public string CLASSINSTANCEID => classInstanceID;

    void Start()
    {
        if (playerStats == null)
            playerStats = PlayerState.instance;

        GrabTheStats();
    }

    void OnEnable()
    {
        if (playerStats == null)
            playerStats = PlayerState.instance;

        GrabTheStats();
    }

    public void GrabTheStats()
    {
        // call this whenever i open the pause menu or need to update in battle
        if (displayPlayerName != null)
            displayPlayerName.text = playerStats.ENTNAME;
        if (displayHP != null)
            displayHP.text = playerStats.HP.ToString();
        if (displayMP != null)
            displayMP.text = playerStats.MP.ToString();
        if (displayLVL != null)
            displayLVL.text = playerStats.LVL.ToString();
        if (displayAtk != null)
            displayAtk.text = playerStats.ATK.ToString();
        if (displayAgi != null)
            displayAgi.text = playerStats.AGI.ToString();
        if (displayXp != null)
            displayXp.text = playerStats.XP.ToString();
        if (displayGold != null)
            displayGold.text = playerStats.GOLD.ToString();
    }
}
