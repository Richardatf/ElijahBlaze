using ElijahBlaze.Core;
using UnityEngine;

namespace ElijahBlaze.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private float cooldown = 1.1f;
        [SerializeField] private int damage = 1;

        private float nextAttackTime;

        public void SetTarget(Transform attackTarget) => target = attackTarget;

        public void Configure(float distance, float attackCooldown, int damageAmount)
        {
            attackDistance = distance;
            cooldown = attackCooldown;
            damage = damageAmount;
        }

        private void Update()
        {
            if (target == null || Time.time < nextAttackTime)
                return;

            if (Vector3.Distance(transform.position, target.position) > attackDistance)
                return;

            Health health = target.GetComponent<Health>();
            if (health == null || health.IsDead)
                return;

            nextAttackTime = Time.time + cooldown;
            health.TakeDamage(damage, transform);
        }
    }
}
