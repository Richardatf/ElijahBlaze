using System;
using UnityEngine;

namespace ElijahBlaze.Core
{
    public interface IDamageInterceptor
    {
        bool TryInterceptDamage(int amount, Transform attacker);
    }

    public class Health : MonoBehaviour
    {
        [SerializeField, Min(1)] private int maxHealth = 3;

        public int CurrentHealth { get; private set; }
        public int MaxHealth => maxHealth;
        public bool IsDead { get; private set; }

        public event Action<int, int> Damaged;
        public event Action Died;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            TakeDamage(amount, null);
        }

        public void TakeDamage(int amount, Transform attacker)
        {
            if (IsDead || amount <= 0)
                return;

            foreach (MonoBehaviour behaviour in GetComponents<MonoBehaviour>())
            {
                if (behaviour is IDamageInterceptor interceptor && interceptor.TryInterceptDamage(amount, attacker))
                {
                    Debug.Log(gameObject.name + " blocked an incoming hit.", this);
                    return;
                }
            }

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            Damaged?.Invoke(CurrentHealth, maxHealth);

            if (CurrentHealth == 0)
                Die();
        }

        public void RestoreToFull()
        {
            if (IsDead)
                return;

            CurrentHealth = maxHealth;
        }

        private void Die()
        {
            if (IsDead)
                return;

            IsDead = true;
            bool hasDeathHandler = Died != null;
            Died?.Invoke();
            Debug.Log($"{gameObject.name} died.", this);

            // DeathFeedback handles the enemy's visible death sequence. The player
            // has a PlayerDeathController subscriber that restarts the scene. If
            // neither handler is present, remove this object as a safe fallback.
            if (!hasDeathHandler)
                Destroy(gameObject);
        }
    }
}
