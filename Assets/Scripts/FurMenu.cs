using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurMenu: MonoBehaviour
{
    [SerializeField] private GameObject furniture;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetNewFurniture()
    {
        FurManager.instance.SetNewFurniture(furniture);
    }
    public void PurchaseNewFurniture()
    {
        FurManager.instance.PurchaseNewFurniture();
    }
}
