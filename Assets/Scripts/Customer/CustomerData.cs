using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CustomerData {

    public Vector3 position;
    public int money;
    public int amountOfTargetsToVisit;
    public bool isInspecting;
    public bool isAngry;
    public List<GameObject> targets;
    public GameObject exitPositionObject;
    public GameObject cashierCounter;
    public GameObject currentTarget;

}
