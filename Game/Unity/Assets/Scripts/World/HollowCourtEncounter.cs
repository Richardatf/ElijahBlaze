using System;
using System.Collections;
using System.Collections.Generic;
using ElijahBlaze.Core;
using ElijahBlaze.Enemies;
using UnityEngine;

namespace ElijahBlaze.World
{
    public class HollowCourtEncounter : MonoBehaviour
    {
        [SerializeField] private List<Health> encounterEnemies = new List<Health>();
        [SerializeField] private Transform player;
        [SerializeField] private Transform awakeningSeal;
        [SerializeField] private Transform exitMarker;
        [SerializeField, Min(0.5f)] private float activationRadius = 3f;
        [SerializeField, Min(0.5f)] private float completionRadius = 2.2f;
        [SerializeField, Min(0f)] private float enemyWakeInterval = 1.15f;
        [SerializeField, Min(0f)] private float gateOpeningGrace = 1.8f;

        private float exitAvailableAt;
        private Coroutine wakeRoutine;

        public event Action EncounterStarted;
        public event Action<int, int> ProgressChanged;
        public event Action EncounterCleared;
        public event Action LevelCompleted;

        public bool IsActive { get; private set; }
        public bool IsCleared { get; private set; }
        public bool IsComplete { get; private set; }
        public int TotalCount => encounterEnemies.Count;
        public int RemainingCount
        {
            get
            {
                int remaining = 0;
                foreach (Health enemy in encounterEnemies)
                {
                    if (enemy != null && !enemy.IsDead)
                        remaining++;
                }
                return remaining;
            }
        }

        public void Configure(
            IEnumerable<Health> enemies,
            Transform playerTransform,
            Transform sealTransform,
            Transform levelExit)
        {
            encounterEnemies = new List<Health>(enemies);
            player = playerTransform;
            awakeningSeal = sealTransform;
            exitMarker = levelExit;

            // They are visible in the court, but stay dormant until Elijah reaches
            // its central seal.
            foreach (Health enemy in encounterEnemies)
                SetEnemyAwake(enemy, false);
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Start()
        {
            PublishProgress();
        }

        private void Update()
        {
            if (player == null)
                return;

            if (!IsActive && !IsCleared && awakeningSeal != null
                && HorizontalDistance(player.position, awakeningSeal.position) <= activationRadius)
            {
                BeginEncounter();
                return;
            }

            if (IsCleared && !IsComplete && Time.time >= exitAvailableAt && exitMarker != null
                && HorizontalDistance(player.position, exitMarker.position) <= completionRadius)
            {
                CompleteLevel();
            }
        }

        private void OnDisable()
        {
            Unsubscribe();
            if (wakeRoutine != null)
                StopCoroutine(wakeRoutine);
        }

        private void Subscribe()
        {
            foreach (Health enemy in encounterEnemies)
            {
                if (enemy != null)
                    enemy.Died += HandleEnemyDied;
            }
        }

        private void Unsubscribe()
        {
            foreach (Health enemy in encounterEnemies)
            {
                if (enemy != null)
                    enemy.Died -= HandleEnemyDied;
            }
        }

        private void BeginEncounter()
        {
            if (IsActive || IsCleared || RemainingCount == 0)
                return;

            IsActive = true;
            EncounterStarted?.Invoke();
            PublishProgress();
            wakeRoutine = StartCoroutine(WakeShardlingsInSequence());
        }

        private IEnumerator WakeShardlingsInSequence()
        {
            foreach (Health enemy in encounterEnemies)
            {
                if (enemy != null && !enemy.IsDead)
                {
                    SetEnemyAwake(enemy, true);
                    yield return new WaitForSeconds(enemyWakeInterval);
                }
            }

            wakeRoutine = null;
        }

        private static void SetEnemyAwake(Health enemy, bool awake)
        {
            if (enemy == null)
                return;

            EnemyChase chase = enemy.GetComponent<EnemyChase>();
            if (chase != null)
                chase.enabled = awake;

            EnemyAttack attack = enemy.GetComponent<EnemyAttack>();
            if (attack != null)
                attack.enabled = awake;
        }

        private void HandleEnemyDied()
        {
            if (!IsActive || IsCleared)
                return;

            PublishProgress();
            if (RemainingCount > 0)
                return;

            IsCleared = true;
            exitAvailableAt = Time.time + gateOpeningGrace;
            if (wakeRoutine != null)
            {
                StopCoroutine(wakeRoutine);
                wakeRoutine = null;
            }

            EncounterCleared?.Invoke();
        }

        private void CompleteLevel()
        {
            if (IsComplete)
                return;

            IsComplete = true;
            LevelCompleted?.Invoke();
        }

        private void PublishProgress()
        {
            ProgressChanged?.Invoke(TotalCount - RemainingCount, TotalCount);
        }

        private static float HorizontalDistance(Vector3 first, Vector3 second)
        {
            first.y = 0f;
            second.y = 0f;
            return Vector3.Distance(first, second);
        }
    }
}
