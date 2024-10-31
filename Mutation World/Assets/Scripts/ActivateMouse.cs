using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Make the cursor visible when the scene starts
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
