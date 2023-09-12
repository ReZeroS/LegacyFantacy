using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rb;

    public Animator _animator;

    public PhysicsCheck _physicsCheck;

    [Header("基本参数")] public float normalSpeed;

    public float chaseSpeed;

    public float currentSpeed;

    public float hurtForce;

    public Vector3 _faceDir;

    public Transform attacker;

    [Header("检测")] public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;
    
    
    [Header("计时器")] 
    public float waitTime;
    private float waitTimeCounter;
    public bool wait;


    public float lostTime;
    public float lostTimeCounter;
    

    [Header("状态")] 
    public bool isHurt;
    public bool isDead;

    private BaseState currentState; 
    
    public BaseState patrolState;

    public BaseState chaseState;
    
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        currentSpeed = normalSpeed;
        _physicsCheck = GetComponent<PhysicsCheck>();
        waitTimeCounter = waitTime;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }


    // Update is called once per frame
    void Update()
    {
        _faceDir = new Vector3(-transform.localScale.x, 0, 0);
        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead && !wait)
        {
            Move();
        }
        currentState.PhysicUpdate();
    }

    protected void Move()
    {
        _rb.velocity = new Vector2(currentSpeed * _faceDir.x, _rb.velocity.y);
    }

    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(_faceDir.x, 1, 1);
            }
          
        }

        if (!FoundPlayer())
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else
        {
            lostTimeCounter = lostTime;
        }
        
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, _faceDir, checkDistance, attackLayer);
    }

    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null,
        };
        currentState.OnExist();
        currentState = newState;
        currentState.OnEnter(this);
    }

    

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        if (attackTrans.transform.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        if (attackTrans.transform.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3( 1, 1, 1);
        }


        isHurt = true;
        _animator.SetTrigger("hurt");

        Vector2 dir = new Vector2(transform.position.x - attackTrans.transform.position.x, 0).normalized;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }


    private IEnumerator OnHurt(Vector2 dir)
    {
        _rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }


    public void OnDead()
    {
        gameObject.layer = 2;
        isDead = true;
        // 先后
        _animator.SetBool("dead", true);
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject); 
    }

    private void OnDisable()
    {
        currentState.OnExist();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3( checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}