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
                        break;
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
                SoundManager.instance.PlayClip("collide");
                if (_health == 0)
                {
                    SoundManager.instance.PlayClip("gameover");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                }
                _animator.SetTrigger("TriggerCollision");
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
