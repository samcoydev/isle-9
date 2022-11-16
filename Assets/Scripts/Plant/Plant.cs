using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour, IInteractable, IPlant, IDataPersistence {

    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() {
        id = System.Guid.NewGuid().ToString();
    }

    [SerializeField] protected string _prompt;

    [SerializeField] private float growthTime;
    [SerializeField] private float size;

    [SerializeField] protected Bounds productSpawnBounds;
    [SerializeField] private int productCount; // SAVE
    [SerializeField] private List<GameObject> grownProducts;

    private bool isDisabled;
    private bool isGrown;
    private bool isReGrowingProduct; // SAVE


    private float reGrowCounter; // SAVE
    public float timeToReGrow;
    public float reGrowTimeMultiplier = 1f;

    public ProductDAO productDAO;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor) {
        if (grownProducts.Count > 0)
            Harvest(interactor);
        return true;
    }

    private void Start() {
        if (!isGrown)
            transform.localScale = Vector3.zero;
        SetPrompt();
    }

    public void LoadData(GameDataCollection data) {
        PlantData plantData = data.plants.ContainsKey(id) ? data.plants[id] : GetInitializedPlantData();

        isGrown = plantData.isGrown;
        productCount = plantData.productCount;
        isReGrowingProduct = plantData.isRegrowingProduct;
        reGrowCounter = plantData.reGrowCounter;

        if (!isReGrowingProduct)
            GrowProducts();
    }

    public void SaveData(ref GameDataCollection data) {
        if (!isGrown) return;

        PlantData plantData = new();

        plantData.isGrown = isGrown;
        plantData.productCount = productCount;
        plantData.isRegrowingProduct = isReGrowingProduct;
        plantData.reGrowCounter = reGrowCounter;


        if (!data.plants.ContainsKey(id))
            data.plants.Add(id, plantData);
        else
            data.plants[id] = plantData;
    }

    private PlantData GetInitializedPlantData() {
        PlantData plantData = new();
        plantData.isGrown = false;
        plantData.productCount = productCount;
        plantData.isRegrowingProduct = false;
        plantData.reGrowCounter = 0;

        return plantData;
    }

    private void Update() {
        if (isReGrowingProduct)
            CountReHarvestTime();
    }

    protected abstract void SetPrompt();

    public void StartGrowth() {
        StartCoroutine(Grow());
    }

    public bool IsHarvestable() {
        return grownProducts.Count == 0;
    }

    public string GetName() {
        return productDAO.prefab.GetComponent<IProduct>().GetName().Length > 0 ? productDAO.prefab.GetComponent<IProduct>().GetName() : "{no name found}";
    }

    protected IEnumerator Grow() {
        isDisabled = true;
        Vector3 sizeToScale = new Vector3(size, size, size);
        float counter = 0;

        while (counter < growthTime) {
            counter += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(Vector3.zero, sizeToScale, counter / growthTime);
            yield return null;
        }

        GrowProducts();
        isGrown = true;

        yield break;
    }

    protected void CountReHarvestTime() {
        isDisabled = true;
        if (reGrowCounter < timeToReGrow)
            reGrowCounter += Time.deltaTime * reGrowTimeMultiplier;

        else {
            GrowProducts();
            isReGrowingProduct = false;
        }
    }

    private void GrowProducts() {
        isDisabled = false;
        reGrowCounter = 0;
        for (int i = 0; i < productCount; i++) {
            var pos = GetRandomSpawnPointWithinBounds(productSpawnBounds);
            var newProduct = Instantiate(productDAO.prefab, this.transform, false);

            newProduct.transform.Translate(pos);
            newProduct.GetComponent<IProduct>().Grow();
        }
        foreach (Transform child in transform) {
            if (child.TryGetComponent<IProduct>(out var _product))
                grownProducts.Add(child.gameObject);
        }
    }

    private void Harvest(Interactor interactor) {
        interactor.GetPlayerData().AddProduct(productDAO.id, grownProducts.Count);

        foreach (GameObject grownProduct in grownProducts) {
            grownProduct.GetComponent<IProduct>().Shrink();
        }

        grownProducts = new List<GameObject>();
        isReGrowingProduct = true;
    }

    protected abstract Vector3 GetRandomSpawnPointWithinBounds(Bounds bounds);

    public bool IsDisabled() {
        return isDisabled;
    }
}
