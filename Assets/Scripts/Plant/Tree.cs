using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Plant {

    protected override void SetPrompt() {
        _prompt = "(E) Harvest Tree";
    }

    protected override Vector3 GetRandomSpawnPointWithinBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
