using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{
    public static FurnitureManager instance;

    [Header("General Components")]
    public GameObject furPrefab;

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

    public void ReplaceFurniture()
    {
        GameObject couch = GameObject.Find("Couch");

        // Destroy current furniture
        GameObject currentFur = couch.transform.GetChild(0).gameObject;
        Destroy(currentFur);

        // Instantiate under the couch immediately
        GameObject newFur = Instantiate(furPrefab, couch.transform);
        newFur.transform.SetParent(couch.transform);
    }
}
