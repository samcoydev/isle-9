using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerSpawning : MonoBehaviour {

    [SerializeField] private StorageDictionary storageDictionary;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject[] spawnPoints;

    [SerializeField] private List<GameObject> customerPool = new();

    public bool manualTemporarySpawnFlag;
    public int maxCustomers;
    public int spawnCooldown;

    private void OnValidate()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("CustomerSpawnPoint");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K))
            SpawnCustomerAtRandomSpawnPoint();
    }
    private void SpawnCustomerAtRandomSpawnPoint()
    {
        var newCustomer = Instantiate(customerPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
        customerPool.Add(newCustomer);
    }

    public GameObject GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
    }

    private bool CanSpawnCustomers() {
        return manualTemporarySpawnFlag
            && customerPool.Count <= maxCustomers;
    }
}
