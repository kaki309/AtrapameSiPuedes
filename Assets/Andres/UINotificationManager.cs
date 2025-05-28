using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UINotificationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject uiNotificationPanel;
    private TMP_Text notificationText;
    [SerializeField] private Image notificationIcon;

    private void Start()
    {
        // Asegurarse de que la notificación está desactivada al inicio
        if (uiNotificationPanel != null) uiNotificationPanel.SetActive(false);
        notificationText = uiNotificationPanel.transform.GetComponentInChildren<TMP_Text>();
    }

    // Método público para notificar que se ha recogido un powerup
    public void NotifyEventOnGame(string powerupName, Sprite icon = null)
    {
        if (uiNotificationPanel != null && notificationText != null)
        {
            // Configurar texto e icono
            notificationText.text = powerupName;

            if (icon != null && notificationIcon != null)
                notificationIcon.sprite = icon;

            // Mostrar notificación
            StopCoroutine(nameof(ShowNotificationCoroutine));
            StartCoroutine(ShowNotificationCoroutine());
        }
    }

    // Corrutina para mostrar la notificación por unos segundos
    private IEnumerator ShowNotificationCoroutine()
    {
        uiNotificationPanel.SetActive(true);

        // Efecto de fade in
        CanvasGroup canvasGroup = uiNotificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            float duration = 0.5f;
            float elapsed = 0;

            while (elapsed < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1;
        }

        // Mantener visible
        yield return new WaitForSeconds(2.5f);

        // Efecto de fade out
        if (canvasGroup != null)
        {
            float duration = 0.5f;
            float elapsed = 0;

            while (elapsed < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        uiNotificationPanel.SetActive(false);
    }

    // FUNCIONAMIENTO DE COLA DE MENSAJES

    private Queue<NotificationData> notificationQueue = new Queue<NotificationData>();
    private bool isDisplaying = false;

    private class NotificationData
    {
        public string message;
        public Sprite icon;

        public NotificationData(string msg, Sprite icn)
        {
            message = msg;
            icon = icn;
        }
    }
    public void QueueNotification(string message, Sprite icon = null)
    {
        notificationQueue.Enqueue(new NotificationData(message, icon));

        if (!isDisplaying)
            StartCoroutine(ProcessNotificationQueue());
    }
    private IEnumerator ProcessNotificationQueue()
    {
        isDisplaying = true;

        while (notificationQueue.Count > 0)
        {
            NotificationData current = notificationQueue.Dequeue();
            NotifyEventOnGame(current.message, current.icon);

            // Esperar a que termine la notificación actual (3.5s en total por tu corrutina)
            yield return new WaitForSeconds(3.5f);
        }

        isDisplaying = false;
    }



}