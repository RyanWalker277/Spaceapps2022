using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Transform cameraPosition;

    Vector3 _followOffset;

    void Start()
    {
        // Cache the initial offset at time of load/spawn:
        _followOffset = transform.position - cameraPosition.position;
    }
    
    void LateUpdate () 
    {
        Vector3 targetPosition = cameraPosition.position + _followOffset;
        transform.position += (targetPosition - transform.position);
    }
}
