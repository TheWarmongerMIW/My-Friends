using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementIndicatorManager : MonoBehaviour
{
    public static MovementIndicatorManager instance;

    [Header("General Components")]
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private float indicatorTimer;
    [SerializeField] private float spawnHeight;

    private GameObject currentIndicatorPrefab;
    private Coroutine currentIndicator;

    void Start()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowIndicator(Vector3 des)
    {
        if (currentIndicator != null)
        {
            StopCoroutine(currentIndicator);
            Destroy(currentIndicatorPrefab);
        }
        currentIndicator = StartCoroutine(DisplayIndicatorPrefab(des));
    }
    public void DestroyIndicator()
    {
        Destroy(currentIndicatorPrefab);
    }
    private IEnumerator DisplayIndicatorPrefab(Vector3 des)
    {
        currentIndicatorPrefab = Instantiate(indicatorPrefab, new Vector3(des.x, des.y + spawnHeight, des.z), transform.rotation);
        yield return new WaitForSeconds(indicatorTimer);

        Destroy(currentIndicatorPrefab);
        currentIndicatorPrefab = null;
        currentIndicator = null;
    }
}
