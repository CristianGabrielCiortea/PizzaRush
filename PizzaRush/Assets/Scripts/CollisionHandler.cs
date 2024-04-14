using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    public Sprite[] _images;
    [SerializeField]
    public Image _healthComponent;
    [SerializeField]
    public Canvas finishCanvas;
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
                if (movementScript.speed > 10f)
                {
                    DecreaseHealth();
                    if (_health <= 0)
                    {
                        GameOver();
                        return;
                    }
                }
                if(movementScript.speed > 8f)
                {
                    movementScript.speed -= 0.25f;
                }
                break;
            case "GroupObstacle":
                Destroy(collision.gameObject);
                foreach (var cone in cones)
                {
                    Destroy(cone);
                }
                DecreaseHealth();
                if (_health <= 0)
                {
                    GameOver();
                    return;
                }
                _animator.SetTrigger("TriggerCollision");
                SoundManager.instance.PlayClip("collide");
                _particleSystem.Play();
                StartCoroutine(SetTriggerAfterDelay("UntriggerCollision", 1f));
                break;
            case "Obstacle":
                Destroy(collision.gameObject);
                DecreaseHealth();
                if (_health <= 0)
                {
                    GameOver();
                    return;
                }
                if (_animator)
                {
                    _animator.SetTrigger("TriggerCollision");
                    SoundManager.instance.PlayClip("collide");
                    _particleSystem.Play();
                    StartCoroutine(SetTriggerAfterDelay("UntriggerCollision", 1f));
                }
                break;
            case "Finish":
                StartCoroutine(FinishWithAnimation());
                break;
            case "Map":
                break;
            default:
                MakeHealthZero();
                GameOver();
                break;
        }
    }

    private void GameOver()
    {
        SoundManager.instance.PlayClip("gameover");
        Destroy(_animator);
        gameObject.GetComponent<MovementScript>().enabled = false;

        transform.GetChild(0).DORotate(new Vector3(0f, 0f, 90f), 2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
        transform.GetChild(0).DOMoveY(1.1f, 2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
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

    private void MakeHealthZero()
    {
        _healthComponent.DOFade(0, 0.25f).OnComplete(() =>
        {
            _healthComponent.sprite = _images[-1];
            _healthComponent.DOFade(1, 0.25f);
        });
    }

    private IEnumerator SetTriggerAfterDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.SetTrigger(triggerName);
    }

    private IEnumerator FinishWithAnimation()
    {
        GetComponent<MovementScript>().enabled = false;
        finishCanvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        float duration = 3.5f;
        float rotations = 3;
        float totalRotation = 360f * rotations;
        float startTime = Time.time;
        Vector3 axis = Vector3.up;
        while (Time.time < startTime + duration)
        {
            transform.GetChild(1).RotateAround(transform.position, axis, totalRotation / duration * Time.deltaTime);
            yield return null;
        }

        finishCanvas.gameObject.SetActive(false);

        int newSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (newSceneIndex > 2)
        {
            newSceneIndex = 0;
        }

        SceneManager.LoadScene(newSceneIndex);
    }
}
