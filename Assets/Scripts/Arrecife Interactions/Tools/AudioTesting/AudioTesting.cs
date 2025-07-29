using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    public static AudioTesting Instance { get; private set; }
    AudioSource audioMain;

    [SerializeField] AudioClip[] clip;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioMain = GetComponent<AudioSource>();
    }

    public void InitializeClip(int i)
    {
        audioMain.Stop();
        audioMain.clip = clip[i];
        audioMain.Play();
    }
}
