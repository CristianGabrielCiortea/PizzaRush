using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    private Animator _animator;
    public ParticleSystem _particleSystem;
    public Sprite[] _images;
    public Image _healthComponent;
    private int _health = 4;
    private GameObject[] cones;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _healthComponent.sprite = _images[_health - 1];
        cones = GameObject.FindGameObjectsWithTag("GroupObstacle");
    }

    private void OnCollisionEnter(Collision collision)
    {
        var movementScript = GetComponent<MovementScript>();
        switch (collision.gameObject.tag)
        {
            case "SpeedBump":
                if (movementScript.speed > 8.5f)
                {
                    DecreaseHealth();
                    if (_health == 0)
                    {
                        SoundManager.instance.PlayClip("gameover");
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        return;
                    }
                }
                if(movementScript.speed > 5.25f)
                {
                    movementScript.speed -= 0.25f;
                }
                break;
            case "GroupObstacle":
            case "Obstacle":
                DecreaseHealth();
                if (_health == 0)
                {
                    SoundManager.instance.PlayClip("gameover");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    return;
                }
                _animator.SetTrigger("TriggerCollision");
                SoundManager.instance.PlayClip("collide");
                _particleSystem.Play();
                StartCoroutine(SetTriggerAfterDelay("UntriggerCollision", 1f));
                if (collision.gameObject.CompareTag("GroupObstacle"))
                {
                    foreach (var cone in cones)
                    {
                        Destroy(cone);
                    }
                }
                else
                {
                    Destroy(collision.gameObject);
                }
                break;
            case "Finish":
                gameObject.GetComponent<MovementScript>().isPlaying = false;

                // Finish animation
                transform.GetChild(0).DORotate(new Vector3(0f, 180f, 0f), 2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
                transform.GetChild(0).DOMoveY(1.1f, 2f).SetEase(Ease.OutQuad).OnComplete(() =>
                    transform.GetChild(0).DOMoveY(0.1f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                        transform.GetChild(0).DOMove(transform.GetChild(0).position + (transform.GetChild(0).forward * 10), 1f).
                            SetEase(Ease.OutQuad).OnComplete(() => LoadNextScene()
                        )
                    )
                );
                break;
            case "Map":
                break;
            default:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void DecreaseHealth()
    {
        _health--;
        _healthComponent.DOFade(0, 0.25f).OnComplete(() =>
        {
            _healthComponent.sprite = _images[_health - 1];
            _healthComponent.DOFade(1, 0.25f);
        });
    }

    private IEnumerator SetTriggerAfterDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.SetTrigger(triggerName);
    }
}
