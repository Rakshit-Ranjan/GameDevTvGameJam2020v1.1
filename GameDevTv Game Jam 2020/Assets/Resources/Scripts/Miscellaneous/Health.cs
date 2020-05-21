using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public float health;

    public bool isEnemy;

    private void Awake() {
        PlayerMovement move = GetComponent<PlayerMovement>();
        if(move != null) {
            isEnemy = false;
        } else {
            isEnemy = true;
        }
    }

    public void TakeDamage(float amount) {
        health -= amount;

        if(health <= 0) {
            if(isEnemy) {
                if (Random.value < 0.5f) {
                    Enemy enemy = GetComponent<Enemy>();
                    Instantiate(enemy.chests[Random.Range(0, enemy.chests.Length)], transform.position, Quaternion.identity);
                }
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPickup>().deaths++;
            } else {
                SceneManager.LoadScene("LoseScene");
            }
            Destroy(gameObject);
        }
    }

}