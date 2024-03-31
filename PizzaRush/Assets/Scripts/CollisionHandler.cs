using System.Collections;
using UnityEngine;

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
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            _animator.SetTrigger("TriggerCollision");
            _particleSystem.Play();
            StartCoroutine(SetTriggerAfterDelay("UntriggerCollision", 1f));
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator SetTriggerAfterDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.SetTrigger(triggerName);
    }
}
