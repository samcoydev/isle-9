using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerSpawning : MonoBehaviour, IDataPersistence {

    [SerializeField] private StorageDictionary storageDictionary;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private List<GameObject> customerPool = new();

    [SerializeField] private bool canSpawnAgain = true;

    public int maxCustomers;
    public float spawnCooldown;

    private void OnValidate()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("CustomerSpawnPoint");
    }

    private void Start() {
        canSpawnAgain = true;
    }

    public void LoadData(GameDataCollection data) {
        foreach(CustomerData customerData in data.customers) {
            var customerObj = Instantiate(customerPrefab, customerData.position, Quaternion.identity);
            var customer = customerObj.GetComponent<MovementSM>();
            customer.amountOfTargetsToVisit = customerData.amountOfTargetsToVisit;
            customer.cashierCounter = customerData.cashierCounter;
            customer.currentTarget = customerData.currentTarget;
            customer.exitPositionObject = customerData.exitPositionObject;
            customer.isAngry = customerData.isAngry;
            customer.isInspecting = customerData.isInspecting;
            customer.money = customerData.money;
            customer.targets = customerData.targets;
        }
    }

    public void SaveData(ref GameDataCollection data) {
        List<CustomerData> customerDataList = new();
        foreach(GameObject customerObj in customerPool) {
            CustomerData customerData = new();
            var customer = customerObj.GetComponent<MovementSM>();
            customerData.amountOfTargetsToVisit = customer.amountOfTargetsToVisit;
            customerData.cashierCounter = customer.cashierCounter;
            customerData.currentTarget = customer.currentTarget;
            customerData.exitPositionObject = customer.exitPositionObject;
            customerData.isAngry = customer.isAngry;
            customerData.isInspecting = customer.isInspecting;
            customerData.money = customer.money;
            customerData.position = customer.transform.position;
            customerData.targets = customer.targets;
            customerDataList.Add(customerData);
        }
        data.customers = customerDataList;
    }

    private void Update() {
        if (CanSpawnCustomers() && canSpawnAgain)
            StartCoroutine(StartCooldown());
    }

    public void RemoveFromCustomerPool(GameObject customer) {
        customerPool.Remove(customer);
    }

    private IEnumerator StartCooldown() {
        canSpawnAgain = false;
        SpawnCustomerAtRandomSpawnPoint();
        yield return new WaitForSeconds(spawnCooldown);
        canSpawnAgain = true;
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
        if (customerPool.Count >= maxCustomers) { return false; }

        var validStorageAvailable = storageDictionary.GetStorageShelvesThatHaveProducts().Count > 0;
        if (!validStorageAvailable) { return false; }

        return true;
    }
}
