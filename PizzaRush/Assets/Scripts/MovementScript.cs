using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MovementScript : MonoBehaviour
{
    public Canvas mainMenu;
    public Canvas gameUI;
    public float speed = 10f; // Forward speed of the car
    public float rotationSpeed = 100f; // Rotation speed of the car
    private bool isPlaying = false;

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(1);
        transform.GetChild(2).DORotate(new Vector3(18f, 270f, 0f), 2f).SetEase(Ease.OutQuad);
        transform.GetChild(2).DOMoveX(41, 2f).SetEase(Ease.OutQuad).OnComplete(() => EnableMovement());
    }

    void EnableMovement()
    {
        gameUI.gameObject.SetActive(true);
        isPlaying = true;
        enabled = true;
    }

    void Update()
    {
        if (!isPlaying)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                enabled = false;
                mainMenu.gameObject.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() => mainMenu.gameObject.SetActive(false));
                StartCoroutine(PlayAnimation());
            }
            return;
        }

        // Get player input for horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate rotation amount based on input
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        if ((transform.rotation.y > -0.4 && rotationAmount > 0) ||
            (transform.rotation.y < -0.9 && rotationAmount < 0))
        {
            rotationAmount = 0;
        }

        transform.Rotate(0, rotationAmount, 0);

        if (Input.GetKey(KeyCode.Space))
        {
            speed -= 0.025f;
            if (speed < 0) speed = 0;
        }
        else
        {
            speed += 0.05f;
            if (speed >= 10f) speed = 10f;
        }

        // Move the car forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
