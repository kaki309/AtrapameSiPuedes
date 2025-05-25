using UnityEngine;

public class FinishAttempt : MonoBehaviour
{
    public GameObject levelCompletePanel;

    void Start()
    {
        levelCompletePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            finishLevel();   
        }
    }

    void finishLevel()
    { 
        levelCompletePanel.SetActive(true);
        GameManager.Instance.UnlockAndShowCursor();

        Time.timeScale = 0;
    }
}
