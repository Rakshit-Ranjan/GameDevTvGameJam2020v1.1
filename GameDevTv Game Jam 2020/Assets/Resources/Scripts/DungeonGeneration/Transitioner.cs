using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour {

    public List<GameObject> spawners = new List<GameObject>();

    public bool allDead;

    public Vector2 transportationPos;

    public Camera minimap;

    int times = 1;

    private void Update() {
        PlayerPickup playerPickup = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPickup>();
        if (playerPickup.deaths == playerPickup.totalDeath) {
            allDead = true;
        }

        if(allDead) {
            if(times > 0) {
                playerPickup.transform.position = transportationPos;
                GameObject.FindGameObjectWithTag("Storage").transform.position = new Vector3(transportationPos.x + 2f, transportationPos.y);
                times--;
            }
            minimap.fieldOfView = 30f;
        }
    }
}
