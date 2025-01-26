using UnityEngine;
public class LevelMovement : MonoBehaviour {
    float moveSpeed = -3f;

    void Update() {
        Vector3 movement = new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;

        // Move the parent
        transform.Translate(movement);

        // If needed, explicitly move all children
        foreach (Transform child in transform) {
            child.Translate(movement);
        }
    }
}