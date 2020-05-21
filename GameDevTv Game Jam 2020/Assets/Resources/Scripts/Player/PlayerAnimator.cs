using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private PlayerMovement pm;
    private Animator anim;

    private void Start() {
        pm = this.GetComponentInParent<PlayerMovement>();
        anim = this.GetComponent<Animator>();
    }

    private void Update() {
        UpdateAnimator();
    }

    private void UpdateAnimator() {
        anim.SetFloat("Speed", pm.moveDir.magnitude);
        anim.SetFloat("Horizontal", pm.moveDir.x);
        anim.SetFloat("Vertical", pm.moveDir.y);
    }

}
