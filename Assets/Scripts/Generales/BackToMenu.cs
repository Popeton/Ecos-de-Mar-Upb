using System.Collections;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
  //  public static BackToMenu Instance { get; private set; }
    [SerializeField] private float waitTime = 25; // Tiempo de espera en segundos antes de volver al menú
    private void Awake()
    {
        
       
    }

    public void OnBackToMenuButtonPressed()
    {
        StartCoroutine(LoadMenuScene());
    }

    private IEnumerator LoadMenuScene()
    {
        yield return new WaitForSeconds(waitTime);
        Initiate.Fade("Home", Color.black, 1f);
    }
}