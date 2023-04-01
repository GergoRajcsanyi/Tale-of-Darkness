using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public new ParticleSystem particleSystem;
    public Animator animator;
    public Rigidbody2D rb;
    private float horizontalVelocity;

    void Awake()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        isPlayerOnGround();
        Move();
        Jump();
        Crouch();
    }

    void Update()
    {
        animator.SetBool("Jump", !isGrounded);
        animator.SetFloat("Vertical", rb.velocity.y);

        CheckLand();
        CheckAirTime();
    }

    #region Horizontal Movement

    [Header("HORIZONTAL")]
    [Range(0, 1)] public float dampingBasic = 0.5f;
    [Range(0, 1)] public float dampingStopping = 0.5f;
    [Range(0, 1)] public float dampingTurning = 0.5f;
    [Range(1f, 1.5f)] public float movementBoost = 1.05f;
    private bool facingRight = true;

    private void Move()
    {
        horizontalVelocity = rb.velocity.x;
        horizontalVelocity += joystick.Horizontal;

        if (isCrouched && !Abilities.isCorruptionActive) 
            horizontalVelocity *= crouchSpeed;

        if (Abilities.isCorruptionActive) 
            horizontalVelocity *= movementBoost;

        if (Mathf.Abs(joystick.Horizontal) < 0.01f)
        {
            horizontalVelocity *= Mathf.Pow(1f - dampingStopping, Time.deltaTime * 10f);
        }
        else if (Mathf.Sign(joystick.Horizontal) != Mathf.Sign(horizontalVelocity))
        {
            horizontalVelocity *= Mathf.Pow(1f - dampingTurning, Time.deltaTime * 10f);
        }
        else
        {
            horizontalVelocity *= Mathf.Pow(1f - dampingBasic, Time.deltaTime * 10f);
        }
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);

        if (facingRight == false && horizontalVelocity >= 0.1f)
        {
            Flip();
        }
        else if (facingRight == true && horizontalVelocity <= -0.1f)
        {
            Flip();
        }

        animator.SetFloat("Velocity", Mathf.Abs(horizontalVelocity));
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    #endregion

    #region Ground & Landing

    [Header("GROUND")] 
    public float checkRadius;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundedRemember = 0;
    public float groundedRememberTime = 0.25f;
    private float airTime = 0f; 

    private void isPlayerOnGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void CheckAirTime()
    {
        if (isGrounded) airTime = 0f;
        else airTime += Time.deltaTime;
    }

    private void CheckLand()
    {
        if (airTime > 0)
        {
            if (isGrounded)
            {
                CreateDust();
                FindObjectOfType<AudioManager>().Play("Landing");
            } 
        }
    }

    private void CreateDust()
    {
        particleSystem.Play();
    }
    #endregion

    #region Jump

    [Header("JUMP")]
    [Range(0f, 50f)] public float jumpForce;
    public float fallMultiplier = 2.5f;
    private int extraJumps;
    public int extraJumpsValue;

    private void Jump()
    {
        float vercticalMove = joystick.Vertical;

        if (crouchDisableCollider.enabled && !Abilities.isCorruptionActive)
        {
            groundedRemember -= Time.deltaTime;
            if (isGrounded)
            {
                groundedRemember = groundedRememberTime;
                extraJumps = extraJumpsValue;
            }

            if (vercticalMove >= 0.25f && extraJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                extraJumps--;
                animator.SetBool("Jump", true);
                FindObjectOfType<AudioManager>().Play("Jump");
            }
            else if (vercticalMove >= 0.25f && extraJumps == 0 && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("Jump", true);
                FindObjectOfType<AudioManager>().Play("Jump");
                CreateDust();
            }
            else if (vercticalMove >= 0.25f && groundedRemember > 0)
            {
                groundedRemember = 0;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("Jump", true);
                FindObjectOfType<AudioManager>().Play("Jump");
                CreateDust();
            }

            //fall multiplier
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    #endregion

    #region Crouch

    [Header("CROUCH")]
    [Range(0.9f, 1f)] public float crouchSpeed = 0.9f;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Collider2D crouchDisableCollider;
    [SerializeField] private Collider2D crouchEnableCollider;
    const float ceilingRadius = 0.2f;
    private bool isCrouched = false;

    private void Crouch()
    {
        float vercticalMove = joystick.Vertical;

        if (vercticalMove <= -0.5f && isGrounded)
        {
            if (crouchDisableCollider != null && crouchEnableCollider != null)
            {
                crouchDisableCollider.enabled = false;
                crouchEnableCollider.enabled = true;
            } 
            isCrouched = true;
            animator.SetBool("Crouch", true);
        }
        else 
        {
            if (crouchDisableCollider != null && crouchEnableCollider != null)
            {
                crouchDisableCollider.enabled = true;
                crouchEnableCollider.enabled = false;
            } 
            isCrouched = false;
            animator.SetBool("Crouch", false);
        }

        if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
        {
            animator.SetBool("Crouch", true);
            isCrouched = true;
            if (crouchDisableCollider != null && crouchEnableCollider != null)
            {
                crouchDisableCollider.enabled = false;
                crouchEnableCollider.enabled = true;
            }
        } 
    }
    #endregion

}