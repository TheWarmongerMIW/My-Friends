using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureMenu: MonoBehaviour
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
        FurnManager.instance.SetNewFurniture(furniture);
    }
    public void PurchaseNewFurniture()
    {
        FurnManager.instance.PurchaseNewFurniture();
    }
}
