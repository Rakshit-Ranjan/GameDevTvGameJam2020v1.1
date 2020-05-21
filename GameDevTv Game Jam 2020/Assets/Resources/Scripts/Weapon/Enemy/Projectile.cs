using UnityEngine;

public class Projectile : MonoBehaviour {

    public float radius = 3f;

    public Transform playerTransform;

    public float speed; 

    private Rigidbody2D rb;

    private Vector2 target;
    
    public ParticleSystem bloodEffect;

    public ParticleSystem explosion;

    public bool canHurt;            

    public RaycastHit2D[] hits;

    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        target = playerTransform.position;
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.position = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
        if (rb.position == target) {
            canHurt = true;
            Destroy(gameObject, 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("WallMap") || collision.gameObject.CompareTag("Player")) {
            canHurt = false;
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        if (canHurt) {
            Collider2D[] hits = Physics2D.OverlapCircleAll(rb.position, radius);
            foreach (Collider2D hit in hits) {
                if (hit.CompareTag("Player")) {
                    print("Hit Player");
                    Instantiate(bloodEffect, transform.position, Quaternion.identity);
                }
            }
        } else {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
