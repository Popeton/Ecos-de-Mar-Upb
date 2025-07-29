using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchByEvent : MonoBehaviour
{

    [SerializeField] SwitchFishPaths switchPath;
    [SerializeField] CustomEvent eventToChange;
    
    void Start()
    {
        eventToChange.GEvent += ChangePath;
    }

    void ChangePath()
    {
        switchPath.SwitchPath();
    }

    private void OnDestroy()
    {
        eventToChange.GEvent -= ChangePath;
    }
}
