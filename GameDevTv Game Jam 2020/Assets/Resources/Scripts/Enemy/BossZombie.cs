using System.Collections;
using UnityEngine;

public class BossZombie : Enemy {

    private Rigidbody2D rb;

    private Animator anim;

    public float chaseSpeed;

    public Transform playerTransform;

    public float distToPlayer;

    public Transform placePoint;

    private EnemyWeapon weapon;

    public WeaponHolder holder;

    public float startHealth;

    public float chaseRange;

    private Vector2 randomSpot;

    public float offset;

    public float attackRange;

    public float health;

    public float startTimeBtwAttack;

    public GameObject projectile;

    public float timeBtwAttack;

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        randomSpot = SetRandomPos(randomSpot, offset);
        anim = this.GetComponent<Animator>();
        weapon = holder.spawnedWeapon;
        timeBtwAttack = startTimeBtwAttack;
        health = startHealth / 2;
    }

    private void Update() {
        distToPlayer = Vector2.Distance(rb.position, playerTransform.position);
        weapon.orgPos = placePoint.transform.position;
        RotateWeapon(weapon, playerTransform);
        ConfigureType();
        HandleType();
    }

    private void ConfigureType() {
        if (health > startHealth / 2) {
            enemyType = EnemyType.Slasher;
        }
        if (health <= startHealth / 2) {
            enemyType = EnemyType.Spitter;
        }
    }

    void HandleType() {
        if (enemyType == EnemyType.Slasher) {
            weapon.gameObject.SetActive(true);
            Slasher();
        }
        if (enemyType == EnemyType.Spitter) {
            weapon.gameObject.SetActive(false);
            Spitter();
        }
    }

    public void Slasher() {
        if (distToPlayer > chaseRange) {
            Idle();
        }
        if (!changeState) {
            if (distToPlayer <= chaseRange && distToPlayer >= attackRange) {
                Chase();
            }
        }


        if (changeState) {
            if (distToPlayer >= attackRange) {
                Chase();
            }
        }

        if (distToPlayer <= attackRange) {
            Attack();
        }
    }

    public void Spitter() {
        if (distToPlayer > chaseRange) {
            Idle();
        }

        if (!changeState) {
            if (distToPlayer <= chaseRange && distToPlayer >= attackRange) {
                Chase();
            }
        }


        if (changeState) {
            if (distToPlayer >= attackRange) {
                Chase();
            }
        }

        if (distToPlayer <= attackRange) {
            Attack();
        }
    }

    void Idle() {
        rb.position = transform.position;
        anim.SetBool("Walk", false);
    }

    void Chase() {
        rb.position = Vector2.MoveTowards(rb.position, playerTransform.position, chaseSpeed * Time.deltaTime);
        if(enemyType == EnemyType.Slasher) {
            if(Random.value < 0.5f) {
                if(timeBtwAttack <= 0) {
                    StartCoroutine(weapon.Attack(playerTransform.position));
                    timeBtwAttack = startTimeBtwAttack;
                } else {
                    timeBtwAttack -= Time.deltaTime;
                }
            }
        }

        if(enemyType == EnemyType.Spitter) {
            if(Random.value < 0.5f) {
                if(timeBtwAttack <= 0) {
                    Instantiate(projectile, transform.position, Quaternion.identity);
                    timeBtwAttack = startTimeBtwAttack;
                } else {
                    timeBtwAttack -= Time.deltaTime;
                }
            }
        }
    }

    void Attack() {
        changeState = false;
        if(enemyType == EnemyType.Slasher) {
            if(timeBtwAttack <= 0) {
                StartCoroutine(weapon.Attack(playerTransform.position));
                timeBtwAttack = startTimeBtwAttack;
            } else {
                timeBtwAttack -= Time.deltaTime;
            }
        }

        if(enemyType == EnemyType.Spitter) {
             if(timeBtwAttack <= 0) {
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBtwAttack = startTimeBtwAttack;
            } else {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    Vector2 SetRandomPos(Vector2 _pos, float _offset) {
        float x = Random.Range(rb.position.x - _offset, rb.position.x + _offset);
        float y = Random.Range(rb.position.y - _offset, rb.position.y + -_offset);
        _pos = new Vector2(x, y);
        return _pos;
    }

    public void RotateWeapon(EnemyWeapon _weapon, Transform targetPos) {
        Vector2 lookDir = _weapon.rb.position - (Vector2)targetPos.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90;
        _weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        DoDamage(collision);
    }

}