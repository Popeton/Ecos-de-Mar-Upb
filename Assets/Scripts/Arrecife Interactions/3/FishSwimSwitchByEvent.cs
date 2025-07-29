using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimSwitchByEvent : MonoBehaviour
{
    // Start is called before the first frame update
    FishSwimming fishAreaSwim;
    Transform firstArea;
    [SerializeField] float newRotationSpeed=4f;
    [SerializeField] float  newSpeed=4f;
    [SerializeField] Transform newArea;
    [SerializeField] CustomEvent switchEvent;

    float originalSpeed;
    float originalRotationSpeed;
    void Start()
    {
        fishAreaSwim = GetComponent<FishSwimming>();
        firstArea=fishAreaSwim.swimAreaCenter;
        originalSpeed = fishAreaSwim.speed;
        originalRotationSpeed = fishAreaSwim.rotationSpeed;
        switchEvent.GEvent += SwitchArea;
    }

    public void SwitchArea()
    {
        fishAreaSwim.rotationSpeed= newRotationSpeed;
        fishAreaSwim.speed= newSpeed;
        fishAreaSwim.swimAreaCenter=newArea;
    }

    public void RestoreOriginalPos()
    {
        fishAreaSwim.rotationSpeed = originalRotationSpeed;
        fishAreaSwim.speed = originalSpeed;
        fishAreaSwim.swimAreaCenter = firstArea;
    }
    private void OnDestroy()
    {
        switchEvent.GEvent -= SwitchArea;
    }
}
