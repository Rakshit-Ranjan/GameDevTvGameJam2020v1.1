using System.Collections;
using UnityEngine;

public class BossDemon : Enemy {

    private Rigidbody2D rb;

    private Animator anim;

    public Transform playerTransform;

    public float distToPlayer;

    public float chaseSpeed;

    public float offset;

    public GameObject projectilePrefab;

    public Transform dropPoint;

    public float startTimeBtwAttack;

    private float timeBtwAttack;

    public float startHealth;

    public float health;

    public float chaseRange;

    public float attackRange;

    private Vector2 patrolPoint;

    public float startSummonTime;

    public int nSummons = 10;

    public float summonTime;

    public Enemy[] enemies;

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        health = startHealth;
        summonTime = startSummonTime;
        timeBtwAttack = startTimeBtwAttack;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        distToPlayer = Vector2.Distance(rb.position, playerTransform.position);
        ConfigureType();
        HandleType();
    }

    private void ConfigureType() {
        if (health > startHealth / 2) {
            enemyType = EnemyType.Summoner;
        }
        if (health <= startHealth / 2) {
            enemyType = EnemyType.Spitter;
        }
    }

    private void HandleType() {
        if (enemyType == EnemyType.Summoner) {
            Summoner();
        }
        if (enemyType == EnemyType.Spitter) {
            Spitter();
        }
    }

    private void Summoner() {
        anim.SetBool("Walk", false);
        Summon();
    }

    private void Spitter() {
        if (distToPlayer <= attackRange) {
            AttackPlayer();
        } else {
            ChasePlayer();
        }
    }

    private void AttackPlayer() {
        rb.position = transform.position;
        if (timeBtwAttack <= 0) {
            anim.SetTrigger("Attack");
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            timeBtwAttack = startTimeBtwAttack;
        } else {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void ChasePlayer() {
        rb.position = Vector2.MoveTowards(rb.position, playerTransform.position, chaseSpeed * Time.deltaTime);
        anim.SetBool("Walk", true);
        if (Random.value < 0.5f) {
            if (timeBtwAttack <= 0) {
                anim.SetTrigger("Attack");
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                timeBtwAttack = startTimeBtwAttack;
            } else {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    private void Summon() {
        if (nSummons > 0) {
            if (summonTime <= 0) {
                int index = Random.Range(0, enemies.Length);
                anim.SetTrigger("Summon");
                Instantiate(enemies[index], dropPoint.position, Quaternion.identity);
                nSummons--;
                summonTime = startSummonTime;
            } else {
                summonTime -= Time.deltaTime;
            }
        } else {
            if (distToPlayer <= chaseRange && distToPlayer > attackRange) {
                rb.position = Vector2.MoveTowards(rb.position, playerTransform.position, chaseSpeed * Time.deltaTime);
                anim.SetBool("Walk", false);
            }
            if (distToPlayer <= attackRange) {
                rb.position = transform.position;
                if (timeBtwAttack <= 0) {
                    StartCoroutine(Attack(transform.position, playerTransform.position));
                    timeBtwAttack = startTimeBtwAttack;
                } else {
                    timeBtwAttack -= Time.deltaTime;
                }
            }
        }
    }

    Vector2 SetRandomPosition(Vector2 _pos, float _offset) {
        float _x = Random.Range(rb.position.x - _offset, rb.position.x + _offset);
        float _y = Random.Range(rb.position.y - _offset, rb.position.y + _offset);
        _pos = new Vector2(_x, _y);
        return _pos;
    }

    IEnumerator Attack(Vector2 _orgPos, Vector2 _targetPos) {
        Vector2 orgPos = _orgPos;
        Vector2 targetPos = _targetPos;
        float percent = 0;
        while (percent <= 1) {
            percent += Time.deltaTime * 4f;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(orgPos, targetPos, formula);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        DoDamage(collision);
    }

} // class


































