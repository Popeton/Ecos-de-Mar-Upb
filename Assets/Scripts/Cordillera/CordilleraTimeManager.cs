using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CordilleraTimeManager : MonoBehaviour
{
    [SerializeField] GameObject[] mountainRange;
    [SerializeField] GameObject firstFish;
    [SerializeField] GameObject ethernet;
    [SerializeField] GameObject rov;
    [SerializeField] GameObject starfish;
    [SerializeField] GameObject cartel;

    [SerializeField] GameObject guia;

    public void ActivateMountainRange() { StartCoroutine(MountainRangeInfo()); }
    public void ActivateFirstFish() { StartCoroutine(SpeciesStart()); }
    public void ActivateEthernetCable() { StartCoroutine(EthernetCable()); }
    public void ActivateRov() { StartCoroutine(RovInteraction()); }
    public void ActivatedStarFishs() { StartCoroutine(StarFishes()); }
    public void ActivateRovInfo() { StartCoroutine(RovInfo()); }
    public void ActivateCartel() { StartCoroutine(StarCartel()); }

    public void AudiosActivation(int num)
    {
        switch (num)
        {
            case 0:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Voice, AudioManager.instance.transform.position);
                break;
            case 1:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase1, AudioManager.instance.transform.position);
                break;
            case 2:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase3, AudioManager.instance.transform.position);
                break;
            case 3:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion3_Fase1, AudioManager.instance.transform.position);
                break;
            case 4:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase4, AudioManager.instance.transform.position);
                break;
            case 5:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion2_Fase1, AudioManager.instance.transform.position);
                break;
            case 6:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion2_Fase2, AudioManager.instance.transform.position);
                break;
            case 7:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion4_Fase1, AudioManager.instance.transform.position);
                break;
            case 8:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion5_Fase1, AudioManager.instance.transform.position);
                break; 
            case 9:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion2_Fase2, AudioManager.instance.transform.position);
                break;
            case 10:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase3, AudioManager.instance.transform.position);
                break; 
            case 11:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion5_Fase2, AudioManager.instance.transform.position);
                break;
            case 12:
                AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion5_Fase3, AudioManager.instance.transform.position);
                break;
            default:
                break;
        }
    }

    private IEnumerator MountainRangeInfo()
    {

        yield return new WaitForSeconds(18f);
        AudioManager.instance.PlaySoundByName("Sound11");
        // AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase1, AudioManager.instance.transform.position);
        mountainRange[0].GetComponent<Collider>().enabled = true;
        mountainRange[1].GetComponent<Outlinable>().enabled = true;
        mountainRange[2].GetComponent<Outlinable>().enabled = true;
        mountainRange[0].GetComponent<XRSimpleInteractable>().enabled = true;

        // yield return new WaitForSeconds(2f);
        //mountainRange.SetActive(false);
    }

    private IEnumerator SpeciesStart()
    {

        yield return new WaitForSeconds(31f);
       // guia.GetComponent<PathMover>().JumpToSpecificPath(1);
        AudioManager.instance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase2, AudioManager.instance.transform.position);
        firstFish.GetComponent<Collider>().enabled=true;
        firstFish.GetComponent<Outlinable>().enabled=true;
        firstFish.GetComponent<XRSimpleInteractable>().enabled=true;
        //firstFish.GetComponent<PathMover>().StartExperience();
    }

    private IEnumerator EthernetCable()
    {
        yield return new WaitForSeconds(20f);
        AudioManager.instance.PlaySoundByName("Sound13");
        ethernet.GetComponentInChildren<Collider>().enabled = true;
        ethernet.GetComponent<Outlinable>().enabled = true;
        ethernet.GetComponent<XRSimpleInteractable>().enabled = true;
    }
    private IEnumerator RovInteraction()
    {
        yield return new WaitForSeconds(7f);
        AudioManager.instance.PlaySoundByName("Sound4");
        rov.SetActive(true);
        rov.GetComponent<PathMover>().StartExperience();
        StartCoroutine(RovInteractionStart());
    }

    private IEnumerator RovInteractionStart()
    {
        yield return new WaitForSeconds(18.4f);
        rov.GetComponentInChildren<Collider>().enabled = true;
        rov.GetComponentInChildren<Outlinable>().enabled = true;
        rov.GetComponentInChildren<XRSimpleInteractable>().enabled = true;
    }

    private IEnumerator RovInfo()
    {
        yield return new WaitForSeconds(13f);
       // AudioManager.instance.PauseVoice();
       
        rov.GetComponentInChildren<Collider>().enabled = true;
        rov.GetComponentInChildren<Outlinable>().enabled = true;
        rov.GetComponentInChildren<XRSimpleInteractable>().enabled = true;
        rov.GetComponentInChildren<CheckRovInfoStatus>().isRovInfoActive = true;
    }

    private IEnumerator StarFishes()
    {
        yield return new WaitForSeconds(29f);
        starfish.GetComponent<Collider>().enabled = true;
        starfish.GetComponent<Outlinable>().enabled = true;
        starfish.GetComponent<XRSimpleInteractable>().enabled = true;

    }   
    private IEnumerator StarCartel()
    {
        yield return new WaitForSeconds(10f);
        cartel.GetComponent<Collider>().enabled = true;
        cartel.GetComponent<Outlinable>().enabled = true;
        cartel.GetComponent<XRSimpleInteractable>().enabled = true;

    }
}