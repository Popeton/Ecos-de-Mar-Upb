using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class ReplayButton : MonoBehaviour
{
    [SerializeField] VideoPlayer vp;
    [SerializeField] float videoDuration;
    [SerializeField] GameObject replayButton, exitButton;

    AudioManager audioMana;

    private void Start()
    {
        audioMana=FullCoralFlow.Instance.audioMana;
    }
    public void ReplayVideo()
    {
        StartCoroutine(ReplayVid());
    }

    IEnumerator ReplayVid()
    {
        //audio video esporas
        audioMana.CleanUp();
        ArrecifeSubtitles.Instance.EsporasVideo();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion2_Fase1, audioMana.transform.position);
        //inicia audio efecto de video
        audioMana.PlayOneShot(FmodEvents.instance.Sound2);
        //AudioTesting.Instance.InitializeClip(5);

        replayButton.SetActive(false);
        exitButton.SetActive(false);
        vp.Stop();
        vp.Play();
        yield return new WaitForSeconds(videoDuration);
        replayButton.SetActive(true);
        exitButton.SetActive(true);
    }

    //testing
    private void OnDisable()
    {
        ReplayVideo();
    }
}
