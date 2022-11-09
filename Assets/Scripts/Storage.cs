using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour, IInteractable, IInspectable {

    [SerializeField] private string _prompt;
    [SerializeField] private ProductDAO productDAO;
    [SerializeField] private GameObject displayPointParent;
    [SerializeField] private List<Transform> displayPoints;
    public CustomerSpawning cs;

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

    private void Update() {
        if (productCount <= 0 && productDAO)
            RemoveProductsFromDisplay();
    }

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (!productDAO) _prompt = "(E) Stock Shelves";
        productCount = 0;
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

    private void AddNewProduct(int id, int count, PlayerData playerData) {
        Debug.Log("d == eeznuts: " + id);
        productDAO = gameManager.GetComponent<ProductPrefabDictionary>().GetProductDAOByID(id);
        _prompt = $"(E) Stock Shelves with {productDAO.prefab.name}";

        IncreaseCount(count, playerData);
        AddProductsToDisplay();
        cs.manualTemporarySpawnFlag = true;
    }

    private void IncreaseCount(int count, PlayerData playerData) {
        productCount += count;

        playerData.DecreaseCount(productDAO.id, count);
    }

    private int DecreaseCountInRangeAndGetMoney() {
        if (productCount <= 0)
            return 0;

        var amountToTake = Random.Range(1, Mathf.Min(productCount, 3));
        productCount -= amountToTake;

        return amountToTake * productDAO.cost;
    }

    private void AddProductsToDisplay() {
        foreach (Transform point in displayPoints) {
            var product = Instantiate(productDAO.GetModelOnly(), point);
            product.transform.localScale = new Vector3(.2f, .2f, .2f);
        }
    }

    private void RemoveProductsFromDisplay() {
        foreach (Transform point in displayPoints) {
            Destroy(point.GetChild(0).gameObject);
        }
        productDAO = null;
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
