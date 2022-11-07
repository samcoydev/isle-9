using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageDictionary : MonoBehaviour
{
    [SerializeField] private List<GameObject> storageShelves;

    private void OnValidate()
    {
        storageShelves = new List<GameObject>(GameObject.FindGameObjectsWithTag("Storage"));
    }

    public List<GameObject> GetStorageShelvesList()
    {
        return storageShelves;
    }

    public List<GameObject> GetStorageShelvesThatHaveProducts()
    {
        return storageShelves.FindAll((s) => s.GetComponent<Storage>().HasProduct());
    }

    public int GetStorageShelvesCount()
    {
        return storageShelves.Count;
    }

    public void AddNewStorageShelf(GameObject shelf)
    {
        storageShelves.Add(shelf);
    }
}
