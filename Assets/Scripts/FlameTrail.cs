using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlameTrail : MonoBehaviour
{
    [Tooltip("Reference to the particle system's transform")]
    public Transform particleTransform;

    [Tooltip("Rotation offset in Euler angles (applied after direction calculation)")]
    [SerializeField] private Vector3 _rotationOffset = Vector3.zero;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (particleTransform == null)
        {
            ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
            if (ps != null) particleTransform = ps.transform;
            else Debug.LogError("No particle system found in children!", this);
        }
    }

    void Update()
    {
        if (particleTransform == null || _rb == null) return;

        if (_rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            Vector3 oppositeDirection = -_rb.linearVelocity.normalized;

            // Calculate base rotation from velocity direction
            Quaternion baseRotation = Quaternion.LookRotation(oppositeDirection);

            // Apply rotation offset
            Quaternion finalRotation = baseRotation * Quaternion.Euler(_rotationOffset);

            particleTransform.rotation = finalRotation;
        }
    }

    // Optional: Editor validation to help visualize the offset
#if UNITY_EDITOR
    void OnValidate()
    {
        if (particleTransform != null && _rb != null)
        {
            Vector3 oppositeDirection = -_rb.linearVelocity.normalized;
            Quaternion baseRotation = Quaternion.LookRotation(oppositeDirection);
            particleTransform.rotation = baseRotation * Quaternion.Euler(_rotationOffset);
        }
    }
#endif
}