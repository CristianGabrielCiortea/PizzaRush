using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MovementScript : MonoBehaviour
{
    public float speed = 10f; // Forward speed of the car
    public float rotationSpeed = 100f; // Rotation speed of the car

    void Start()
    {
        // Disable movement script until camera animation finishes
        enabled = false;

        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(2);
        transform.GetChild(2).DORotate(new Vector3(18f, 270f, 0f), 2f).SetEase(Ease.OutQuad);
        transform.GetChild(2).DOMoveX(41, 2f).SetEase(Ease.OutQuad).OnComplete(() => EnableMovement());
    }

    void EnableMovement()
    {
        // Enable user input for movement
        enabled = true;
    }

    void Update()
    {
        // Only allow movement if script is enabled
        if (!enabled)
            return;

        // Get player input for horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate rotation amount based on input
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        Debug.Log(rotationAmount);
        Debug.Log(transform.rotation.y);

        if ((transform.rotation.y > -0.4 && rotationAmount > 0) ||
            (transform.rotation.y < -0.9 && rotationAmount < 0))
        {
            rotationAmount = 0;
        }

        transform.Rotate(0, rotationAmount, 0);

        // Move the car forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
