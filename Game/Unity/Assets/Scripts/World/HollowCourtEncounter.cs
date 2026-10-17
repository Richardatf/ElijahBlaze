using System;
using System.Collections.Generic;
using ElijahBlaze.Core;
using UnityEngine;

namespace ElijahBlaze.World
{
    public class HollowCourtEncounter : MonoBehaviour
    {
        [SerializeField] private List<Health> encounterEnemies = new List<Health>();

        public event Action EncounterCleared;
        public bool IsCleared { get; private set; }

        private void OnEnable()
        {
            foreach (Health enemy in encounterEnemies)
            {
                if (enemy != null)
                    enemy.Died += CheckEncounter;
            }
        }

        private void Start()
        {
            CheckEncounter();
        }

        private void OnDisable()
        {
            foreach (Health enemy in encounterEnemies)
            {
                if (enemy != null)
                    enemy.Died -= CheckEncounter;
            }
        }

        private void CheckEncounter()
        {
            if (IsCleared)
                return;

            foreach (Health enemy in encounterEnemies)
            {
                if (enemy != null && !enemy.IsDead)
                    return;
            }

            IsCleared = true;
            Debug.Log("Hollow Court encounter cleared.", this);
            EncounterCleared?.Invoke();
        }
    }
}
