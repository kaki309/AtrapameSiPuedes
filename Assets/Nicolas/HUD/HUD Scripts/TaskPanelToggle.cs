using UnityEngine;

public class TaskPanelToggle : MonoBehaviour
{
    [Header("Animación")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Vector2 visiblePosition = new Vector2(150f, 0f); // Ajusta según tu UI
    [SerializeField] private Vector2 hiddenPosition = new Vector2(-400f, 0f);  // Fuera de pantalla

    private bool isVisible = true;
    private float animationTimer = 0f;
    private Vector2 startPos;
    private Vector2 endPos;
    private bool isAnimating = false;

    void Start()
    {
        panel.anchoredPosition = visiblePosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePanel();
        }

        if (isAnimating)
        {
            animationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(animationTimer / animationDuration);
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            if (t >= 1f) isAnimating = false;
        }
    }

    void TogglePanel()
    {
        isVisible = !isVisible;

        startPos = panel.anchoredPosition;
        endPos = isVisible ? visiblePosition : hiddenPosition;
        animationTimer = 0f;
        isAnimating = true;
    }
}

