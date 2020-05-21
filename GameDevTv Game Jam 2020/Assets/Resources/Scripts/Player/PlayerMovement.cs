using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;

    public float speed;

    public Vector2 moveDir;

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        HandleInput();
    }

    private void FixedUpdate() {
        Move();
    }

    private void HandleInput() {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDir = moveDir.normalized;
    }

    private void Move() {
        rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);
    }

}
