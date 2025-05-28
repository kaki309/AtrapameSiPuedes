using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            killPlayer();
        }
    }

    void killPlayer()
    {
        GameManager.Instance.PlayDeathSequence();

        Time.timeScale = 0;
    }
}
