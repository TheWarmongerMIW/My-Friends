using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class CursorModeManager : MonoBehaviour
{
    public static CursorModeManager instance;

    public UnityEvent OnMultiSelectEnable;
    public UnityEvent OnMultiSelectDisable;
    public bool enableMultiSelect;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    void Start()
    {
        OnMultiSelectDisable.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (enableMultiSelect) OnMultiSelectEnable.Invoke();
    }
}
