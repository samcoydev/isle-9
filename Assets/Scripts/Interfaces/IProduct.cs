using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduct
{
    public void Grow();

    public void Shrink();

    public int GetProductID();
}
