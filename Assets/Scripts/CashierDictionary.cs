using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierDictionary : MonoBehaviour {
    [SerializeField] private List<GameObject> cashierCounters;

    private void OnValidate() {
        cashierCounters = new List<GameObject>(GameObject.FindGameObjectsWithTag("CashierCounter"));
    }

    public List<GameObject> GetMannedCashierCounters() {
        return cashierCounters.FindAll(c => c.GetComponent<CashierCounter>().isManned);
    }

    public GameObject GetRandomMannedCashierCounter() {
        var list = GetMannedCashierCounters();
        return list.Count > 0 ? list[Random.Range(0, list.Count)] : cashierCounters[0];
    }

    public void AddNewCashierCounter(GameObject shelf) {
        cashierCounters.Add(shelf);
    }
}
