using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayTrigger : MonoBehaviour
{
    //public string targetScene;        // scene to load
    //public string targetSpawnPoint;   // spawn ID in target scene
    [SerializeField] private string linkedScene;
    [SerializeField] private string linkedGateID;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Colliding with gateway!!");

            // Upon collision with this tile space, communicate to the game manager the designated scene and door ID to send the player to.
            // This system allows the accurate gateway selection in another scene, in the event that that scene has multiple gateways.
            // Every scene must have a general purpose script upon scene load to take in the ID and set the player's coordinates accordingly.

            // Store where we should spawn in the next scene
            //GameManager.Instance.linkedGateDestination = linkedGateID;
            //SceneManager.LoadScene(linkedScene);
        }
    }
}