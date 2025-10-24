using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class CursorList : ScriptableObject
{
    public List<CursorType> cursorList = new();

#if UNITY_EDITOR
    private void OnEnable()
    {
        for (int i = 0; i < cursorList.Count; i++)
            if (cursorList[i].cursorTextures.Length == 1)
                cursorList[i].name = cursorList[i].cursorTextures[0].name;
            else cursorList[i].name = "Loading";
    }
#endif
}

[System.Serializable]
public class CursorType
{
    [HideInInspector] public string name;
    public Vector2 cursorHotspot;
    public Texture2D[] cursorTextures;
}
