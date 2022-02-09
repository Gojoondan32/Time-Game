using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookX : MonoBehaviour
{
    [SerializeField] private float sensitivity = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Lock the cursor position
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Rotate the camera around the x axis
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }
}
