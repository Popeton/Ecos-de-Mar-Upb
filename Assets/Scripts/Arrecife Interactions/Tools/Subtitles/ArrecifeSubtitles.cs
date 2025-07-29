using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrecifeSubtitles : MonoBehaviour
{
    public static ArrecifeSubtitles Instance { get; private set; }

    [SerializeField] SceneSubtitles subs;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] CanvasGroup canvasG;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Intro()
    {
        StartCoroutine(IntroSubs());
    }
    IEnumerator IntroSubs()
    {
        text.text = subs.subtitle[0];
        yield return new WaitForSeconds(5.6f);
        text.text = subs.subtitle[1];
        yield return new WaitForSeconds(3.5f);
        text.text = subs.subtitle[2];
        yield return new WaitForSeconds(6.3f);
        text.text = subs.subtitle[3];
        yield return new WaitForSeconds(3f);
        text.text = subs.subtitle[4];
        yield return new WaitForSeconds(2.4f);
        text.text = "";
    }

    //esto es para mostrar un solo subtitulo
    public void SingleSubtitle(int subtitle,float delay)
    {
        StartCoroutine(SingleSub(subtitle,delay));
    }

    IEnumerator SingleSub(int subtitle, float delay)
    {
        text.text = subs.subtitle[subtitle];
        yield return new WaitForSeconds(delay);
        text.text = "";
    }
    //==================================================================

    public void TercerCoral()
    {
        StartCoroutine(TercerCoralSubs());
    }

    IEnumerator TercerCoralSubs()
    {
        text.text = subs.subtitle[6];
        yield return new WaitForSeconds(5.1f);
        text.text = subs.subtitle[7];
        yield return new WaitForSeconds(2.4f);
        text.text = "";
    }

    public void EsporasIntro()
    {
        StartCoroutine (EsporasIntroSubs());
    }

    IEnumerator EsporasIntroSubs() 
    {
        text.text = subs.subtitle[8];
        yield return new WaitForSeconds(5.6f);
        text.text = subs.subtitle[9];
        yield return new WaitForSeconds(6f);
        text.text = subs.subtitle[10];
        yield return new WaitForSeconds(4f);
        text.text = subs.subtitle[11];
        yield return new WaitForSeconds(4f);
        text.text = "";
    }


    public void EsporasVideo()
    {
        StartCoroutine(EsporasVideoSubs());
    }

    IEnumerator EsporasVideoSubs()
    {
        text.text = subs.subtitle[12];
        yield return new WaitForSeconds(4.5f);
        text.text = subs.subtitle[13];
        yield return new WaitForSeconds(3.8f);
        text.text = subs.subtitle[14];
        yield return new WaitForSeconds(2f);
        text.text = subs.subtitle[15];
        yield return new WaitForSeconds(5.5f);
        text.text = subs.subtitle[16];
        yield return new WaitForSeconds(3.3f);
        text.text = subs.subtitle[17];
        yield return new WaitForSeconds(6f);
        text.text = "";
    }
    public void IntroPeces()
    {
        StartCoroutine (IntroPecesSubs());
    }

    IEnumerator IntroPecesSubs()
    {
        text.text = subs.subtitle[18];
        yield return new WaitForSeconds(5.7f);
        text.text = subs.subtitle[19];
        yield return new WaitForSeconds(4.5f);
        text.text = subs.subtitle[20];
        yield return new WaitForSeconds(3.8f);
        text.text = subs.subtitle[21];
        yield return new WaitForSeconds(3.4f);
        text.text = "";
    }

    public void LoraComer()
    {
        StartCoroutine(LoraComerSubs());
    }
    IEnumerator LoraComerSubs()
    {
        text.text = subs.subtitle[24];
        yield return new WaitForSeconds(3.7f);
        text.text = subs.subtitle[25];
        yield return new WaitForSeconds(7f);
        text.text = "";
    }

    public void LoraDigestion()
    {
        StartCoroutine(LoraDigestionSubs());
    }

    IEnumerator LoraDigestionSubs()
    {
        text.text = subs.subtitle[26];
        yield return new WaitForSeconds(6.9f);
        text.text = subs.subtitle[27];
        yield return new WaitForSeconds(7f);
        text.text = "";
    }

    public void Webs()
    {
        StartCoroutine (WebsSubs());
    }

    IEnumerator WebsSubs()
    {
        text.text = subs.subtitle[28];
        yield return new WaitForSeconds(5.2f);
        text.text = subs.subtitle[29];
        yield return new WaitForSeconds(4.7f);
        text.text = subs.subtitle[30];
        yield return new WaitForSeconds(2.2f);
        text.text = "";
    }

    public void Microplastic()
    {
        StartCoroutine(MicorplasticSubs());
    }

    IEnumerator MicorplasticSubs()
    {
        text.text = subs.subtitle[32];
        yield return new WaitForSeconds(5.1f);
        text.text = subs.subtitle[33];
        yield return new WaitForSeconds(2.5f);
        text.text = "";
    }

    public void End()
    {
        StartCoroutine(EndSubs());
    }
    IEnumerator EndSubs()
    {
        text.text = subs.subtitle[34];
        yield return new WaitForSeconds(1.7f);
        text.text = subs.subtitle[35];
        yield return new WaitForSeconds(4.7f);
        text.text = subs.subtitle[36];
        yield return new WaitForSeconds(4f);
        text.text = "";
    }
}
