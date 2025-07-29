using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrowCoralsManager : MonoBehaviour
{
    [SerializeField] GrowObject video;

    private bool onInteraction;

    
    public void RemoveVideo()
    {
        if (!onInteraction) {
            StartCoroutine(ShrinkAndAppearCoral());
        }
    }

    IEnumerator ShrinkAndAppearCoral()
    {
        onInteraction = true;
        //GuidePath.Instance.PreFishes();
        video.Shrink();
        yield return new WaitForSeconds(0.7f);
        FullCoralFlow.Instance.SeedInteraction();
    }

    //testing

    private void OnDisable()
    {
        RemoveVideo();
    }
}

