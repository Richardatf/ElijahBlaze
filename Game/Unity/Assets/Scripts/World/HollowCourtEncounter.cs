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

        public void Configure(IEnumerable<Health> enemies)
        {
            encounterEnemies = new List<Health>(enemies);
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Start()
        {
            CheckEncounter();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            foreach (Health enemy in encounterEnemies)
            {
                if (enemy != null)
                    enemy.Died += CheckEncounter;
            }
        }

        private void Unsubscribe()
        {
            foreach (Health enemy in encounterEnemies)
            {
                if (enemy != null)
                    enemy.Died -= CheckEncounter;
            }
        }

        private void CheckEncounter()
        {
            if (IsCleared || encounterEnemies.Count == 0)
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
