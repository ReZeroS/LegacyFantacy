using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private PhysicsCheck _physicsCheck;
    private PlayerController _playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _physicsCheck = GetComponent<PhysicsCheck>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        _animator.SetFloat("velocityX", Mathf.Abs(_rigidbody2D.velocity.x));
        _animator.SetFloat("velocityY", _rigidbody2D.velocity.y );
        _animator.SetBool("isGround", _physicsCheck.isGround);
        _animator.SetBool("isDead", _playerController.isDead);
        _animator.SetBool("isAttack", _playerController.isAttack);
    }

    public void PlayHurt()
    { 
        _animator.SetTrigger("hurt");
    }
    
    public void PlayerAttack()
    {
        _animator.SetTrigger("attack");
    }
    
    
}
