using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour, IProduct {
    [SerializeField] private ProductDAO productDAO;
    [SerializeField] private float minimumSize;
    [SerializeField] private float minimumGrowthTime;
    [SerializeField] private float maximumGrowthTime;
    [SerializeField] private bool shouldDrop;
    private float size;

    /* Only save the following if product hasn't grown */
    private float isGrowingTimer;
    private float isShrinkingTimer;
    private bool isGrowing;
    private bool isShrinking;
    private Vector3 sizeToScale;
    private Vector3 originPos;
    private float randomizedGrowthTime;
    private float randomizedShrinkTime;

    private void Start() {
        isShrinking = false;
        this.transform.localScale = Vector3.zero;
        if (!productDAO)
            Debug.LogError("Ensure all Product Prefabs have their respective ProductDAOs set, and vice versa!");
    }

    private void Update() {
        if (isGrowing)
            StartGrowth();
        if (isShrinking)
            StartShrinking();
    }

    public void Grow() {
        isGrowingTimer = 0;
        randomizedGrowthTime = Random.Range(minimumGrowthTime, maximumGrowthTime);
        size = Random.Range(minimumSize, 1);
        sizeToScale = new Vector3(size, size, size);
        isGrowing = true;
    }
    public void Shrink() {
        isShrinkingTimer = 0;
        originPos = transform.position;
        randomizedShrinkTime = Random.Range(minimumGrowthTime - .6f, maximumGrowthTime - .6f);
        isShrinking = true;
    }

    public int GetProductID() {
        return productDAO.id;
    }

    public string GetName() {
        return productDAO.name;
    }

    private void StartGrowth() {
        if (isGrowingTimer < randomizedGrowthTime) {
            isGrowingTimer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, sizeToScale, isGrowingTimer / randomizedGrowthTime);
        } else {
            isGrowing = false;
        }
    }

    private void StartShrinking() {

        if (isShrinkingTimer < randomizedShrinkTime) {
            isShrinkingTimer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(new Vector3(size, size, size), Vector3.zero, isShrinkingTimer / randomizedShrinkTime);

            if (shouldDrop)
                transform.position = Vector3.Lerp(new Vector3(originPos.x, originPos.y, originPos.z), new Vector3(originPos.x, originPos.y - 1, originPos.z), isShrinkingTimer / randomizedShrinkTime);
        } else {
            Destroy(gameObject);
        }
    }
}
