using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CharacterEventSO healthEvent;

    public PlayerStatBar playerStatBar;
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var characterCurrentHealth = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(characterCurrentHealth); 
    }
}
