using System.Collections;
using UnityEngine;

public class GuidePath : MonoBehaviour
{
    public static GuidePath Instance { get; private set; }

    [SerializeField] FishPathFollowing guidePath;
    [SerializeField] SwitchFishPaths[] switchPath;

    [SerializeField] GameObject guide;

    [SerializeField] float timeCorals = 15;
    [SerializeField] float timeSpores = 11;
    [SerializeField] float timeFishes = 15;
    [SerializeField] float timeWeb = 6;

    
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void InteractionCorals()
    {
        StartCoroutine(Corals());
    }
    
    IEnumerator Corals()
    {
        guide.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        guidePath.enabled = true;
        yield return new WaitForSeconds(timeCorals);
        FirtsCoral();
    }

    public void FirtsCoral()
    {
        guidePath.speed = 2;
        guidePath.rotationSpeed = 2;
        guidePath.decelerationDistance = 2;
        guidePath.loop = false;
        switchPath[0].SwitchPath();
    }
    
    public void SecondCoral()
    {
        guidePath.speed = 5;
        guidePath.rotationSpeed = 5;
        guidePath.decelerationDistance = 2;
        switchPath[1].SwitchPath();
    }

    public void ThirdCoral()
    {
        guidePath.speed = 1;
        guidePath.rotationSpeed = 1;
        guidePath.decelerationDistance = 1;
        switchPath[2].SwitchPath();
    }
    public void InteractionSpores()
    {
        guidePath.speed = 2;
        guidePath.rotationSpeed = 2;
        guidePath.decelerationDistance = 2;
        StartCoroutine(Spores());
    }

    IEnumerator Spores()
    {
        switchPath[3].SwitchPath();
        yield return new WaitForSeconds(timeSpores);
    }

    public void VideoSpores()
    {
        switchPath[4].SwitchPath();
    }

    public void InteractionFishesAndLora()
    {
        StartCoroutine(Fishes());
    }

    IEnumerator Fishes()
    {
        guidePath.loop = true;
        switchPath[5].SwitchPath();
        yield return new WaitForSeconds(timeFishes);
        guidePath.loop = false;
        switchPath[6].SwitchPath();
    }

    public void LoraPath()
    {
        switchPath[7].SwitchPath();
    }

    public void LoraNight()
    {
        switchPath[8].SwitchPath();
    }

    public void LoraEat()
    {
        switchPath[9].SwitchPath();
    }

    public void LoraPoop()
    {
        switchPath[10].SwitchPath();
    }

    public void InteractionWeb()
    {
        StartCoroutine(Web());
    }

    IEnumerator Web()
    {
        guidePath.loop = true;
        switchPath[11].SwitchPath();
        yield return new WaitForSeconds(timeWeb);
        guidePath.loop = false;
        switchPath[12].SwitchPath();
    }

    public void InteractionWhiteCorals()
    {
        guidePath.speed = 1;
        guidePath.rotationSpeed = 1;
        guidePath.decelerationDistance = 1;
        switchPath[13].SwitchPath();
    }
    
    public void InteractionPlastic()
    {
        switchPath[14].SwitchPath();
    }

    public void EndInteraction()
    {
        switchPath[15].SwitchPath();
    }
}
