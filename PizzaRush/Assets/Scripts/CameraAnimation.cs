using UnityEngine;
using DG.Tweening;

public class CameraAnimation : MonoBehaviour
{
    public Vector3 targetRotation = new Vector3(0f, 180f, 0f);
    public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    public float duration = 2f;

    void Start()
    {
        //transform.DORotate(targetRotation, duration).SetEase(Ease.OutQuad);
        //transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);
    }
}
