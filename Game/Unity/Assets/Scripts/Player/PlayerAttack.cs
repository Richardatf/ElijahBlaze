using ElijahBlaze.Core;
using ElijahBlaze.Enemies;
using UnityEngine;

namespace ElijahBlaze.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField, Min(0.5f)] private float range = 2.6f;
        [SerializeField, Min(0.25f)] private float radius = 1.65f;
        [SerializeField, Min(1)] private int damage = 1;
        [SerializeField, Min(0.1f)] private float cooldown = 0.45f;
        [SerializeField, Min(0.1f)] private float stunDuration = 0.85f;

        private float nextAttackTime;

        private void Update()
        {
            if (Time.time < nextAttackTime)
                return;

            // Holding either attack input repeats swings at the cooldown rate.
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
                Attack();
        }

        private void Attack()
        {
            nextAttackTime = Time.time + cooldown;

            // Keep the strike close to Elijah so it still catches a Shardling
            // that has rushed into melee range instead of passing behind the swing.
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 center = transform.position + forward * (range * 0.5f) + Vector3.up * 0.9f;
            Collider[] hits = Physics.OverlapSphere(center, radius);

            Health closestHealth = null;
            EnemyChase closestChase = null;
            float closestDistance = float.MaxValue;

            foreach (Collider hit in hits)
            {
                Health health = hit.GetComponentInParent<Health>();
                if (health == null || health.gameObject == gameObject || health.IsDead)
                    continue;

                Vector3 toTarget = health.transform.position - transform.position;
                toTarget.y = 0f;
                float distance = toTarget.magnitude;
                if (distance > range || distance < 0.01f)
                    continue;

                // A small overlap behind Elijah is allowed for enemies that slip
                // past his shoulder, while attacks still favor the direction he faces.
                if (Vector3.Dot(forward, toTarget / distance) < -0.2f)
                    continue;

                if (distance >= closestDistance)
                    continue;

                closestHealth = health;
                closestChase = health.GetComponent<EnemyChase>();
                if (closestChase == null)
                    closestChase = health.GetComponentInParent<EnemyChase>();
                closestDistance = distance;
            }

            if (closestHealth == null)
                return;

            closestHealth.TakeDamage(damage);
            if (closestChase != null)
                closestChase.Stun(stunDuration);
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 center = transform.position + forward * (range * 0.5f) + Vector3.up * 0.9f;
            Gizmos.DrawWireSphere(center, radius);
        }
    }
}
