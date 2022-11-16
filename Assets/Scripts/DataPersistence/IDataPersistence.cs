using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameDataCollection data);

    void SaveData(ref GameDataCollection data);
}
