using UnityEngine;
using TMPro;

public class DeathValue : MonoBehaviour {

    TextMeshProUGUI text;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        text.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPickup>().deaths.ToString();
    }
}
