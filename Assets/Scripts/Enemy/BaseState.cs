using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseState
{

   protected Enemy _currentEnemy;
   public abstract void OnEnter(Enemy enemy);
   
   public abstract void LogicUpdate();
   
   public abstract void PhysicUpdate();
   
   public abstract void OnExist();
}
