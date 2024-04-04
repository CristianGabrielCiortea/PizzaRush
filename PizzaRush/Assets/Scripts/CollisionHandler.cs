using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private ParticleSystem _particleSystem;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                _animator.SetTrigger("TriggerCollision");
                _particleSystem.Play();
                StartCoroutine(SetTriggerAfterDelay("UntriggerCollision", 1f));
                Destroy(collision.gameObject);
                break;
            case "Finish":
                StartCoroutine(ReloadWithAnimation());
                break;
            case "Map":
                break;
            default:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }

    private IEnumerator SetTriggerAfterDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.SetTrigger(triggerName);
    }

    void PlayAnimation()
    {
        Transform childTransform = transform.GetChild(2);
        Vector3 targetRotation = childTransform.localEulerAngles + new Vector3(0f, 360f, 0f); 

        childTransform.DORotate(targetRotation, 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear);

    }

    IEnumerator ReloadWithAnimation()
    {
        PlayAnimation();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
