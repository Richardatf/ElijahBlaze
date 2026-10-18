using ElijahBlaze.Core;
using UnityEngine;

namespace ElijahBlaze.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float range = 2.2f;
        [SerializeField] private float radius = 1.15f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float cooldown = 0.5f;
        [SerializeField, Min(0.1f)] private float stunDuration = 0.85f;

        private float nextAttackTime;

        private void Update()
        {
            if (Time.time < nextAttackTime)
                return;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                Attack();
        }

        private void Attack()
        {
            nextAttackTime = Time.time + cooldown;
            Vector3 center = transform.position + transform.forward * range + Vector3.up * 0.85f;
            Collider[] hits = Physics.OverlapSphere(center, radius);

            foreach (Collider hit in hits)
            {
                Health health = hit.GetComponentInParent<Health>();
                if (health == null || health.gameObject == gameObject || health.IsDead)
                    continue;

                health.TakeDamage(damage);
                EnemyChase chase = health.GetComponent<EnemyChase>();
                if (chase != null)
                    chase.Stun(stunDuration);
                break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + transform.forward * range + Vector3.up * 0.85f, radius);
        }
    }
}
