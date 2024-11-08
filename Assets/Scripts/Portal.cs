using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject portalDestination;

    public Vector2 Teleport()
    {
        return portalDestination.transform.position;
    }
}
