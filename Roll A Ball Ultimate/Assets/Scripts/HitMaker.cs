using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMaker : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 0.1f;

    void Update() {
        foreach (RawImage img in GetComponentsInChildren<RawImage>()) {
            img.color = new Color(
                img.color.r,
                img.color.g,
                img.color.b,
                img.color.a - fadeSpeed * Time.deltaTime
            );
        }

        if (GetComponentInChildren<RawImage>().color.a <= 0) { Destroy(gameObject); }
    }

    public void Hit(Vector3 pos) {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pos);
    }
}
