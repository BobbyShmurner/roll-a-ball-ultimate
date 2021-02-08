using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform forwardDir;

    [SerializeField] float movementSpeed = 500f;
    [SerializeField] float jumpForce = 250f;
    [SerializeField] float jumpCheckRadius = 0.1f;
    [SerializeField] float jumpCheckOffset = 0.5f;
    [SerializeField] LayerMask jumpMaskExclude;

    [SerializeField] float cameraTransparantDistance = 0.5f;
    [SerializeField] Material transMat;

    private Rigidbody rb;
    private Camera cam;
    private Renderer renderer;
    private Material mat;

    private bool canJump = false;

    void Start() {
        rb= GetComponent<Rigidbody>();
        cam = Camera.main;
        renderer = GetComponent<Renderer>();
        mat = renderer.material;
    }

    void Update()
    {
        Movement();

        JumpCheck();
        if (Input.GetButtonDown("Jump") && canJump) { Jump(); }

        UpdateCameraDistanceColor();
    }

    void Movement() {
        Vector3 force = Vector3.zero;

        force += forwardDir.right * Input.GetAxis("Horizontal") * movementSpeed;
        force += forwardDir.forward * Input.GetAxis("Vertical") * movementSpeed;

        rb.AddForce(force * Time.deltaTime);
    }

    void Jump() {
        rb.AddForce(Vector3.up * jumpForce);
    }

    void JumpCheck() {
        canJump = Physics.CheckSphere(transform.position - new Vector3(0f, jumpCheckOffset * transform.localScale.y, 0f), jumpCheckRadius, ~jumpMaskExclude);
    }

    void UpdateCameraDistanceColor() {
        float distance = Mathf.Clamp((cam.gameObject.transform.position - transform.position).magnitude - cameraTransparantDistance, 0f, 1f);

        if (distance < 1f) {
            renderer.material = transMat;

            transMat.color = new Color(mat.color.r, mat.color.g, mat.color.b, distance);
        } else {
            renderer.material = mat;
        }
    }

    void OnDrawGizmos() {
        if (canJump) {
            Gizmos.color = new Vector4(0f, 1f, 0f, 1f);
        } else {
            Gizmos.color = new Vector4(1f, 0f, 0f, 1f);
        }

        Gizmos.DrawWireSphere(transform.position - new Vector3(0f, jumpCheckOffset * transform.localScale.y, 0f), jumpCheckRadius);
    }
}
