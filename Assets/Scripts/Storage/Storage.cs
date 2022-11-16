using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Storage : MonoBehaviour, IInteractable, IInspectable, IDataPersistence {

    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] private string _prompt;
    [SerializeField] private ProductDAO productDAO;
    [SerializeField] private GameObject displayPointParent;
    [SerializeField] private List<Transform> displayPoints;
    [SerializeField] private TextMeshProUGUI productInfoText;

    private GameObject gameManager;

    public string InteractionPrompt => _prompt;
    public int productCount;
    public bool isInspecting;

    private void OnValidate() {
        if (displayPointParent) {
            displayPoints = new List<Transform>(displayPointParent.GetComponentsInChildren<Transform>());
            displayPoints.RemoveAt(0);
        }
    }

    private void Start() {
        if (!productDAO) _prompt = "(E) Stock Shelves";
        productCount = 0;
    }

    public void LoadData(GameDataCollection data) {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        StorageData storageShelfData = data.storageShelves.ContainsKey(id) ? data.storageShelves[id] : GetInitializedStorageData();

        if (storageShelfData.productID != -1)
            productDAO = gameManager.GetComponent<ProductPrefabDictionary>().GetProductDAOByID(storageShelfData.productID);

        Debug.Log($"My product DAO is: {productDAO}");

        transform.position = storageShelfData.position;
        LoadProductCount(storageShelfData);
    }

    public void SaveData(ref GameDataCollection data) {
        StorageData storageData = new();

        storageData.productID = productDAO != null ? productDAO.id : -1;
        storageData.position = transform.position;
        storageData.productCount = productCount;

        if (!data.storageShelves.ContainsKey(id))
            data.storageShelves.Add(id, storageData);
        else
            data.storageShelves[id] = storageData;
    }

    private StorageData GetInitializedStorageData() {
        StorageData storageData = new();

        storageData.productID = -1;
        storageData.position = transform.position;
        storageData.productCount = 0;

        Debug.Log("Initalizing for some reason..");

        return storageData;
    }

    private void LoadProductCount(StorageData storageData) {
        productCount = storageData.productCount;
        if (productCount > 0 && productDAO)
            AddProductsToDisplay();
    }

    private void Update() {
        if (productCount <= 0 && productDAO)
            RemoveProductsFromDisplay();
    }

    public bool Interact(Interactor interactor) {
        PlayerData playerData = interactor.GetPlayerData();

        if (!productDAO) {
            var currentHeldProductCount = playerData.GetCurrentHeldProductCount();

            if (currentHeldProductCount <= 0) {
                Debug.Log("You need at least one product to store");
                return true;
            }

            AddNewProduct(playerData.GetCurrentHeldProductID(), currentHeldProductCount, playerData);
            return true;
        } else {
            var count = playerData.GetCountOfHeldProductWithID(productDAO.id);

            if (count <= 0) {
                Debug.Log("You need at least one product to store");
                return true;
            }

            IncreaseCount(count, playerData);
        }

        return true;
    }

    public ProductDAO GetProduct() {
        return productDAO;
    }

    public bool HasProduct() {
        return productDAO ? true : false;
    }

    private void UpdateGUI() {
        if (productDAO)
            productInfoText.text = $"{productDAO.name} x {productCount}";
        else
            productInfoText.text = "";
    }

    private void AddNewProduct(int id, int count, PlayerData playerData) {
        productDAO = gameManager.GetComponent<ProductPrefabDictionary>().GetProductDAOByID(id);
        _prompt = $"(E) Stock Shelves with {productDAO.prefab.name}";

        IncreaseCount(count, playerData);
        AddProductsToDisplay();
    }

    private void IncreaseCount(int count, PlayerData playerData) {
        productCount += count;

        UpdateGUI();

        playerData.SubtractProduct(productDAO.id, count);
    }

    private int DecreaseCountInRangeAndGetMoney() {
        if (productCount <= 0)
            return 0;

        var amountToTake = GetAmountToTake();
        productCount -= amountToTake;

        UpdateGUI();

        return amountToTake * productDAO.cost;
    }

    private int GetAmountToTake() {
        if (productCount <= 2)
            return Random.Range(1, 2);

        return (int)Random.Range(1, Mathf.Max(3, Mathf.Round(productCount / 10)));
    }

    private void AddProductsToDisplay() {
        foreach (Transform point in displayPoints) {
            var product = Instantiate(productDAO.GetModelOnly(), point);
            product.transform.localScale = new Vector3(.2f, .2f, .2f);
        }
        UpdateGUI();
    }

    private void RemoveProductsFromDisplay() {
        foreach (Transform point in displayPoints) {
            Destroy(point.GetChild(0).gameObject);
        }
        productDAO = null;
        UpdateGUI();
    }

    public void Inspect(MovementSM _sm) {
        _sm.isInspecting = true;
        StartCoroutine(InspectCoroutine(_sm));
    }

    private IEnumerator InspectCoroutine(MovementSM _sm) {
        yield return new WaitForSeconds(Random.Range(_sm.minSecondsToInspect, _sm.maxSecondsToInspect));

        var moneyFromSelling = DecreaseCountInRangeAndGetMoney();

        if (moneyFromSelling == 0)
            _sm.TryAnotherTarget();

        _sm.money += moneyFromSelling;
        _sm.isInspecting = false;
    }

    public bool IsInspecting() {
        return isInspecting;
    }

    public bool IsDisabled() {
        return false;
    }
}
