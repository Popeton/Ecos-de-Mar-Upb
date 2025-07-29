using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;

public class WebInCoralController : MonoBehaviour
{
    [SerializeField] MoveObjectDown  webPosRef;
    [SerializeField] FadeOut webMaterial;
    [SerializeField] CustomEvent scareFishEvent,appearWeb;
    [SerializeField] float webDroppingTime;
    [SerializeField] float distanceDrop = 5;
    [SerializeField] float speedDrop = 1;
    [SerializeField] float distanceUp = 2;
    [SerializeField] float speedUp = 1;

    public List<GameObject> fishwithPathSwitch;

    private XRSimpleInteractable webInteractable;
    Outlinable outline;

    private bool onInteraction;

    private void Start()
    {
        webInteractable = GetComponent<XRSimpleInteractable>();
        outline = GetComponent<Outlinable>();
        webInteractable.enabled = false;
        webPosRef.moveSpeed = speedDrop;
        webPosRef.moveDistance = distanceDrop;
        AppearWeb();
    }

    public void StartWebInteraction(XRSimpleInteractable interactable)
    {
        if(interactable==webInteractable && !onInteraction) {
            StartCoroutine(WebInteraction());
        }
    }

    IEnumerator WebInteraction()
    {
        webInteractable.enabled = false;
        outline.enabled = false;
        onInteraction = true;
        webPosRef.MoveDown(); //poner valores negativos para que vaya hacia arriba
        yield return new WaitForSeconds(1f);
        webMaterial.StartFadeOut();
        yield return new WaitForSeconds(1.5f);
        FullCoralFlow.Instance.HurtLoraInteraction();
    }
    //cambio de path de los peces asustados
    void ChangePaths(bool switchPath , float minSpeed , float maxSpeed)
    {
        foreach (GameObject fish in fishwithPathSwitch) {
            if(switchPath) fish.GetComponent<SwitchFishPaths>().SwitchPath();
            fish.GetComponent<FishPathFollowing>().speed = Random.Range(minSpeed, maxSpeed);
            fish.GetComponent<FishPathFollowing>().rotationSpeed = Random.Range(minSpeed, maxSpeed);
        }
        scareFishEvent.FireEvent();
    }

    IEnumerator InitialAparition()
    {
        ChangePaths(true, 3f, 5f); //se asustan los peces
        //deben desaparecer los peces
        appearWeb.FireEvent();
        yield return new WaitForSeconds(webDroppingTime);
        
        webInteractable.enabled = true;
        outline.enabled = true;
        webPosRef.moveSpeed = speedUp;
        webPosRef.moveDistance = -distanceUp;
        ChangePaths(false, 1f, 2f); // se calman
    }

    public void AppearWeb()
    {
        StartCoroutine(InitialAparition());
    }
    //testing
    private void OnDisable()
    {
        if (!onInteraction) {
            StartCoroutine(WebInteraction());
        }
    }

}
