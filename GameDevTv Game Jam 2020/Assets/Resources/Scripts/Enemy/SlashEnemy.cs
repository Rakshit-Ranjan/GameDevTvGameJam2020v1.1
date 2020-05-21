using UnityEngine;

public class SlashEnemy : Enemy {

    private Animator anim;

    private Rigidbody2D rb;

    private Transform playerTransfom;

    private float distToPlayer;

    public float chaseRange;

    public float attackRange;

    public float chaseSpeed;

    public EnemyWeapon weapon;

    public Transform placePoint;

    private float startTimeBtwAttack;

    private float timeBtwAttack;

    private Vector2 patrolPoint;

    public float offset;

    public WeaponHolder holder;

    private void Start() {
        holder = this.GetComponentInChildren<WeaponHolder>();
        playerTransfom = GameObject.FindGameObjectWithTag("Player").transform;
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        weapon = holder.spawnedWeapon;
        startTimeBtwAttack = weapon.attackRate;
        timeBtwAttack = startTimeBtwAttack;
        float x = Random.Range(transform.position.x - offset, transform.position.x + offset);
        float y = Random.Range(transform.position.y - offset, transform.position.y + offset);
        patrolPoint = new Vector2(x, y);
    }

    private void Update() {
        if (playerTransfom != null)
            distToPlayer = Vector2.Distance(rb.position, playerTransfom.position);
        ConfigureState();
    }

    private void FixedUpdate() {
        HandleState();
        weapon.orgPos = placePoint.transform.position;
        weapon.Rotate(playerTransfom);
    }

    private void ConfigureState() {
        if (distToPlayer >= chaseRange) {
            enemyState = EnemyState.Idle;
        }

        if (!changeState) {
            if (distToPlayer < chaseRange && distToPlayer >= attackRange) {
                enemyState = EnemyState.Chase;
            }
        }

        if (distToPlayer < attackRange) {
            enemyState = EnemyState.Attack;
        }

        if (changeState) {
            if (distToPlayer >= attackRange) {
                enemyState = EnemyState.Chase;
            }
        }
    }

    private void HandleState() {
        if (enemyState == EnemyState.Idle) {
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
            rb.position = Vector2.MoveTowards(rb.position, playerTransfom.position, chaseSpeed * Time.fixedDeltaTime);
        }

        if (enemyState == EnemyState.Attack) {
            changeState = false;
            anim.SetBool("Walk", false);
            if (timeBtwAttack <= 0) {
                StartCoroutine(weapon.Attack(playerTransfom.position));
                timeBtwAttack = startTimeBtwAttack;
            } else {
                timeBtwAttack -= Time.deltaTime;
            }
        }
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
