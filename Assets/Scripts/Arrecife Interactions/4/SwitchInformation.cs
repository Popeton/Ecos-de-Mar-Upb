using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SwitchInformation : MonoBehaviour
{
    public static SwitchInformation Instance { get; private set; }

    private int infoCard;

    [SerializeField] Color newColor;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Sprite newImage;
    [SerializeField] Image actualSprite;
    
    [SerializeField] GameObject[] card;
    [SerializeField] GameObject[] loraAnim;
    
    //[SerializeField] float[] delayAppear;
    [SerializeField] float delayDisappear=0.7f;

    private bool audioOn;
    [SerializeField] GrowObject button;

    [SerializeField] GameObject allInfo;

    [SerializeField] AudioManager audioMana;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Destruye la instancia duplicada
            return;
        }

        Instance = this;
        
    }

    void ChangeAspect()
    {
        actualSprite.sprite = newImage;
        text.color = newColor;
    }

    public void ChangeInformation()
    {
        StartCoroutine(ChangeCard());
    }

    IEnumerator ChangeCard()
    {
        switch (infoCard) {
            case 0:
                //reduce las cartas
                allInfo.GetComponent<GrowObject>().Shrink();
                yield return new WaitForSeconds(delayDisappear);
                ChangeAspect();
                button.Shrink();
                //desaparece la tarjeta actual
                card[infoCard].SetActive(false);
                loraAnim[infoCard].SetActive(false);
                infoCard++;

                //reproducir audio lora comer
                audioMana.CleanUp();
                ArrecifeSubtitles.Instance.LoraComer();
                audioMana.InitializeVoice(FmodEvents.instance.Interaccion4_Fase1, audioMana.transform.position);
                //AudioTesting.Instance.InitializeClip(8);
                LoraInteraccionController.Instance.LoraEat();
                GuidePath.Instance.LoraEat();

                yield return new WaitForSeconds(LoraInteraccionController.Instance.delayEatAudio); 
                LoraInteraccionController.Instance.ActivateInteraction(true);

                break;
            case 1:
                //reduce las cartas
                allInfo.GetComponent<GrowObject>().Shrink();
                yield return new WaitForSeconds(delayDisappear);
                button.Shrink();
                //desaparece la tarjeta actual
                card[infoCard].SetActive(false);
                loraAnim[infoCard].SetActive(false);
                infoCard++;

                //reproducir audio lora digestion
                audioMana.CleanUp();
                ArrecifeSubtitles.Instance.LoraDigestion();
                audioMana.InitializeVoice(FmodEvents.instance.Interaccion5_Fase1, audioMana.transform.position);
                LoraInteraccionController.Instance.LoraPoop();
                GuidePath.Instance.LoraPoop();
                //AudioTesting.Instance.InitializeClip(9);

                yield return new WaitForSeconds(LoraInteraccionController.Instance.delayPSandAudio);
                LoraInteraccionController.Instance.ActivateInteraction(true);

                break;
            case 2:
                infoCard++;
                StartCoroutine(ShrinkAndDeactivate());
                break;
        }
    }

    public void AppearOtherCard()
    {
        StartCoroutine(AppearNextCard());
    }

    IEnumerator AppearNextCard()
    {
        //yield return new WaitForSeconds(delayAppear[infoCard]);
        //aparece la tarjeta nueva
        card[infoCard].SetActive(true);
        loraAnim[infoCard].SetActive(true);
        allInfo.GetComponent<GrowObject>().Grow();
        yield return new WaitForSeconds(1f);
        button.Grow();
    }

    IEnumerator ShrinkAndDeactivate()
    {
        allInfo.GetComponent<GrowObject>().Shrink();
        LoraInteraccionController.Instance.EndInteraction();
        yield return new WaitForSeconds(1f);
        allInfo.SetActive(false);
    }

    //testing
    private void OnDisable()
    {
        ChangeInformation();
    }
}
