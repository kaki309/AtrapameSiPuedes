using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamMovement : MonoBehaviour
{
    [SerializeField] Transform playerCamPosition;
    Transform originalCamTarget;
    Transform positionToFollow;
    void Start()
    {
        originalCamTarget = playerCamPosition;
        positionToFollow = playerCamPosition;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = positionToFollow.position;
    }

    public Transform getOriginalCamTarget()
    {
        return originalCamTarget;
    }

    public void setPositionToFollow(Transform position)
    {
        positionToFollow = position;
    }

}
