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
        switch(collision.gameObject.tag)
        {
            case "Obstacle":
                _animator.SetTrigger("TriggerCollision");
                _particleSystem.Play();
                StartCoroutine(SetTriggerAfterDelay("UntriggerCollision", 1f));
                Destroy(collision.gameObject);
                break;
            case "Finish":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
