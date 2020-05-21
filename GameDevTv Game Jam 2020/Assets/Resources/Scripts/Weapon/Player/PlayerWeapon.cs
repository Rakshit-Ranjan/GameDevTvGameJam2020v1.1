using System.Collections;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public float startTimeBtwAttack;

    public WeaponAttackType attackType;

    private float timeBtwAttack;

    private Enemy enemy;

    private Rigidbody2D rb;

    public WeaponType weaponType;

    public float damage;

    public float attackSpeed;

    private Vector2 mousePos;

    public Transform placePoint;

    public PlayerMovement player;

    public bool canAttack;

    public Vector2 orgPos;

    public bool hurt;

    private void Start() {
        canAttack = false;
        rb = this.GetComponent<Rigidbody2D>();
        player = GetComponentInParent<PlayerMovement>();
        timeBtwAttack = startTimeBtwAttack;
    }

    private void Update() {
        orgPos = placePoint.position;
        Rotate();

        if (timeBtwAttack <= 0) {
            if (Input.GetMouseButtonDown(0)) {
                if (attackType == WeaponAttackType.Slash)
                    StartCoroutine(SlashAttack());
                if (attackType == WeaponAttackType.Spin) {
                    StartCoroutine(SpinAttack(2f));
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        } else {
            timeBtwAttack -= Time.deltaTime;
        }

    }

    public void Rotate() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (Vector2)transform.position - mousePos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public IEnumerator SlashAttack() {
        FindObjectOfType<AudioManager>().Play("PlayerSwordSlash");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        Vector2 targetPos = hit.point;

        float percent = 0;

        while (percent <= 1) {
            percent += Time.deltaTime * attackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(orgPos, targetPos, formula);
            yield return null;
        }
        if(hurt) {
            if (enemy != null) {
                if (!enemy.isAttacking) {
                    Health health = enemy.GetComponent<Health>();
                    FindObjectOfType<AudioManager>().Play("Hurt");
                    health.TakeDamage(damage);
                    hurt = false;
                    enemy = null;
                }
            }
        }
    }

    public IEnumerator SpinAttack(float duration) {
        FindObjectOfType<AudioManager>().Play("PlayerAxeSlash");
        float timeBtwFrames = duration / 360;
        Vector3 org = transform.localEulerAngles;
        for (int i = 360; i >= 1; i -= 45) {
            org.z = i;
            transform.localEulerAngles = org;
            yield return new WaitForSeconds(timeBtwFrames);
        }
        if (hurt) {
            if (enemy != null) {
                if (!enemy.isAttacking) {
                    Health health = enemy.GetComponent<Health>();
                    health.TakeDamage(damage);
                    FindObjectOfType<AudioManager>().Play("Hurt");
                    hurt = false;
                    enemy = null;
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        Enemy _enemy = collision.GetComponent<Enemy>();

        if(_enemy != null) {
            enemy = _enemy;
            enemy.changeState = true;
            hurt = true;
        }
    }

}
