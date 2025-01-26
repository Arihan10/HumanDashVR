using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Tooltip("Player object")]
    [SerializeField] private GameObject player;
    [Tooltip("Array of spawn points for the game")]
    [SerializeField] private GameObject[] spawnPoints;
    [Tooltip("Distance from the player within which the cats start shooting")]
    [SerializeField] private float activationDistance = 50f;
    [Tooltip("Cooldown before the next projectile is spawned")]
    [SerializeField] private float coolDown = 5f;
    [Tooltip("Object to use as a projectile")]
    [SerializeField] private GameObject projectile;
    [Tooltip("Force applied to the projectile towards the player")]
    [SerializeField] private float projectileForce = 10f;

    private double timeFromLastProjectile = 0f;

    void Update()
    {
        timeFromLastProjectile += Time.deltaTime;

        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (Vector3.Distance(spawnPoint.transform.position, player.transform.position) <= activationDistance &&
                timeFromLastProjectile >= coolDown &&
                player.transform.position.z <= spawnPoint.transform.position.z)
            {
                timeFromLastProjectile = 0f;
                GameObject projectileInstance = Instantiate(projectile, spawnPoint.transform.position, Quaternion.identity);

                // Apply force towards player
                Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = (player.transform.position - spawnPoint.transform.position).normalized;
                    rb.AddForce(direction * projectileForce, ForceMode.Impulse);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (GameObject spawnPoint in spawnPoints)
        {
            Gizmos.DrawSphere(spawnPoint.transform.position, activationDistance);
        }
    }
}