using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurManager : MonoBehaviour
{
    public static FurManager instance;

    [Header("General Components")]
    [SerializeField] private GameObject newFur;

    private void Awake()
    {
        if (instance != this && instance != null) Destroy(instance);
        else instance = this;   
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewFurniture(GameObject newFur)
    {
        this.newFur = newFur;
    }
    public void PurchaseNewFurniture()
    {
        GameObject parentObj = GameObject.Find(newFur.tag);

        // Destroy current furniture
        GameObject currentFur = parentObj.transform.GetChild(0).gameObject;
        Destroy(currentFur);

        //Spawn new furniture
        GameObject chosenFur = Instantiate(newFur, parentObj.transform);
        chosenFur.transform.SetParent(parentObj.transform);
    }
}
