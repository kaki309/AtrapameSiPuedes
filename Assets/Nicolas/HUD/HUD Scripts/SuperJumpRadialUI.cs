using UnityEngine;
using UnityEngine.UI;

public class SuperJumpUIController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    [Header("UI")]
    [SerializeField] private Image spriteHolder;
    [SerializeField] private Sprite readySprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite cooldownSprite;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundReady;
    [SerializeField] private AudioClip soundActivated;
    [SerializeField] private AudioClip soundDeactivated;

    [Header("Blink Effect")]
    [SerializeField] private float blinkSpeed = 2f;
    private bool blinking = false;

    private float superJumpDuration;
    private float superJumpCooldown;
    private float blinkTimer = 0f;

    private enum SuperJumpState { Ready, Active, Cooldown }
    private SuperJumpState currentState = SuperJumpState.Ready;

    void Start()
    {
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();

        superJumpDuration = playerMovement.GetSuperJumpDuration();
        superJumpCooldown = playerMovement.GetSuperJumpCooldown();

        SetUIReady();
    }

    void Update()
    {
        if (playerMovement.IsSuperJumpActive())
        {
            float timer = playerMovement.GetSuperJumpTimer();
            float fillAmount = Mathf.Clamp01(1f - (timer / superJumpDuration));
            SetUIActive(fillAmount);
        }
        else if (!playerMovement.CanActivateSuperJump())
        {
            float cooldownTimer = playerMovement.GetSuperJumpCooldownTimer();
            float fillAmount = Mathf.Clamp01(cooldownTimer / superJumpCooldown);
            SetUICooldown(fillAmount);
        }
        else
        {
            SetUIReady();
        }

        // Aplicar parpadeo si est� activo
        if (blinking)
        {
            blinkTimer += Time.deltaTime * blinkSpeed;
            float alpha = Mathf.Lerp(0.3f, 1f, Mathf.PingPong(blinkTimer, 1f));
            Color color = spriteHolder.color;
            color.a = alpha;
            spriteHolder.color = color;
        }
    }

    private void SetUIReady()
    {
        if (currentState == SuperJumpState.Ready) return;

        blinking = false;
        ResetAlpha();

        spriteHolder.sprite = readySprite;
        spriteHolder.fillAmount = 1f;
        spriteHolder.type = Image.Type.Simple;

        if (currentState == SuperJumpState.Cooldown)
            PlaySound(soundReady);

        currentState = SuperJumpState.Ready;
    }

    private void SetUIActive(float fill)
    {
        if (currentState != SuperJumpState.Active)
        {
            blinking = true;
            blinkTimer = 0f;

            spriteHolder.sprite = activeSprite;
            spriteHolder.type = Image.Type.Filled;
            spriteHolder.fillMethod = Image.FillMethod.Radial360;
            spriteHolder.fillOrigin = (int)Image.Origin360.Top; // Desde las 12
            spriteHolder.fillClockwise = true;

            PlaySound(soundActivated);
            currentState = SuperJumpState.Active;
        }

        spriteHolder.fillAmount = fill;
    }

    private void SetUICooldown(float fill)
    {
        if (currentState != SuperJumpState.Cooldown)
        {
            blinking = false;
            ResetAlpha();

            spriteHolder.sprite = cooldownSprite;
            spriteHolder.type = Image.Type.Filled;
            spriteHolder.fillMethod = Image.FillMethod.Vertical;
            spriteHolder.fillOrigin = (int)Image.OriginVertical.Bottom;

            if (currentState == SuperJumpState.Active)
                PlaySound(soundDeactivated);

            currentState = SuperJumpState.Cooldown;
        }

        spriteHolder.fillAmount = fill;
    }

    private void ResetAlpha()
    {
        Color color = spriteHolder.color;
        color.a = 1f;
        spriteHolder.color = color;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}




