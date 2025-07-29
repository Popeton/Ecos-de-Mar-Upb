using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwilightSubtitles : MonoBehaviour
{
    public static TwilightSubtitles Instance { get; private set; }

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

    //esto es para mostrar 3 subtitulos
    public void ThreeSubtitles(int sub1,int sub2,int sub3, float del1,float del2,float del3)
    {
        StartCoroutine(ThreeSub(sub1,sub2,sub3, del1,del2,del3));
    }
    IEnumerator ThreeSub(int sub1, int sub2, int sub3, float del1, float del2, float del3)
    {
        text.text = subs.subtitle[sub1];
        yield return new WaitForSeconds(del1);
        text.text = subs.subtitle[sub2];
        yield return new WaitForSeconds(del2);
        text.text = subs.subtitle[sub3];
        yield return new WaitForSeconds(del3);
        text.text = "";
    }

    //esto es para mostrar 4 subtitulos
    public void FourSubtitles(int sub1, int sub2, int sub3,int sub4, float del1, float del2, float del3,float del4)
    {
        StartCoroutine(FourSub(sub1, sub2, sub3,sub4, del1, del2, del3,del4));
    }
    IEnumerator FourSub(int sub1, int sub2, int sub3, int sub4, float del1, float del2, float del3, float del4)
    {
        text.text = subs.subtitle[sub1];
        yield return new WaitForSeconds(del1);
        text.text = subs.subtitle[sub2];
        yield return new WaitForSeconds(del2);
        text.text = subs.subtitle[sub3];
        yield return new WaitForSeconds(del3);
        text.text = subs.subtitle[sub4];
        yield return new WaitForSeconds(del4);
        text.text = "";
    }

    //===================================================================================

    public void Intro()
    {
        StartCoroutine(IntroSubs());
    }

    IEnumerator IntroSubs()
    {
        text.text = subs.subtitle[0];
        yield return new WaitForSeconds(6.2f);
        text.text = subs.subtitle[1];
        yield return new WaitForSeconds(4.2f);
        text.text = subs.subtitle[2];
        yield return new WaitForSeconds(5f);
        text.text = subs.subtitle[3];
        yield return new WaitForSeconds(3f);
        text.text = subs.subtitle[4];
        yield return new WaitForSeconds(2.5f);
        text.text = subs.subtitle[5];
        yield return new WaitForSeconds(6f);
        text.text = "";
    }


    public void End()
    {
        StartCoroutine (EndSubs());
    }

    IEnumerator EndSubs()
    {
        text.text = subs.subtitle[29];
        yield return new WaitForSeconds(3.4f);
        text.text = subs.subtitle[30];
        yield return new WaitForSeconds(3.8f);
        text.text = "";
    }
}
