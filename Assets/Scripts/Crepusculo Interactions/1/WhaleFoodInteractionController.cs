using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WhaleFoodInteractionController : MonoBehaviour
{
    [SerializeField] float wholeDelay=27f;
    [SerializeField] ParticleSystem inkEffect;
    [SerializeField] float nextInteractionDelay=2f;

    void Start()
    {
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound11);
        StartCoroutine(WhaleArrive());
        StartCoroutine(BiteSound());
        
    }

    IEnumerator WhaleArrive()
    {
        //sonido cachalote
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound2);
        //sonido de nado de ballena
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound6);
        yield return new WaitForSeconds(wholeDelay);
        inkEffect.Play();
        //sonido de tinta
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound10);
        yield return new WaitForSeconds(nextInteractionDelay);
        //se activa la interaccion de plancton
        FullTwilightFlow.Instance.ActivateInteraction(2, 0f, true);
        //sonido de plancton
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound9);
        //sonido de nado de ballena
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound6);
    }

    IEnumerator BiteSound()
    {
        yield return new WaitForSeconds(25f);
        //sonido de mandibula
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound4);
    }
}
