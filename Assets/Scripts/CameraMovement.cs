using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 movement = new Vector3 (1, 0, 0);
    [SerializeField] private Vector2 rotation = Vector2.zero;
    [SerializeField] private float speed = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        transform.eulerAngles = (Vector2)rotation * speed;
        transform.position += movement * Time.deltaTime;
    }
}
