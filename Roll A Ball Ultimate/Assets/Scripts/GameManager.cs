using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int collectables = 0;
    public static float mouseSensitivity = 3f;

    [SerializeField] private float mouseSens = 3f;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        mouseSensitivity = mouseSens;

        scoreText.text = collectables.ToString();
    }
}
