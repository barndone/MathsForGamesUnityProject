using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //the object to follow
    public Transform followTarget;

    //the offset from to follow the object at
    public Vector3 followOffset;

    private void Start()
    {
        followOffset = transform.position - followTarget.position;
    }

    private void LateUpdate()
    {
        transform.position = followTarget.position + followOffset;
    }
}
