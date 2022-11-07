using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private ProductDAO productDAO;
    [SerializeField] private Bounds displayBox;
    [SerializeField] private GameObject displayPointParent;
    [SerializeField] private Transform[] displayPoints;

    private List<GameObject> displayedProducts;
    private GameObject gameManager;

    public string InteractionPrompt => _prompt;
    public int productCount;

    private void OnValidate()
    {
        if (displayPointParent)
            displayPoints = displayPointParent.GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (!productDAO) _prompt = "(E) Stock Shelves";
        productCount = 0;
    }

    public bool Interact(Interactor interactor)
    {
        PlayerData playerData = interactor.GetPlayerData();
        if (!productDAO)
        {
            AddNewProduct(playerData.GetCurrentHeldProductID(), playerData.GetCurrentHeldProductCount(), playerData);
            return true;
        }

        IncreaseCount(playerData.GetCountOfHeldProductWithID(productDAO.id), playerData);

        return true;
    }

    public ProductDAO GetProduct()
    {
        return productDAO;
    }

    public bool HasProduct()
    {
        return productDAO ? true : false;
    }

    private void AddNewProduct(int id, int count, PlayerData playerData)
    {
        productDAO = gameManager.GetComponent<ProductPrefabDictionary>().GetProductDAOByID(id);
        _prompt = $"(E) Stock Shelves with {productDAO.prefab.name}";
        IncreaseCount(count, playerData);
        AddProductsToDisplay();
    }

    private void IncreaseCount(int count, PlayerData playerData)
    {
        productCount += count;

        playerData.DecreaseCount(productDAO.id, count);
    }

    private void AddProductsToDisplay()
    {
        foreach(Transform point in displayPoints)
        {
            var product = Instantiate(productDAO.GetModelOnly(), point);
            product.transform.localScale = new Vector3(.2f, .2f, .2f);
        }
    }

    private void RemoveProductsFromDisplay()
    {
        displayedProducts = new List<GameObject>();
    }
}
