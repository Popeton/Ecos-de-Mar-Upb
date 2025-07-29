using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSongManglar : MonoBehaviour
{
    public void ActiveFinalSong()
    {
        StartCoroutine(FinalSong());
    }

    IEnumerator FinalSong()
    {
        yield return new WaitForSeconds(18.1f);
        AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase4, AudioManager.instance.transform.position);
    }
}
