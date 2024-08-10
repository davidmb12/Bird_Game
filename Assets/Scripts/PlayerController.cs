using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    AnimationHandler animationHandler;
    [SerializeField]
    Camera mainCam;
    [SerializeField]
    GameManagerScript gameManager;
    [SerializeField]
    ColorManager colorManager;

    [Header("Ground Check")]
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    LayerMask whatIsGround;
    [SerializeField]
    float boxCastHeight;
    [SerializeField]
    float checkDistance;
    [SerializeField]
    Vector3 boxSize = new Vector3(1f, 0.1f, 1f);
    public bool isGrounded;

    [Header("Color Handling")]
    [SerializeField]
    float initialColorLinearValue;
    float currentColorLinearValue;
    [SerializeField]
    float jumpColorCost;
    [SerializeField]
    float dashColorCost;
    [SerializeField]
    float colorRegenRate;
    [SerializeField]
    Color initialBackgroundColor;
    public Color currentPlayerColor;
    [SerializeField]
    float regenStartTime;

    [Header("Movement")]
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float dashCooldown;
    [SerializeField]
    float slideSpeed;
    bool isSliding;
    [SerializeField]
    SpriteRenderer playerSpriteRenderer;
    bool canRegenColor;
    bool canJump;
    bool canDash;
    [SerializeField]
    float deathHeight;

    int currentScore;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        animationHandler = GetComponentInChildren<AnimationHandler>();
        gameManager = GameManagerScript.Instance;
        colorManager = ColorManager.Instance;
        UIManager.Instance.SetPlayerController(this);
        initialColorLinearValue = initialBackgroundColor.r;
        currentColorLinearValue = initialColorLinearValue;
        canRegenColor = true;
        isSliding = false;
        canDash = true;
        currentScore = 0;
        currentPlayerColor = Color.white;
        isGrounded = false;
    }
    void HandlePlayerColor()
    {
        currentPlayerColor = new Color(initialColorLinearValue - currentColorLinearValue, initialColorLinearValue - currentColorLinearValue, initialColorLinearValue - currentColorLinearValue);
        playerSpriteRenderer.color = currentPlayerColor;
    }

    void HandleJump()
    {
        if (canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            if (currentColorLinearValue > (jumpColorCost / 100f))
            {
                canJump = true;
            }
        }
    }
    void Jump()
    {
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        currentColorLinearValue -= jumpColorCost / 100;
        animationHandler.SetTrigger("Flap");

    }

    void HandleDash()
    {
        if (canDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dash();
            }
        }
        
    }

    void Dash()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector2.right * dashSpeed, ForceMode.Impulse);
        currentColorLinearValue -= dashColorCost / 100;
        canDash = false;
        StartCoroutine(DashCoolDown());
    }
    public void Heal(float healAmount)
    {
        currentColorLinearValue += healAmount / 100;
    }

    void CheckGround() 
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, checkDistance, whatIsGround);
        if(!isGrounded)
        {
            isSliding = false;
        }
    }
    private void Update()
    {
        CheckGround();

        if (isGrounded)
        {
            transform.Translate(Vector3.left * LevelManager.Instance.currentLevelSpeed * Time.deltaTime);
        }
        else
        {
            if (transform.right != new Vector3(1, 0, 0))
            {
                transform.right = Vector3.Lerp(transform.right,new Vector3(1,0,0), 0.5f);
            }
            if(transform.position.x < 0)
            {
                rb.AddForce(Vector3.right * 0.2f, ForceMode.Force);
            }
            
        }
        
        HandlePlayerColor();
        if(currentColorLinearValue >0f)
        {
            HandleJump();
            HandleDash();
        }
        else if(currentColorLinearValue <=0)
        {
            canRegenColor = false;
            canJump = false;
            Invoke("StartRegenColor", regenStartTime);
        }
        HandleRegenColor();
        colorManager.ChangeBackgroundColor(new Color(currentColorLinearValue, currentColorLinearValue, currentColorLinearValue));


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Death"))
        {
            HandleDeath();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            HandleDeath();
        }
        if (other.CompareTag("Point"))
        {
            other.GetComponent<LevelPart>();
            IncreaseScore();
            RegenColor(0.2f);
        }
    }
    void IncreaseScore()
    {
        currentScore += 1;
        UIManager.Instance.UpdateScore(currentScore);
    }
    
    void RegenColor(float colorAmount)
    {
        currentColorLinearValue += colorAmount;
        canRegenColor = true;
    }
    void HandleDeath()
    {
        gameManager.HandleGameOver();
        Destroy(gameObject);
    }
    void HandleSlide(Vector3 normal)
    {
        rb.AddForce(normal * slideSpeed,ForceMode.Impulse);

    }
    void HandleRegenColor()
    {
        if (canRegenColor)
        {
            currentColorLinearValue += colorRegenRate * Time.deltaTime;
        }
        currentColorLinearValue = Mathf.Clamp(currentColorLinearValue, 0f, 1f);
    }
    private void LateUpdate()
    {
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        animationHandler.SetBool("Grounded", isGrounded);
        animationHandler.SetBool("Sliding", isSliding);
    }
    void StartRegenColor()
    {
        canRegenColor = true;
        if (currentColorLinearValue <= 0f)
        {
            currentColorLinearValue = 0.001f;
        }
    }
    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        rb.useGravity = true;

    }

    public int GetScore()
    {
        return currentScore;
    }

}
