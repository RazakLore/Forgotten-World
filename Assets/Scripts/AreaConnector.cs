using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AreaConnector : MonoBehaviour
{
    [Header("Where does this door go?")]
    [SerializeField] private string doorID;
    [SerializeField] private string targetID;
    [SerializeField] private string targetScene;

    public string TargetID { get { return targetID; } }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Send the targetID to the game manager
            GameManager.instance.targetDoorID = targetID;
            SceneManager.LoadScene(targetScene);
        }
    }
}