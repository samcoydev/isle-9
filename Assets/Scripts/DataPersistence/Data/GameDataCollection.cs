using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameDataCollection {

    // Player Movement
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Quaternion cameraRotation;

    // Player Data
    public SerializableDictionary<int, int> actualHeldProducts; // ID - Count
    public int currentHeldProductID;
    public int money;

    // Storage
    public SerializableDictionary<string, StorageData> storageShelves;

    // Plants
    public SerializableDictionary<string, PlantData> plants;

    // Customer AI
    public List<CustomerData> customers; 

    public GameDataCollection() {
        playerPosition = new Vector3(0, 3, 0);
        playerRotation = new Quaternion();
        cameraRotation = new Quaternion();

        actualHeldProducts = new();
        currentHeldProductID = -1;
        money = 0;

        storageShelves = new();

        plants = new();

        customers = new();
    }

}
