using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    
    [Header("Attributes")]
    public float maxHealth;
    
    public float currentHealth;

    [Header("无敌时间")] 
    public float invulnerableDuration;

    private float invulnerableCounter;

    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamege;
    public UnityEvent OnDie;
    
    
   

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    }
    
    
    public void TakeDamage(Attack attack)
    {
        if (invulnerable) 
        {
            return;
        }

        if (currentHealth - attack.damage > 0)
        {
            currentHealth -= attack.damage;
            TriggerInvulnerable();
            // 执行受伤
            OnTakeDamege?.Invoke(attack.transform);
        }
        else
        {
            currentHealth = 0;
            // 执行死亡
            OnDie?.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }


    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
        
    }
}
