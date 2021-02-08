using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool invertYAxis = true;

    [SerializeField] private Transform target;
    [SerializeField] private LayerMask rayMask;

    [SerializeField] private Vector3 normalOffset;
    [SerializeField] private Vector3 adsOffset;
    [SerializeField] private float lerpSpeed = 5f;

    private float cameraXRot = 0f;

    private Vector3 offset;

    void Start() {
        offset = normalOffset;
    }

    void Update() {
        if (invertYAxis) {
            cameraXRot -= Input.GetAxis("Mouse Y") * GameManager.mouseSensitivity;
        } else {
            cameraXRot += Input.GetAxis("Mouse Y") * GameManager.mouseSensitivity;
        }
    }

    void LateUpdate() {
        Vector3 newPos = target.position;
        Vector3 targetOffset;

        if (GameObject.FindWithTag("Weapon").GetComponent<Weapon>().adsing) {
            targetOffset = adsOffset;
        } else {
            targetOffset = normalOffset;
        }

        offset = Vector3.Lerp(offset, targetOffset, Time.deltaTime * lerpSpeed);

        cameraXRot = Mathf.Clamp(cameraXRot, -89.9f, 89.9f);

        transform.rotation = target.rotation;
        transform.eulerAngles = new Vector3( cameraXRot, transform.eulerAngles.y, 0 );

        newPos += transform.right * offset.x;

        newPos += transform.up * offset.y;

        newPos += transform.forward * offset.z;

        RaycastHit hit;
        Vector3 direction = newPos - target.position;

        if (Physics.Raycast(target.position, direction, out hit, direction.magnitude, rayMask)) {
            newPos = hit.point - (direction * 0.1f);
        }

        transform.position = newPos;
        //transform.LookAt(target.position);
    }
}
