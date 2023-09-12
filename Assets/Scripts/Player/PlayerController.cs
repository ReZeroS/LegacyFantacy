using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    
    
    private Rigidbody2D rb;
    
    public PlayerInputControl inputControl;

    private SpriteRenderer spriteRenderer;

    private PhysicsCheck physicsCheck;

    private PlayerAnimation playerAnimation;

    public CapsuleCollider2D _collider;

    public Vector2 inputDirection;

    public PhysicsMaterial2D wall;

    public PhysicsMaterial2D normal;


    [Header("Normal Config")] public float speed;

    public float jumpForce;

    private bool isDashing;


    public int jumpTimes;

    public int canJumpTimes;

    public bool isHurt;


    public bool isDead;

    public bool isAttack;

    public float hurtForce;

    private void Awake()
    {
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        _collider = GetComponent<CapsuleCollider2D>();
        inputControl.GamePlay.Jump.started += JumpStart;
        // inputControl.GamePlay.Jump.performed += JumpPerformed;
        inputControl.GamePlay.Dash.started += DashStart;

        // 攻击
        inputControl.GamePlay.Attack.started += PlayerAttack;
        canJumpTimes = 2;
        jumpTimes = canJumpTimes;
    }


    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlayerAttack();
        isAttack = true;
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }


    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
        CheckState();
    }


    private void FixedUpdate()
    {
        if (!isHurt && !isAttack && !isDashing)
        {
            Move();
        }
    }


    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed, rb.velocity.y);
        // flip the player
        if (inputDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void JumpStart(InputAction.CallbackContext context)
    {
        Debug.Log("jump started");
        if (!physicsCheck.isGround)
        {
            jumpTimes -= 1;
            return;
        }

        jumpTimes = 2;
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        // if (jumpTimes < 0)
        // {
        //     return;
        // }
        // rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    IEnumerator DashCoroutine(int dashDuration)
    {
        if (isDashing)
            yield break;
        Debug.Log("dash started");
        isDashing = true;
        rb.velocity = new Vector2(jumpForce, 0);
        yield return new WaitForSeconds(dashDuration);

        rb.velocity = new Vector2(0, 0);

        isDashing = false;
    }

    private void DashStart(InputAction.CallbackContext obj)
    {
        StartCoroutine(DashCoroutine(2));
    }

    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }

    public void CheckState()
    {
        _collider.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
}