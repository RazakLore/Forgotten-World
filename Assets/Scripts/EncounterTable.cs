using UnityEngine;

public class EncounterTable : MonoBehaviour
{
    [Header("Enemies that spawn in this region")]
    [SerializeField] private GameObject[] enemyPrefabs;

    public GameObject GetRandomEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }
}
