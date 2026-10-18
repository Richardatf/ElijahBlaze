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
        [SerializeField, Min(0.1f)] private float windup = 0.42f;

        private float nextAttackTime;
        private float attackAt;
        private float strikeFlashUntil;
        private bool windingUp;
        private EnemyChase chase;
        private LineRenderer attackTell;

        public void SetTarget(Transform attackTarget) => target = attackTarget;

        private void Awake()
        {
            chase = GetComponent<EnemyChase>();
            CreateAttackTell();
        }

        public void Configure(float distance, float attackCooldown, int damageAmount)
        {
            attackDistance = distance;
            cooldown = attackCooldown;
            damage = damageAmount;
        }

        private void Update()
        {
            if (target == null)
            {
                CancelTell();
                return;
            }

            if (chase != null && chase.IsStunned)
            {
                CancelTell();
                return;
            }

            if (Vector3.Distance(transform.position, target.position) > attackDistance)
            {
                CancelTell();
                return;
            }

            if (Time.time < strikeFlashUntil)
            {
                UpdateTell(true);
                return;
            }

            if (windingUp)
            {
                UpdateTell(false);
                if (Time.time < attackAt)
                    return;

                Health health = target.GetComponent<Health>();
                if (health != null && !health.IsDead)
                    health.TakeDamage(damage, transform);

                windingUp = false;
                strikeFlashUntil = Time.time + 0.16f;
                UpdateTell(true);
                return;
            }

            if (Time.time < nextAttackTime)
                return;

            FaceTarget();
            windingUp = true;
            attackAt = Time.time + windup;
            nextAttackTime = Time.time + cooldown;
            UpdateTell(false);
        }

        private void CreateAttackTell()
        {
            GameObject tellObject = new GameObject("Shardling_Attack_Tell");
            tellObject.transform.SetParent(transform, false);

            attackTell = tellObject.AddComponent<LineRenderer>();
            attackTell.useWorldSpace = true;
            attackTell.positionCount = 2;
            attackTell.startWidth = 0.055f;
            attackTell.endWidth = 0.055f;
            attackTell.numCapVertices = 4;
            attackTell.sharedMaterial = CreateEffectMaterial();
            attackTell.enabled = false;
        }

        private void UpdateTell(bool striking)
        {
            if (attackTell == null || target == null)
                return;

            Vector3 start = transform.position + Vector3.up * 0.78f;
            Vector3 end = target.position + Vector3.up * 0.9f;
            attackTell.SetPosition(0, start);
            attackTell.SetPosition(1, end);

            Color color = striking
                ? new Color(1f, 0.82f, 0.2f, 1f)
                : new Color(1f, 0.12f, 0.035f, 1f);
            attackTell.startColor = color;
            attackTell.endColor = color;
            attackTell.startWidth = striking ? 0.19f : 0.055f;
            attackTell.endWidth = striking ? 0.19f : 0.055f;
            attackTell.enabled = true;
        }

        private void FaceTarget()
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }

        private void CancelTell()
        {
            windingUp = false;
            if (attackTell != null)
                attackTell.enabled = false;
        }

        private static Material CreateEffectMaterial()
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            return new Material(shader) { color = Color.white };
        }

        private void OnDisable()
        {
            if (attackTell != null)
                attackTell.enabled = false;
        }
    }
}
