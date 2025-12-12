using UnityEngine;

public class EncounterTable : MonoBehaviour
{
    [Header("Enemies that spawn in this region")]
    [SerializeField] private GameObject[] enemyPrefabs;

    public GameObject GetRandomEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return null;

        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }
}
