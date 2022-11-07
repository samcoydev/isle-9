using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Product DAO", menuName = "Product DAO")]
public class ProductDAO : ScriptableObject
{

    public int id;
    public GameObject prefab;
    
    public GameObject GetModelOnly()
    {
        return prefab.transform.Find("Model").gameObject;
    }

}
