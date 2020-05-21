using System.Collections;
using UnityEngine;

public class PunchingEnemy : Enemy {

    private Rigidbody2D rb;

    private Animator anim;

    private Transform playerTransform;

    [SerializeField]
    private float distToPlayer;

    public float chaseDistance;

    public float attackDistance;

    public float chaseSpeed;

    public float startTimeBtwAttack;

    private float timeBtwAttack;

    public float offset;

    private Vector2 patrolPoint;

    private void Start() {
        timeBtwAttack = startTimeBtwAttack;
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float x = Random.Range(transform.position.x - offset, transform.position.x + offset);
        float y = Random.Range(transform.position.y - offset, transform.position.y + offset);
        patrolPoint = new Vector2(x, y);
    }

    private void FixedUpdate() {
        ConfigureState();
        HandleState();
    }

    public void ConfigureState() {
        if (playerTransform != null) {
            distToPlayer = Vector2.Distance(rb.position, playerTransform.position);
        }
        if (distToPlayer >= chaseDistance) {
            enemyState = EnemyState.Patroll;
        }
        if (!changeState) {
            if (distToPlayer < chaseDistance && distToPlayer > attackDistance) {
                enemyState = EnemyState.Chase;
            }
        }

        if (distToPlayer <= attackDistance) {
            enemyState = EnemyState.Attack;
        }


        if (changeState) {
            if (distToPlayer > attackDistance) {
                enemyState = EnemyState.Chase;
            }
        }
    }

    public void HandleState() {
        if (enemyState == EnemyState.Patroll) {
            anim.SetBool("Walk", false);
            if (rb.position != patrolPoint) {
                rb.position = Vector2.MoveTowards(rb.position, patrolPoint, chaseSpeed * Time.deltaTime);
            } else {
                float x = Random.Range(transform.position.x - offset, transform.position.x + offset);
                float y = Random.Range(transform.position.y - offset, transform.position.y + offset);
                patrolPoint = new Vector2(x, y);
            }
        }

        if (enemyState == EnemyState.Chase) {
            anim.SetBool("Walk", true);
            rb.position = Vector2.MoveTowards(rb.position, playerTransform.position, chaseSpeed * Time.deltaTime);
        }

        if (enemyState == EnemyState.Attack) {
            changeState = false;
            anim.SetBool("Walk", false);
            if (timeBtwAttack <= 0) {
                StartCoroutine(Attack());
                timeBtwAttack = startTimeBtwAttack;
            } else {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    public IEnumerator Attack() {
        isAttacking = true; 
        Vector2 orgPos = rb.position;
        Vector2 targetPos = playerTransform.position;
        float percent = 0;

        while (percent <= 1) {
            percent += Time.deltaTime * 4f;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(orgPos, targetPos, formula);
            yield return null;
        }

        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("WallMap") || collision.gameObject.CompareTag("Enemy")) {
            float x = Random.Range(transform.position.x - offset, transform.position.x + offset);
            float y = Random.Range(transform.position.y - offset, transform.position.y + offset);
            patrolPoint = new Vector2(x, y);
        }
        DoDamage(collision);
    }

}
