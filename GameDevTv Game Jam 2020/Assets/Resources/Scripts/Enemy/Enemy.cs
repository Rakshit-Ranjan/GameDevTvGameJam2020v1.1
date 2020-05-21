using UnityEngine;

public class Enemy : MonoBehaviour {

    public bool isAttacking;

    public bool changeState;

    public EnemyType enemyType;

    public EnemyState enemyState;

    public float damage;

    public bool isDead;

    public Chest[] chests;

    private void OnDestroy() {
        FindObjectOfType<AudioManager>().Play("DeathEnemy");
    }

    public void DoDamage(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Health health = other.gameObject.GetComponent<Health>();
            health.TakeDamage(damage);
        }
    }
}
