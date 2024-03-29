using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed = 10f; // Forward speed of the car
    public float rotationSpeed = 100f; // Rotation speed of the car

    void Update()
    {
        // Get player input for horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate rotation amount based on input
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        // Rotate the car
        transform.Rotate(0, rotationAmount, 0);

        // Move the car forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
