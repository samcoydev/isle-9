using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnPoint : MonoBehaviour, IInspectable {

    private CustomerSpawning customerSpawning;

    private void Start() {
        customerSpawning = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CustomerSpawning>();
    }

    public void Inspect(MovementSM _sm) {
        customerSpawning.RemoveFromCustomerPool(_sm.gameObject);
        GameObject.Destroy(_sm.gameObject);
    }

    public bool IsInspecting() {
        return false;
    }
}
