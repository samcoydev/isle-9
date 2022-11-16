using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Plant {

    protected override void SetPrompt() {
        _prompt = "(E) Harvest Bush";
    }

    protected override Vector3 GetRandomSpawnPointWithinBounds(Bounds bounds) {
        var x = bounds.extents.x / 2;
        var z = bounds.extents.z / 2;
        return new Vector3(
            GetRandomBool() ? x : -x,
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(-z, z)
        );
    }

    private bool GetRandomBool() {
        return (Random.value > 0.5f);
    }
}
