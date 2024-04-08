using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
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
                StartCoroutine(FinishWithAnimation());
                break;
            case "Map":
                break;
            default:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
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
            transform.GetChild(2).RotateAround(transform.position, axis, totalRotation / duration * Time.deltaTime);
            yield return null;
        }

        finishCanvas.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
