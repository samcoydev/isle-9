using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerSpawning : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject[] spawnPoints;

    private void OnValidate()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("CustomerSpawnPoint");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnCustomerAtSpawnPoint(spawnPoints[Random.Range(0, spawnPoints.Length)]);
        }
    }

    private void SpawnCustomerAtSpawnPoint(GameObject spawnPoint)
    {
        Instantiate(customerPrefab, spawnPoint.transform.position, Quaternion.identity);
    }

    public GameObject GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
    }
}
