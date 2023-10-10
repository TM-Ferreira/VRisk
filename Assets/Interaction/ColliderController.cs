using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderController : MonoBehaviour
{
    private Collider player_collider;
    private Transform collider_transform;
    public Transform head;

    private void Start()
    {
        player_collider = GetComponent<Collider>();
        collider_transform = player_collider.transform;
    }

    private void FixedUpdate()
    {
        collider_transform.position = new Vector3(head.position.x, 
                                    collider_transform.position.y,
                                    head.position.z);
    }
}
