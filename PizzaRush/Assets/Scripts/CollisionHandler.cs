using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CollisionHandler : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    public Sprite[] _images;
    [SerializeField]
    public Image _healthComponent;
    private int _health = 4;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _healthComponent.sprite = _images[_health - 1];
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                _health--;
                _healthComponent.DOFade(0, 0.25f).OnComplete(() => {
                    _healthComponent.sprite = _images[_health - 1];
                    _healthComponent.DOFade(1, 0.25f);
                });

                if (_health == 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                }

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
