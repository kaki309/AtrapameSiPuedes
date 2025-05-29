using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            killPlayer();
            //Debug.Log(playerRb.name);
        }
    }

    void killPlayer()
    {
        GameManager.Instance.PlayDeathSequence();
    }
}
