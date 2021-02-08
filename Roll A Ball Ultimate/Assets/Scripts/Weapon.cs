using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float fireForce = 5f;
    [SerializeField] private float fireDelay = 0.1f;

    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private LayerMask rayMaskExclude;

    [SerializeField] private Transform firingPoint;

    [HideInInspector] public bool objectInWay = false;
    [HideInInspector] public bool adsing = false;
    [HideInInspector] public Vector3 objectInWayPoint;

    [SerializeField] private GameObject hitMaker;

    private Camera cam;
    private Animator animator;
    private Transform canvas;

    private bool firing = false;

    private GameObject targetObject;
    private Vector3 targetPoint;

    void Start() {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        canvas = GameObject.FindWithTag("Canvas").transform;
    }

    void Update() {
        RotateTowardsPoint();

        if (Input.GetButton("Fire2")) {
            adsing = true;
        } else {
            adsing = false;
        }

        if (Input.GetButton("Fire1") && !firing) {
            animator.Play("Gun|Idel", -1, 1f);
            Fire();
        }

        UpdateAnimations();
    }

    void Fire() {
        firing = true;

        if (targetObject && targetObject.CompareTag("Enemy")) {
            GameObject hMaker = Instantiate(hitMaker, canvas);

            hMaker.GetComponent<HitMaker>().Hit(targetPoint);

            Kill();
        }

        StartCoroutine(FireDelay(fireDelay));
    }

    void Kill() {
        Rigidbody targetRb = targetObject.GetComponent<Rigidbody>();
        Animator targetAnimator = targetObject.GetComponent<Animator>();

        if (!targetRb) {
            targetRb = targetObject.transform.parent.gameObject.GetComponent<Rigidbody>();
            Collider tmpCollider = targetObject.transform.parent.gameObject.GetComponent<Collider>();

            if (!targetRb || tmpCollider) {
                targetRb = null;
            }
        }

        if (!targetAnimator) {
            targetAnimator = targetObject.GetComponentInParent<Animator>();
        }

        if (targetAnimator) {
            targetAnimator.enabled = false;

            foreach (Rigidbody tmpRb in targetAnimator.gameObject.GetComponentsInChildren<Rigidbody>()) {
                tmpRb.isKinematic = false;
            }
        }

        if (targetRb) {
            Vector3 forceDir = (targetPoint - firingPoint.position).normalized;

            foreach (Rigidbody tmpRb in targetObject.transform.root.gameObject.GetComponentsInChildren<Rigidbody>()) {
                tmpRb.constraints = RigidbodyConstraints.None;
                tmpRb.AddForceAtPosition(forceDir * fireForce, targetPoint);
            }
        }
    }

    IEnumerator FireDelay(float delay) {
        yield return new WaitForSeconds(delay);
        firing = false;
    }

    void RotateTowardsPoint() {
        RaycastHit hit;
        Quaternion endDir = Quaternion.LookRotation(cam.gameObject.transform.forward * 100000f - firingPoint.position);
        Quaternion originalDir = firingPoint.rotation;
        bool hitObject = false;

        targetObject = null;
        targetPoint = Vector3.zero;

        if (Physics.Raycast(cam.gameObject.transform.position, cam.gameObject.transform.forward, out hit, 100000f, ~rayMaskExclude)) {
            endDir = Quaternion.LookRotation(hit.point - firingPoint.position);
            firingPoint.rotation = endDir;
            hitObject = true;

            targetObject = hit.collider.gameObject;
            targetPoint = hit.point;
        }

        if (Physics.Raycast(firingPoint.position, firingPoint.forward, out hit, 100000f, ~rayMaskExclude)) {
            if (!hitObject || (targetPoint - hit.point).magnitude > 0.1f) {
                objectInWay = true;
                objectInWayPoint = hit.point;

                targetObject = hit.collider.gameObject;
                targetPoint = hit.point;
            } else {
                objectInWay = false;
            }
        } else {
            objectInWay = false;
        }

        firingPoint.rotation = originalDir;
        firingPoint.rotation = Quaternion.Lerp(firingPoint.rotation, endDir, Time.deltaTime * lerpSpeed);

        originalDir = firingPoint.rotation;

        transform.rotation = firingPoint.rotation;
        firingPoint.rotation = originalDir;
    }

    void UpdateAnimations() {
        animator.SetBool("Fire", firing);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(firingPoint.position, firingPoint.forward * 100000f);
    }
}
