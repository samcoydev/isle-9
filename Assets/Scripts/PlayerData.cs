using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private bool debugMode = false;
    public GameObject gameManager;

    private Dictionary<int, int> heldProducts = new Dictionary<int, int>(); // Product ID | Count
    private int currentHeldProductID;
    [SerializeField] public int money { get; private set; }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        InitializeHeldProducts();
        currentHeldProductID = 0;
    }

    private void InitializeHeldProducts()
    {
        foreach (ProductDAO productDAO in gameManager.GetComponent<ProductPrefabDictionary>().GetProductDAOList())
        {
            heldProducts.Add(productDAO.id, 0);
        }
    }

    public int getProductCount(int productId)
    {
        if (heldProducts.TryGetValue(productId, out var value))
            return value;
        
        return 0;
    }

    public void AddProduct(int productId, int productAmount)
    {
        heldProducts[productId] += productAmount;
        if (debugMode && heldProducts.TryGetValue(productId, out var value))
            Debug.Log("Holding: " + "ID - " + productId + " " + value);
    }

    public void SubtractProduct(int productId, int productAmount)
    {
        heldProducts[productId] -= productAmount;
        if (debugMode && heldProducts.TryGetValue(productId, out var value))
            Debug.Log("Holding: " + "ID - " + productId + " " + value);
    }

    public void AddMoney(int count) {
        money += count;
        goldText.text = $"Gold - {money}";
    }

    public void SubtractMoney(int count) {
        money -= count;
        goldText.text = $"Gold - {money}";
    }

    public int GetCurrentHeldProductCount()
    {
        return heldProducts[currentHeldProductID];
    }

    public int GetCurrentHeldProductID() {
        return currentHeldProductID;
    }

    public int GetCountOfHeldProductWithID(int id)
    {
        if (heldProducts.TryGetValue(id, out int count))
            return count;

        return -1;
    }

    public void DecreaseCount(int id, int count)
    {
        heldProducts[id] -= count;
        Debug.Log($"You now have {heldProducts[id]} {gameManager.GetComponent<ProductPrefabDictionary>().GetProductDAOByID(id)}s!");
    }

    public void SwitchHeldProduct(Vector2 mouseScrollDelta)
    {
        if (mouseScrollDelta.y < 0)
            currentHeldProductID = GetNextID();
        if (mouseScrollDelta.y > 0)
            currentHeldProductID = GetPreviousID();
    }

    private int GetNextID()
    {
        if (currentHeldProductID == heldProducts.Count - 1)
            return 0;

        return currentHeldProductID + 1;
    }

    private int GetPreviousID()
    {
        if (currentHeldProductID == 0)
            return heldProducts.Count - 1;

        return currentHeldProductID - 1;
    }
}
