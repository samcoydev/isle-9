using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProductPrefabDictionary : MonoBehaviour
{
    [SerializeField] private List<ProductDAO> productDAOList;

    private void OnValidate()
    {
        productDAOList = new List<ProductDAO>(Resources.LoadAll<ProductDAO>("Prefabs/Products/DAOs"));
    }

    public List<ProductDAO> GetProductDAOList()
    {
        return productDAOList;
    }

    public ProductDAO GetProductDAOByID(int id)
    {
        return productDAOList.Find((p) => p.id == id);
    }

    public int GetProductDAOCount()
    {
        return productDAOList.Count;
    }
}
