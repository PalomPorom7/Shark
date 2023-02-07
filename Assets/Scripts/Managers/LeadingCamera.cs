using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadingCamera : MonoBehaviour
{
    [SerializeField]
    private Transform toFollow;

    private void LateUpdate()
    {
        transform.position = new Vector3(toFollow.position.x, 0, -10);
    }
}
