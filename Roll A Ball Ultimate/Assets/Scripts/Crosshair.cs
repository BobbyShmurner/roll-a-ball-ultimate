using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private float adsDistance;
    [SerializeField] private float normalDistance;
    [SerializeField] private float lerpSpeed =5f;
    [SerializeField] private float crossLerpSpeed = 10f;

    [SerializeField] private float adsFov = 50f;
    [SerializeField] private float normalAlpha = 0.25f;
    [SerializeField] private float adsAlpha = 0.5f;

    [SerializeField] private GameObject[] crosshairs;

    private Camera cam;
    private Weapon weapon;

    private float normalFov;

    /* The Crosshair Order goes:
        Top
        Left
        Bottom
        Right
        Center
        Cross
    */

    void Start() {
        cam = Camera.main;

        normalFov = cam.fieldOfView;
    }

    void Update()
    {
        weapon = GameObject.FindWithTag("Weapon").GetComponent<Weapon>();
        if (weapon.adsing) {
            UpdateCrosshair(adsDistance, adsFov, adsAlpha);
        } else {
            UpdateCrosshair(normalDistance, normalFov, normalAlpha);
        }
    }

    void UpdateCrosshair(float distance, float fov, float alpha) {
        crosshairs[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Mathf.Lerp(crosshairs[0].GetComponent<RectTransform>().anchoredPosition.y, distance, Time.deltaTime * lerpSpeed));
        crosshairs[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Lerp(crosshairs[1].GetComponent<RectTransform>().anchoredPosition.x, -distance, Time.deltaTime * lerpSpeed), 0f);
        crosshairs[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Mathf.Lerp(crosshairs[2].GetComponent<RectTransform>().anchoredPosition.y, -distance, Time.deltaTime * lerpSpeed));
        crosshairs[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Lerp(crosshairs[3].GetComponent<RectTransform>().anchoredPosition.x, distance, Time.deltaTime * lerpSpeed), 0f);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * lerpSpeed);

        foreach (GameObject i in crosshairs) {
            Color newCol = new Color(
                i.GetComponent<RawImage>().color.r,
                i.GetComponent<RawImage>().color.g,
                i.GetComponent<RawImage>().color.b,
                alpha
            );

            i.GetComponent<RawImage>().color = Color.Lerp(i.GetComponent<RawImage>().color, newCol, Time.deltaTime * lerpSpeed);
        }

        //Check For Object In The Way

        if (weapon.objectInWay) {
            crosshairs[5].SetActive(true);

            crosshairs[5].GetComponent<RectTransform>().position = Vector3.Lerp(crosshairs[5].GetComponent<RectTransform>().position, cam.WorldToScreenPoint(weapon.objectInWayPoint), Time.deltaTime * crossLerpSpeed);

            foreach (GameObject i in crosshairs) {
                Color newCol = new Color(
                    1f,
                    0f,
                    0f,
                    i.GetComponent<RawImage>().color.a
                );

                i.GetComponent<RawImage>().color = newCol;
            }
        } else {
            crosshairs[5].SetActive(false);

            foreach (GameObject i in crosshairs) {
                Color newCol = new Color(
                    1f,
                    1f,
                    1f,
                    i.GetComponent<RawImage>().color.a
                );

                i.GetComponent<RawImage>().color = newCol;
            }
        }
    }
}
