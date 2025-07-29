using UnityEngine;

public class ToggleObjectByEvent : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private bool activateOnEvent = true;
    [SerializeField] private float delayTime = 1f;
    [SerializeField] private CustomEvent changeEvent;

    void Start()
    {
        if (changeEvent != null)
            changeEvent.GEvent += ToggleObject;
    }

    private void ToggleObject()
    {
        Invoke(nameof(DoToggle), delayTime);
    }

    private void DoToggle()
    {
        if (targetObject != null)
            targetObject.SetActive(activateOnEvent);
    }

    private void OnDestroy()
    {
        if (changeEvent != null)
            changeEvent.GEvent -= ToggleObject;
    }
}
