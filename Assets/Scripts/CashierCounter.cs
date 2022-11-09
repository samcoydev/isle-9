using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierCounter : MonoBehaviour, IInspectable {

    private PlayerData playerData;
    public bool isManned;

    private void Start() {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    public void Inspect(MovementSM _sm) {
        _sm.isInspecting = true;
        StartCoroutine(InspectCoroutine(_sm));
    }

    private IEnumerator InspectCoroutine(MovementSM _sm) {
        yield return new WaitForSeconds(Random.Range(_sm.minSecondsToInspect, _sm.maxSecondsToInspect));

        playerData.AddMoney(_sm.money);
        _sm.isInspecting = false;
    }
}
