using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private void OnEnable()
    {
        // Make sure GameManager knows we exist
        //if (GameManager.Instance != null && GameManager.Instance.playerInstance == null)
        //{
        //    GameManager.Instance.playerInstance = gameObject;
        //    DontDestroyOnLoad(gameObject);
        //}

        //// Place player at correct spawn point if set
        //if (!string.IsNullOrEmpty(AreaConnectorManager.nextConnectorID))
        //{
        //    AreaConnector[] connectors = FindObjectsOfType<AreaConnector>();
        //    foreach (AreaConnector connector in connectors)
        //    {
        //        if (connector.connectorID == AreaConnectorManager.nextConnectorID)
        //        {
        //            transform.position = connector.transform.position;
        //            break;
        //        }
        //    }
        //}
    }
}
