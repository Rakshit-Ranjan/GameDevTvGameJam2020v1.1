    using UnityEngine;
using TMPro;

public class totalDeathValue : MonoBehaviour {
    TextMeshProUGUI text;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
        text.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPickup>().totalDeath.ToString();
    }
}
