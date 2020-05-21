using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {

    public float damage;

    public float attackRate;

    public float attackSpeed;

    public Vector2 orgPos;

    public WeaponAttackType attackType;

    public Rigidbody2D rb;

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public void Rotate(Transform targetPos) {
        Vector2 lookDir = rb.position - (Vector2)targetPos.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90;
        rb.rotation = angle;
    }

    public IEnumerator Attack(Vector2 _target) {
        Vector2 targetPos = _target;
        FindObjectOfType<AudioManager>().Play("EnemySlashSword");   
        float percent = 0;

        while (percent <= 1) {
            percent += Time.deltaTime * attackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(orgPos, targetPos, formula);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Health health = other.gameObject.GetComponent<Health>();
            health.TakeDamage(damage);
            FindObjectOfType<AudioManager>().Play("Hit");
        }
    }

}
