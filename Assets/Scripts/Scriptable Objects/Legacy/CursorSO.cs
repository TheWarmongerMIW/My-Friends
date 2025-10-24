using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CursorSO : ScriptableObject
{
    public string cursorName;
    public Vector2 cursorHotSpot;
    public Texture2D[] cursorTextures;
}

