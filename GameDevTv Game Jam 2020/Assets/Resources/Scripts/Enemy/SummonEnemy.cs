using UnityEngine;
using Random = UnityEngine.Random;

public class SummonEnemy : Enemy {

    public Vector2 patrolPoint;

    public float patrolSpeed;

    private Animator anim;

    private Rigidbody2D rb;

    public float offset;

    public float startSummonTime;

    private float summonTime;

    public Enemy[] enemies;

    public int nSummons;

    private void Start() {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        summonTime = startSummonTime;
        enemyState = EnemyState.Patroll;
        float x = Random.Range(transform.position.x - offset, transform.position.x + offset);
        float y = Random.Range(transform.position.y - offset, transform.position.y + offset);
        patrolPoint = new Vector2(x, y);
    }

    private void Update() {
        HandleState();
    }

    private void HandleState() {
        if (enemyState == EnemyState.Patroll) {
            anim.SetBool("Walk", true);
            rb.position = Vector2.MoveTowards(rb.position, patrolPoint, patrolSpeed * Time.deltaTime);
            if (rb.position == patrolPoint) {
                enemyState = EnemyState.Idle;
            }
        }

        if (enemyState == EnemyState.Idle) {
            rb.position = transform.position;
            anim.SetBool("Walk", false);
            if (summonTime <= 0) {
                enemyState = EnemyState.Summon;
                summonTime = startSummonTime;
            } else {
                summonTime -= Time.deltaTime;
            }
        }

        if (enemyState == EnemyState.Summon) {
            anim.SetTrigger("Summon");
            enemyState = EnemyState.Idle;
        }


    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("WallMap")) {
            patrolPoint = rb.position;
        }
    }

    public void Summon() {
        if (nSummons > 0) {
            Enemy enemyToSpawn = enemies[Random.Range(0, enemies.Length)];
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            nSummons--;
        }
    }
}
