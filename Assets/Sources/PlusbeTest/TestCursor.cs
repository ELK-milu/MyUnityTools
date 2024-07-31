using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCursor : MonoBehaviour {

    public Texture2D texture;

    void LateUpdate()
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }
}
