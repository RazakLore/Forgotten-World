using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaConnector : MonoBehaviour
{
    [Header("Where does this door go?")]
    [SerializeField] string doorID;
    [SerializeField] string targetID;
    [SerializeField] string targetScene;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Send the targetID to the game manager

            SceneManager.LoadScene(targetScene);
        }
    }
}
