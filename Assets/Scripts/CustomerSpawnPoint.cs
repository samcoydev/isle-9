using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnPoint : MonoBehaviour, IInspectable {
    public void Inspect(MovementSM _sm) {
        GameObject.Destroy(_sm.gameObject);
    }

    public bool IsInspecting() {
        return false;
    }
}
