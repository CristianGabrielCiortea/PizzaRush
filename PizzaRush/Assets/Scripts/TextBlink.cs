using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class TextBlink : MonoBehaviour
{
    public GameObject textObject; // Text object to blink
    public float blinkInterval = 0.5f; // Time interval between blinks
    private bool isVisible = true;

    void Start()
    {
        StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            isVisible = !isVisible;
            textObject.GetComponent<TextMeshProUGUI>().DOFade(isVisible ? 1 : 0, blinkInterval);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
