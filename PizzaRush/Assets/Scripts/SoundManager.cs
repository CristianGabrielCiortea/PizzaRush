using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
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
        _audioSource.clip = Resources.Load<AudioClip>($"Sounds/{clipSound}");
        _audioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
