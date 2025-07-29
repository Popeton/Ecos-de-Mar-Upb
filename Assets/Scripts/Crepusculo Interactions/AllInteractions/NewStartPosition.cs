using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewStartPosition : MonoBehaviour
{
    [SerializeField] Transform positionFollow;
    // Update is called once per frame
    void Update()
    {
        transform.position = positionFollow.position;
    }
}
