using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform playerTransform;

    void LateUpdate() {
        Vector3 org = transform.position;
        org.x = playerTransform.position.x;
        org.y = playerTransform.position.y;
        transform.position = org;
    }
}
