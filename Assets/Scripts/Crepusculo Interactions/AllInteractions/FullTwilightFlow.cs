using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullTwilightFlow : MonoBehaviour
{
    public static FullTwilightFlow Instance { get; private set; }

    [Header("General Configurations")]
    public AudioManager audiMana;
    public Transform guide;

    [Header("Interactions")]
    public GameObject[] interaction;

    private void Awake()
    {
        guide = audiMana.gameObject.transform;
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ActivateInteraction(int i,float delay,bool state)
    {
        StartCoroutine(ActivateWithDelay(i, delay,state));
    }

    IEnumerator ActivateWithDelay(int i,float delay,bool state)
    {
        yield return new WaitForSeconds(delay);
        interaction[i - 1].SetActive(state);
    }
}
