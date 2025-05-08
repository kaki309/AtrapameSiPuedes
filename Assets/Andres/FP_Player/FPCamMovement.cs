using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamMovement : MonoBehaviour
{
    [SerializeField] Transform playerCamPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCamPosition.position;
    }
}
