using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField]
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Awake()
    {

    }
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    public void PlayClip(string clipSound)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sounds/{clipSound}");
        if (clip != null)
        {
            // Stop any currently playing clip
            _audioSource.Stop();

            // Assign the new clip and play it
            _audioSource.clip = clip;
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip '{clipSound}' not found in Resources/Sounds folder.");
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
