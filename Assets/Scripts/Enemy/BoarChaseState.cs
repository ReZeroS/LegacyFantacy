using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        _currentEnemy = enemy;
        _currentEnemy.currentSpeed = _currentEnemy.chaseSpeed;
        _currentEnemy._animator.SetBool("run", true);
    }

    public override void LogicUpdate()
    {
        if (!_currentEnemy._physicsCheck.isGround 
            || (_currentEnemy._physicsCheck.touchLeftWall && _currentEnemy._faceDir.x < 0)
            || (_currentEnemy._physicsCheck.touchRightWall  && _currentEnemy._faceDir.x > 0))
        {
            _currentEnemy.transform.localScale = new Vector3(_currentEnemy._faceDir.x, 1, 1);
        }

        if (_currentEnemy.lostTimeCounter <= 0)
        {
            _currentEnemy.SwitchState(NPCState.Patrol);    
        }
    }

    public override void PhysicUpdate()
    {
        
    }

    public override void OnExist()
    {
        _currentEnemy.lostTimeCounter = _currentEnemy.lostTime;
        _currentEnemy._animator.SetBool("run", false);
    }
}
