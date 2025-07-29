using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    private static bool hasReloaded = false;

    void Start()
    {
        if (!hasReloaded)
        {
            hasReloaded = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public static void ResetFlag()
    {
        hasReloaded = false;
    }
}

