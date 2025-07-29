using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePathTwilight : MonoBehaviour
{
    public static GuidePathTwilight Instance { get; private set; }

    [SerializeField] FishPathFollowing guidePath;
    [SerializeField] SwitchFishPaths[] switchPath;

    [SerializeField] GameObject guide;


    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartExperience()
    {
        StartCoroutine(BeginExperience());
    }
    //inicio
    IEnumerator BeginExperience()
    {
        guide.SetActive(true);
        yield return new WaitForSeconds(2f);
        guidePath.enabled = true;
    }

    //cambia a interaccion de plancton

    public void PlanctonInteraction()
    {
        guidePath.speed = 0.5f;
        guidePath.rotationSpeed = 0.5f;
        guidePath.loop=false;
        switchPath[0].SwitchPath();
    }

    //cambia a cachalote con ficha

    public void CachaloteCard()
    {
        guidePath.speed = 2.75f;
        guidePath.loop = true;
        guidePath.rotationSpeed = 2f;
        switchPath[1].SwitchPath();
    }

    public void BrigthFishCard()
    {
        StartCoroutine(BrigthPath());
    }

    IEnumerator BrigthPath()
    {
        guidePath.speed = 2.3f;
        guidePath.rotationSpeed = 2.3f;
        guidePath.loop = true;
        yield return new WaitForSeconds(1.5f);
        switchPath[2].SwitchPath();
    }

    

    public void JellyfishToFinal()
    {
        guidePath.speed = 3f;
        guidePath.loop = true;
        guidePath.rotationSpeed = 1.5f;
        switchPath[3].SwitchPath();
    }

    public void InfoCard()
    {
        guidePath.speed = 2f;
        guidePath.loop = false;
        switchPath[4].SwitchPath();
    }
    //cambia a mantarraya

    public void StingRay()
    {
        guidePath.loop = true;
        guidePath.speed = 3f;
        guidePath.rotationSpeed = 3f;
        switchPath[5].SwitchPath();
    }

    //cambia a tiburon
     public void Mako()
    {
        guidePath.speed = 3f;
        guidePath.rotationSpeed = 3f;
        guidePath.loop = true;
        switchPath[6].SwitchPath();
    }

    //final

    public void End()
    {
        guidePath.loop = true;
        switchPath[6].SwitchPath();
    }
}
