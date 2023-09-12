using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        _currentEnemy = enemy;
        _currentEnemy.currentSpeed = _currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        if (_currentEnemy.FoundPlayer())
        {
            _currentEnemy.SwitchState(NPCState.Chase);
        }
        
         if (!_currentEnemy._physicsCheck.isGround 
             || (_currentEnemy._physicsCheck.touchLeftWall && _currentEnemy._faceDir.x < 0)
             || (_currentEnemy._physicsCheck.touchRightWall  && _currentEnemy._faceDir.x > 0))
        {
            _currentEnemy.wait = true;
            _currentEnemy._animator.SetBool("walk", false);
        }
         else
         {
             _currentEnemy._animator.SetBool("walk", true);
         }
    }

    public override void PhysicUpdate()
    {
        
    }

    public override void OnExist()
    {
        Debug.Log("exit patrol state");
    }
}
