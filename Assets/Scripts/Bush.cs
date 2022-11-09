using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour, IInteractable, IPlant {
    [SerializeField] private string _prompt;

    [SerializeField] private float growthTime;
    [SerializeField] private float size;

    [SerializeField] private Bounds productSpawnBounds;
    [SerializeField] private int productCount;
    [SerializeField] private List<GameObject> grownProducts;

    private bool isDisabled;
    private float reGrowCounter;
    public float timeToReGrow;
    public float reGrowTimeMultiplier = 1f;

    public GameObject product;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor) {
        if (grownProducts.Count > 0)
            Harvest(interactor);
        return true;
    }

    private void Start() {
        this.transform.localScale = Vector3.zero;
    }

    public void StartGrowth() {
        StartCoroutine(Grow());
    }

    public bool IsHarvestable() {
        return grownProducts.Count == 0;
    }

    public string GetName() {
        return product.GetComponent<IProduct>().GetName().Length > 0 ? product.GetComponent<IProduct>().GetName() : "{no name found}";
    }

    IEnumerator Grow() {
        isDisabled = true;
        Vector3 sizeToScale = new Vector3(size, size, size);
        float counter = 0;

        while (counter < growthTime) {
            counter += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(Vector3.zero, sizeToScale, counter / growthTime);
            yield return null;
        }

        GrowProducts();

        yield break;
    }

    IEnumerator ReHarvestTime() {
        isDisabled = true;
        while (reGrowCounter < timeToReGrow) {
            reGrowCounter += Time.deltaTime * reGrowTimeMultiplier;
            yield return null;
        }

        GrowProducts();

        yield break;
    }

    private void GrowProducts() {
        isDisabled = false;
        reGrowCounter = 0;
        for (int i = 0; i < productCount; i++) {
            var pos = getRandomSpawnPointWithinBounds(productSpawnBounds);
            var newProduct = Instantiate(product, this.transform, false);

            newProduct.transform.Translate(pos);
            newProduct.GetComponent<IProduct>().Grow();
        }
        foreach (Transform child in transform) {
            if (child.TryGetComponent<IProduct>(out var _product))
                grownProducts.Add(child.gameObject);
        }
    }

    private void Harvest(Interactor interactor) {
        int productId = product.GetComponent<IProduct>().GetProductID();
        Debug.Log("Get pid" + productId);
        interactor.GetPlayerData().AddProduct(productId, grownProducts.Count);

        foreach (GameObject grownProduct in grownProducts) {
            grownProduct.GetComponent<IProduct>().Shrink();
        }

        grownProducts = new List<GameObject>();
        StartCoroutine(ReHarvestTime());
    }

    private static Vector3 getRandomSpawnPointWithinBounds(Bounds bounds) {

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public bool IsDisabled() {
        return isDisabled;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position + productSpawnBounds.center, productSpawnBounds.extents);
    }
}
