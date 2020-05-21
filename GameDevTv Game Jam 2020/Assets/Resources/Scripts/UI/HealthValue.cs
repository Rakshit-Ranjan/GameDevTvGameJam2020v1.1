using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthValue : MonoBehaviour {

    TextMeshProUGUI text;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }


    void Update() {
        text.text = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().health.ToString();
    }
}
