using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossOgre : Enemy {

    private Rigidbody2D rb;

    private Animator anim;

    public float startHealth;

    public float health;

    public float chaseSpeed;

    private Vector2 patrolPoint;

    public float startTimeBtwAttack;

    private float timeBtwAttack;

    public float distToPlayer;

    public EnemyWeapon weapon;

    public WeaponHolder holder;

    public Transform playerTransform;

    public Transform placePoint;

    public float chaseDistance;

    public float offset;

    public float attackDistance;

    public float timer = 1f;

    public bool isOgre;

    private void Start() {
        weapon = holder.spawnedWeapon;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        health = startHealth;
        timeBtwAttack = startTimeBtwAttack;
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        patrolPoint = SetRandomPosition(patrolPoint, offset);
    }

    private void Update() {
        ConfigureType();
        HandleType();
        HandleState();
    }

    private void ConfigureType() {
        if (isOgre) {
            RotateWeapon(weapon, playerTransform);
            weapon.orgPos = placePoint.transform.position;
            if (health > startHealth / 2) {
                weapon.gameObject.SetActive(false);
                enemyType = EnemyType.Puncher;
            }
            if (health <= startHealth / 2) {
                weapon.gameObject.SetActive(true);
                enemyType = EnemyType.Slasher;
            }
        }
    }

    private void HandleType() {
        if (enemyType == EnemyType.Puncher) {
            Puncher();
        }
        if (enemyType == EnemyType.Slasher) {
            Slasher();
        }
    }

    private void Puncher() {
        if (playerTransform != null)
            distToPlayer = Vector2.Distance(rb.position, playerTransform.position);

        if (distToPlayer > chaseDistance) {
            enemyState = EnemyState.Patroll;
        }
        if (changeState) {
            if (distToPlayer <= chaseDistance && distToPlayer > attackDistance) {
                enemyState = EnemyState.Chase;
            }
        }


        if (changeState) {
            if (distToPlayer > attackDistance) {
                enemyState = EnemyState.Chase;
            }
        }

        if (distToPlayer <= attackDistance) {
            enemyState = EnemyState.Attack;
        }

    }

    private void Slasher() {
        if (playerTransform != null)
            distToPlayer = Vector2.Distance(rb.position, playerTransform.position);

        if (distToPlayer > chaseDistance) {
            enemyState = EnemyState.Patroll;
        }
        if (!changeState) {
            if (distToPlayer <= chaseDistance && distToPlayer > attackDistance) {
                enemyState = EnemyState.Chase;
            }
        }


        if (changeState) {
            enemyState = EnemyState.Chase;
        }

        if (distToPlayer <= attackDistance) {
            enemyState = EnemyState.Attack;
        }
    }

    private void HandleState() {
        if (enemyState == EnemyState.Patroll) {
            if (rb.position != patrolPoint) {
                rb.position = Vector2.MoveTowards(rb.position, patrolPoint, chaseSpeed * Time.deltaTime);
                anim.SetBool("Walk", true);
            } else {
                patrolPoint = SetRandomPosition(patrolPoint, offset);
            }
        }

        if (enemyState == EnemyState.Chase) {
            if (enemyType == EnemyType.Puncher) {
                anim.SetBool("Walk", true);
                rb.position = Vector2.MoveTowards(rb.position, playerTransform.position, chaseSpeed * Time.deltaTime);
            }
            if (enemyType == EnemyType.Slasher) {
                anim.SetBool("Walk", true);
                rb.position = Vector2.MoveTowards(rb.position, playerTransform.position, chaseSpeed * Time.deltaTime);
                if (timer <= 0) {
                    if (Random.value < .5f) {
                        StartCoroutine(weapon.Attack(playerTransform.position));
                    }
                    timer = 1f;
                } else {
                    timer -= Time.deltaTime;
                }
            }
        }

        if (enemyState == EnemyState.Attack) {
            changeState = false;
            rb.position = transform.position;
            if (enemyType == EnemyType.Puncher) {
                anim.SetBool("Walk", false);
                if (timeBtwAttack <= 0) {
                    StartCoroutine(Attack());
                    timeBtwAttack = startTimeBtwAttack;
                } else {
                    timeBtwAttack -= Time.deltaTime;
                }
            }

            if (enemyType == EnemyType.Slasher) {
                anim.SetBool("Walk", false);
                if (timeBtwAttack <= 0) {
                    StartCoroutine(weapon.Attack(playerTransform.position));
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

    public void RotateWeapon(EnemyWeapon _weapon, Transform targetPos) {
        Vector2 lookDir = _weapon.rb.position - (Vector2)targetPos.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90;
        _weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public IEnumerator Attack() {
        Vector2 orgPos = rb.position;
        Vector2 targetPos = playerTransform.position;
        float percent = 0;

        while (percent <= 1) {
            percent += Time.deltaTime * 4f;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(orgPos, targetPos, formula);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (enemyState == EnemyState.Patroll) {
            if (other.gameObject.CompareTag("WallMap")) {
                patrolPoint = SetRandomPosition(patrolPoint, offset);
            }
        }
        DoDamage(other); 
    }
}
