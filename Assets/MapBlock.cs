using UnityEngine;
using System.Collections.Generic;

public class MapBlock : MonoBehaviour
{
    [SerializeField] List<Transform> spawnpoints = new();
    public int VariantIndex;

    private void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        spawnpoints.Clear();
        transform.GetChild(VariantIndex).gameObject.SetActive(true);
        spawnpoints = new List<Transform>(transform.GetChild(VariantIndex).transform.GetChild(1).GetComponentsInChildren<Transform>());
    }

    public void SpawnEnemies()
    {
        foreach (Transform spawnpoint in spawnpoints)
        {
            int randomIndex = Random.Range(0, GameManager.Instance.EnemyTypes.Length);
            int randomHealth = Random.Range(30, 151*GameManager.Instance.Stage);
            GameObject enemy = Instantiate(GameManager.Instance.EnemyTypes[randomIndex], spawnpoint.position, Quaternion.identity);
            enemy.GetComponent<StatsSystem>().MaxHP = randomHealth;
            GameManager.Instance.EnemiesAlive.Add(enemy);
        }
    }
}
