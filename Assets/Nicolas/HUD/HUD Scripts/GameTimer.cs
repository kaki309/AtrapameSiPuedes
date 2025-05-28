using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [Header("Tiempo (en segundos)")]
    [SerializeField] private float startTime = 90f;

    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;

    private float currentTime;
    private bool isGameOver = false;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            GameOver();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("¡Game Over!");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // PAUSA global del tiempo
        Time.timeScale = 0f;

        // Opcional: desbloquear cursor si estabas usando cámara FPS
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}


