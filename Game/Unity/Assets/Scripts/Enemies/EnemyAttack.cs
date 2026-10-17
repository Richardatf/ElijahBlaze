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
            health.TakeDamage(damage);
        }
    }
}
