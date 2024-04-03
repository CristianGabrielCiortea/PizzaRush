using UnityEngine;

public class ObstacleCarMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float distanceThreshold = 10f; 
    private Vector3 moveDirection = Vector3.forward; 

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        float distanceFromStart = Vector3.Distance(transform.position, startPosition);

        if (distanceFromStart >= distanceThreshold || distanceFromStart == 0)
        {
            transform.Rotate(Vector3.up, 180f);
        }
    }
}
