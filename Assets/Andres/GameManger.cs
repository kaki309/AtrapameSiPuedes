using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Kill Player")]
    [SerializeField] private CanvasGroup deathPanelGroup;
    [SerializeField] private float failSlowMotionTime = 4f;
    [Header("Hide Counter")]
    public int hideCount = 0;

    private void Awake()
    {
        // Asegurar singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // Opcional si quieres que persista entre escenas
    }

    private void Start()
    {
        Time.timeScale = 1;
        LockAndHideCursor();
        deathPanelGroup.gameObject.SetActive(false);
        hideCount = 0;
    }

    public void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void loadSceneByNumber(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // SECUENCE FOR PLAYER DEATH
    public void PlayDeathSequence(float slowDuration = 3f)
    {
        StartCoroutine(SlowMotionThenFade(slowDuration));
    }

    private IEnumerator SlowMotionThenFade(float slowDuration)
    {
        float startScale = Time.timeScale;
        float t = 0f;

        // ⏳ Disminuir Time.timeScale progresivamente a lo largo de `slowDuration`
        while (t < slowDuration)
        {
            t += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(startScale, 0f, t / slowDuration);
            yield return null;
        }

        Time.timeScale = 0f;

        // 🎬 Activar panel de muerte con fade in
        StartCoroutine(FadeInDeathPanel(failSlowMotionTime));
    }

    private IEnumerator FadeInDeathPanel(float fadeDuration)
    {
        deathPanelGroup.gameObject.SetActive(true);
        deathPanelGroup.alpha = 0f;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            deathPanelGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        deathPanelGroup.alpha = 1f;
        // 🕶️ Asegurar cursor visible
        UnlockAndShowCursor();
    }


}
