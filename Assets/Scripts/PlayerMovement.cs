using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 0.5f;
    [SerializeField] private float detectionDistance = 5f;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        // Move forward constantly
        transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);

        // Check for obstacles ahead
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance)) {
            // Apply upward force to jump
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    // Optional: Visualize the detection ray in the editor
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionDistance);
    }
}
