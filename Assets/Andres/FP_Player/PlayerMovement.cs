using TMPro;
using UnityEngine;
using UnityEngine.UI; // Añadido para usar Slider

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerObj;
    public TMP_Text debugText;

    [Header("Movement")]
    float horizontalInput;
    float verticalInput;
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;

    // Nuevas variables para correr
    [Header("Sprint")]
    [SerializeField] float sprintMultiplier = 1.5f;
    [SerializeField] float sprintDuration = 8f;
    [SerializeField] Slider energySlider;
    private float currentEnergy = 100f;
    private bool isSprinting = false;
    private float baseSpeed;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    bool isGrounded;
    bool readyToJump = true;

    [Header("Super Jump")]
    [SerializeField] float superJumpMultiplier = 2f;
    [SerializeField] float superJumpDuration = 5f;
    [SerializeField] float superJumpCooldown = 20f;
    bool canActivateSuperJump = false;
    bool isSuperJumpActive = false;
    protected float superJumpTimer = 0f;
    protected float superJumpCooldownTimer = 0f;

    public Animator anim;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseSpeed = moveSpeed;
        
        // Inicializar slider de energía
        if (energySlider != null)
        {
            energySlider.maxValue = 100f;
            energySlider.value = currentEnergy;
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.2f);

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        speedControl();

        if (Input.GetKeyDown(KeyCode.Space)) jump();

        // Powers
        HandleSuperJump();
        HandleSprint();

        // Debug
        if (debugText != null) debugText.text = $"Salto cooldown:{superJumpCooldownTimer:F1} Salto tActivo:{superJumpTimer:F1} Energía:{currentEnergy:F1}%";
        // --- ANIMACIONES --- //
        /*
        float speed = new Vector2(horizontalInput, verticalInput).magnitude;
        anim.SetFloat("Speed", speed);

        anim.SetBool("IsJumping", !isGrounded); // True si está en el aire

        if (Input.GetKeyDown(KeyCode.K)) // Puedes cambiar la tecla
        {
            anim.SetTrigger("Die");
        }
        */
    }

    void FixedUpdate()
    {
        move();
    }

    void move()
    {
        Vector3 moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (playerObj!=null) playerObj.rotation = orientation.rotation;

        if (isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDir.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
    }

    void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void jump()
    {
        if (readyToJump && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            readyToJump = false;
            float currentJumpForce = isSuperJumpActive ? jumpForce * superJumpMultiplier : jumpForce;
            rb.AddForce(transform.up * currentJumpForce, ForceMode.Impulse);
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    void resetJump()
    {
        readyToJump = true;
    }

    void HandleSuperJump()
    {
        if (!canActivateSuperJump)
        {
            superJumpCooldownTimer += Time.deltaTime;
            if (superJumpCooldownTimer >= superJumpCooldown)
            {
                canActivateSuperJump = true;
            }
        }

        if (canActivateSuperJump && Input.GetKeyDown(KeyCode.E) && !isSuperJumpActive)
        {
            ActivateSuperJump();
        }

        if (isSuperJumpActive)
        {
            superJumpTimer += Time.deltaTime;
            if (superJumpTimer >= superJumpDuration)
            {
                DeactivateSuperJump();
            }
        }
    }

    void ActivateSuperJump()
    {
        isSuperJumpActive = true;
        superJumpTimer = 0f;
    }

    void DeactivateSuperJump()
    {
        canActivateSuperJump = false;
        isSuperJumpActive = false;
        superJumpCooldownTimer = 0f;
    }

    // Nuevo método para manejar la funcionalidad de sprint
    void HandleSprint()
    {
        // Verificar si el jugador está intentando correr (presionando Shift)
        bool tryingToSprint = Input.GetKey(KeyCode.LeftShift);
        
        // Si el jugador intenta correr y tiene energía
        if (tryingToSprint && currentEnergy > 0)
        {
            // Activar sprint si no estaba activo
            if (!isSprinting)
            {
                isSprinting = true;
                moveSpeed = baseSpeed * sprintMultiplier;
            }
            
            // Reducir energía mientras corre
            currentEnergy -= (100f / sprintDuration) * Time.deltaTime;
            
            // Limitar la energía a 0 como mínimo
            if (currentEnergy < 0)
                currentEnergy = 0;
        }
        // Si el jugador deja de correr o se queda sin energía
        else if (isSprinting)
        {
            // Desactivar sprint
            isSprinting = false;
            moveSpeed = baseSpeed;
        }
        
        // Regenerar energía cuando no está corriendo
        if (!isSprinting && currentEnergy < 100f)
        {
            // Regenerar energía
            currentEnergy += (100f / (sprintDuration * 1.5f)) * Time.deltaTime;
            
            // Limitar la energía a 100 como máximo
            if (currentEnergy > 100f)
                currentEnergy = 100f;
        }
        
        // Actualizar el slider de energía
        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }
    }

    public void RestoreEnergy(float amount)
    {
        // Añadir la cantidad de energía
        currentEnergy += amount;
        
        // Limitar a máximo 100
        if (currentEnergy > 100f)
            currentEnergy = 100f;
            
        // Actualizar slider
        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }
    }
    // GETTERS PARA LLAMAR DESDE OTRO SCRIPT
    public float GetSuperJumpDuration()
    {
        return superJumpDuration;
    }
    public float GetSuperJumpTimer()
    {
        return superJumpTimer;
    }
    public bool IsSuperJumpActive()
    {
        return isSuperJumpActive;
    }
    public float GetSuperJumpCooldown() => superJumpCooldown;
    public float GetSuperJumpCooldownTimer() => superJumpCooldownTimer;
    public bool CanActivateSuperJump() => canActivateSuperJump;


}