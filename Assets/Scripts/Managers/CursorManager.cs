using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    [Header("Cursor List")]
    [SerializeField] private string currentCursor;
    [SerializeField] private CursorList cursorList;
    private Dictionary<string, CursorType> cursorLookup;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;

        cursorLookup = new Dictionary<string, CursorType>();
        foreach (var cursor in cursorList.cursorList)
            cursorLookup[cursor.name] = cursor;
    }
    private void Start()
    {
        SetCursor("Arrow");
    }
    private void Update()
    {
        
    }

    public string GetCurrentCursor()
    {
        return currentCursor;   
    }
    public void SetCursor(string name)
    {
        if (cursorLookup.TryGetValue(name, out var cursor))
        {
            Cursor.SetCursor(cursor.cursorTextures[0], cursor.cursorHotspot, CursorMode.Auto);
            currentCursor = name;
        }
        else
            Debug.LogWarning($"Cursor '{name}' not found!");
    }
    public IEnumerator ResetCursorToArrow(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetCursor("Arrow");
    }
    public void SetCursorProgress(string name, float progress)
    {
        if (!cursorLookup.TryGetValue(name, out var cursor))
        {
            Debug.LogWarning($"Cursor '{name}' not found!");
            return;
        }

        int frameIndex = Mathf.Clamp(Mathf.FloorToInt(progress * (cursor.cursorTextures.Length - 1)), 0, cursor.cursorTextures.Length - 1);
        Cursor.SetCursor(cursor.cursorTextures[frameIndex], cursor.cursorHotspot, CursorMode.Auto);
    }
}

