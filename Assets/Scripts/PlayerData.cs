using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

public class PlayerData : MonoBehaviour, IDataPersistence {

    [SerializeField] private ProductPrefabDictionary productPrefabDictionary;

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI productInfoText;
    [SerializeField] private GameObject hand;
    public GameObject gameManager;

    private SerializableDictionary<int, int> heldProducts = new(); // Product ID | Count
    [SerializeField] private List<int> validProductIDs = new();
    private int currentHeldProductID;
    public int money;

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        productPrefabDictionary = gameManager.GetComponent<ProductPrefabDictionary>();
        currentHeldProductID = -1;
    }

    public void LoadData(GameDataCollection data) {
        currentHeldProductID = data.currentHeldProductID;
        money = data.money;
        LoadHeldProducts(data);
    }

    public void SaveData(ref GameDataCollection data) {
        data.currentHeldProductID = currentHeldProductID;
        data.money = money;
        SaveHeldProducts(data);
    }

    private void LoadHeldProducts(GameDataCollection data) {
        InitializeHeldProducts();
        validProductIDs = data.actualHeldProducts.Keys.ToList();
        for (int i = 0; i < data.actualHeldProducts.Count; i++) {
            var productID = validProductIDs[i];
            heldProducts[productID] = data.actualHeldProducts[productID];
        }
        CreateProductModelInHand();
    }

    private void SaveHeldProducts(GameDataCollection data) {
        var newHeldProducts = new SerializableDictionary<int, int>();
        for (int i = 0; i < validProductIDs.Count; i++) {
            var productId = validProductIDs[i];
            newHeldProducts.Add(productId, heldProducts[productId]);
        }
        data.actualHeldProducts = newHeldProducts;
    }

    private void InitializeHeldProducts() {
        foreach (ProductDAO productDAO in productPrefabDictionary.GetProductDAOList()) {
            heldProducts.Add(productDAO.id, 0);
        }
    }

    public int getProductCount(int productId) {
        if (heldProducts.TryGetValue(productId, out var value))
            return value;

        return 0;
    }

    public void AddOrRemoveValidProductID(int productId) {
        if (heldProducts[productId] > 0 && !validProductIDs.Contains(productId))
            validProductIDs.Add(productId);
        if (heldProducts[productId] <= 0) {
            if (currentHeldProductID == productId) {
                currentHeldProductID = validProductIDs.Count != 1 ? GetPreviousID() : -1;
            }
            validProductIDs.Remove(productId);
        }
        CreateProductModelInHand();
        validProductIDs.Sort();
    }

    public void AddProduct(int productId, int productAmount) {
        heldProducts[productId] += productAmount;
        AddOrRemoveValidProductID(productId);
    }

    public void SubtractProduct(int productId, int productAmount) {
        heldProducts[productId] -= productAmount;
        AddOrRemoveValidProductID(productId);
    }

    public void AddMoney(int count) {
        money += count;
        goldText.text = $"Gold - {money}";
    }

    public void SubtractMoney(int count) {
        money -= count;
        goldText.text = $"Gold - {money}";
    }

    public int GetCurrentHeldProductCount() {
        if (currentHeldProductID == -1) return 0;
        return heldProducts[currentHeldProductID];
    }

    public int GetCurrentHeldProductID() {
        return currentHeldProductID;
    }

    public int GetCountOfHeldProductWithID(int id) {
        if (heldProducts.TryGetValue(id, out int count))
            return count;

        return -1;
    }

    public void SwitchHeldProduct(Vector2 mouseScrollDelta) {
        if (mouseScrollDelta.y < 0)
            currentHeldProductID = GetNextID();
        if (mouseScrollDelta.y > 0)
            currentHeldProductID = GetPreviousID();
        CreateProductModelInHand();
    }

    private void CreateProductModelInHand() {
        RemoveProductModelInHand();
        if (currentHeldProductID != -1 && productPrefabDictionary.GetProductDAOByID(currentHeldProductID)) {
            var product = Instantiate(productPrefabDictionary.GetProductDAOByID(currentHeldProductID).GetModelOnly(), hand.transform);
            SetLayerAllChildren(product.transform, LayerMask.NameToLayer("Arm"));
        }

        ChangeProductInfoText();
    }

    private void RemoveProductModelInHand() {
        if (hand.transform.childCount > 0)
            Destroy(hand.transform.GetChild(0).gameObject);
    }

    private void ChangeProductInfoText() {
        if (currentHeldProductID == -1) productInfoText.text = "Hand";
        else productInfoText.text = $"{productPrefabDictionary.GetProductDAOByID(currentHeldProductID).name} - {heldProducts[currentHeldProductID]}";
    }

    private int GetNextID() {
        if (validProductIDs.Count == 0) return -1;

        var currentIndex = validProductIDs.FindIndex(p => p == currentHeldProductID);

        if (currentIndex == validProductIDs.Count - 1)
            return validProductIDs[0];

        return validProductIDs[currentIndex + 1];
    }

    private int GetPreviousID() {
        if (validProductIDs.Count == 0) return -1;

        var currentIndex = validProductIDs.FindIndex(p => p == currentHeldProductID);

        if (currentIndex == 0)
            return validProductIDs[^1];

        return validProductIDs[currentIndex - 1];
    }

    private void SetLayerAllChildren(Transform root, int layer) {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children) {
            child.gameObject.layer = layer;
        }
    }
}
