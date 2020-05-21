using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour {

    public Enemy[] enemies;

    public int nEnemy;

    public float timeBtwSpawns;

    public float startTimeBtwSpawns;

    private void Start() {
        timeBtwSpawns = startTimeBtwSpawns;
    }

    private void Update() {
        if (nEnemy > 0) {
            if (timeBtwSpawns <= 0) {
                float x = Random.Range(transform.position.x - 2f, transform.position.x + 2f);
                float y = Random.Range(transform.position.y - 2f, transform.position.y + 2f);
                Vector2 randomSpot = new Vector2(x, y);
                foreach (Enemy enemy in enemies) {
                    Instantiate(enemy, randomSpot, Quaternion.identity);
                    nEnemy--;
                }
                timeBtwSpawns = startTimeBtwSpawns;
            } else {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }

}
