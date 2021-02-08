using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField] private Vector3 rot = new Vector3(15, 30, 45);
    [SerializeField] private bool collectable = true;

    void Update()
    {
        transform.Rotate(rot * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player")) {
            GameManager.collectables++;
            Destroy(gameObject);
        }
    }
}
