using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MovementScript : MonoBehaviour
{
    public Canvas mainMenu;
    public Canvas gameUI;
    public Canvas settingsMenu;
    public float speed = 10f; // Forward speed of the car
    public float rotationSpeed = 100f; // Rotation speed of the car
    private bool isPlaying = false;
    public TrailRenderer[] tireMarks;
    private bool tireMarksFlag = false;
    private float initialSpeed;

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
        if (settingsMenu.gameObject.activeSelf)
        {
            return;
        }

        if (!isPlaying)
        {
            initialSpeed = speed;
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
            emitTrailMarks();
            if (speed > 5f)
                speed -= 0.025f;
        }
        else
        {
            stopTrailMarks();
            if (speed < initialSpeed)
                speed += 0.05f;
        }

        // Move the car forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void emitTrailMarks()
    {
        if (tireMarksFlag) return;
        foreach (TrailRenderer trail in tireMarks)
        {
            trail.emitting = true;
        }

        tireMarksFlag = true;
    }

    private void stopTrailMarks()
    {
        if (!tireMarksFlag) return;
        foreach (TrailRenderer trail in tireMarks)
        {
            trail.emitting = false;
        }

        tireMarksFlag = false;
    }
}
