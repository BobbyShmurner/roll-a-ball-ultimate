using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForward : MonoBehaviour
{
    private Vector3 dir;

    void Update() {
        dir += new Vector3( 0f, Input.GetAxis("Mouse X") * GameManager.mouseSensitivity, 0f );
        transform.rotation = Quaternion.Euler(dir);
    }
}
