﻿using System;
using Mirror;
using UnityEngine;

namespace IntelligentCake.Player
{
    public class Player : NetworkBehaviour
    {
        [SyncVar]
        private bool _isDead = false;
        public bool isDead
        {
            get => _isDead;
            protected set => _isDead = value;
        }

        [SerializeField]
        private int maxHealth = 100;
        
        [SyncVar]
        public float currentHealth;

        [SerializeField]
        private Behaviour[] disableOnDeath;
        private bool[] _wasEnabled;
        public void Setup()
        {
            _wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < _wasEnabled.Length; i++)
            {
                _wasEnabled[i] = disableOnDeath[i].enabled;
            }
            
            SetDefaults();
        }

        private void Update()
        {
            if (!isLocalPlayer)
                    return;
            
            if (Input.GetKeyDown(KeyCode.K))
                RpcTakeDamage(9999);
        }

        [ClientRpc]
        public void RpcTakeDamage(int amount)
        {
            if(isDead)
                return;
            
            currentHealth -= amount;
            
            Debug.Log(transform.name + " now has " + currentHealth + " health.");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;

            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = false;
            }
            
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = true;
            
            Debug.Log(transform.name + " is DEAD!");
            
            // TODO CALL RESPAWN METHOD
        }

        public void SetDefaults()
        {
            isDead = false;
            
            currentHealth = maxHealth;
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = _wasEnabled[i];
            }

            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = true;
        }
    }
}