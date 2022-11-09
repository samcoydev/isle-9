using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour, IProduct
{
    [SerializeField] private ProductDAO productDAO;
    [SerializeField] private float minimumSize;
    [SerializeField] private float minimumGrowthTime;
    [SerializeField] private float maximumGrowthTime;
    [SerializeField] private bool shouldDrop;
    private float size;

    private void Start()
    {
        this.transform.localScale = Vector3.zero;
        if (!productDAO)
            Debug.LogError("Ensure all Product Prefabs have their respective ProductDAOs set, and vice versa!");
    }

    public void Grow()
    {
        StartCoroutine(EGrow());
    }
    public void Shrink()
    {
        StartCoroutine(EShrink());
    }

    public int GetProductID()
    {
        return productDAO.id;
    }

    public string GetName() {
        return productDAO.name;
    }

    private IEnumerator EGrow()
    {
        var _size = Random.Range(minimumSize, 1);
        size = _size;
        Vector3 sizeToScale = new Vector3(_size, _size, _size);
        float counter = 0;

        var growthTime = Random.Range(minimumGrowthTime, maximumGrowthTime);
        while (counter < growthTime)
        {
            counter += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(Vector3.zero, sizeToScale, counter / growthTime);
            yield return null;
        }

        yield break;
    }

    private IEnumerator EShrink()
    {
        float counter = 0;
        float tx = this.transform.position.x;
        float ty = this.transform.position.y;
        float tz = this.transform.position.z;
        float dropLength = Random.Range(.5f, 1.5f);

        var shrinkTime = Random.Range(minimumGrowthTime - .6f, maximumGrowthTime - .6f);
        while (counter < shrinkTime)
        {
            counter += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(new Vector3(size, size, size), Vector3.zero, counter / shrinkTime);

            if (shouldDrop)
                this.transform.position = Vector3.Lerp(new Vector3(tx, ty, tz), new Vector3(tx, ty - dropLength, tz), counter / shrinkTime);

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
