using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private Enemy currentEnemy;

    public Enemy[] enemies;

    public int nEnemy;

    private void Update() {
        SpawnEnemy();
    }

    void SpawnEnemy() {
        if (nEnemy > 0) {
            if(currentEnemy == null) {
                int index = Random.Range(0, enemies.Length);
                currentEnemy = Instantiate(enemies[index], transform.position, Quaternion.identity);
                nEnemy--;
            }
        }
    }

}
