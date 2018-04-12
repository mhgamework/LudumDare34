using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncVRCameraToPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform = null;
    [SerializeField]
    private Vector3 offset = new Vector3();
    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z) + offset;
    }
}
