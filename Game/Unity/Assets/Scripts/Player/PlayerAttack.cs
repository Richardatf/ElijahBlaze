using System.Collections;
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
        private LineRenderer slashLine;
        private Coroutine slashRoutine;

        private void Awake()
        {
            CreateSlashVisual();
        }

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
            ShowSlash();

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

            EnemyHitFeedback feedback = closestHealth.GetComponent<EnemyHitFeedback>();
            if (feedback == null)
                feedback = closestHealth.GetComponentInParent<EnemyHitFeedback>();
            if (feedback != null)
                feedback.ShowStunned(stunDuration);
        }

        private void CreateSlashVisual()
        {
            GameObject slashObject = new GameObject("Elijah_Attack_Slash");
            slashObject.transform.SetParent(transform, false);

            slashLine = slashObject.AddComponent<LineRenderer>();
            slashLine.useWorldSpace = true;
            slashLine.loop = false;
            slashLine.positionCount = 9;
            slashLine.startWidth = 0.14f;
            slashLine.endWidth = 0.045f;
            slashLine.numCapVertices = 4;
            slashLine.numCornerVertices = 4;
            slashLine.sharedMaterial = CreateEffectMaterial();
            slashLine.startColor = new Color(0.9f, 0.98f, 1f, 1f);
            slashLine.endColor = new Color(0.22f, 0.72f, 1f, 1f);
            slashLine.enabled = false;
        }

        private void ShowSlash()
        {
            if (slashRoutine != null)
                StopCoroutine(slashRoutine);

            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
            Vector3 center = transform.position + Vector3.up * 1.05f + forward * 0.35f;
            float arcRadius = Mathf.Min(range * 0.52f, 1.4f);

            for (int i = 0; i < slashLine.positionCount; i++)
            {
                float t = i / (float)(slashLine.positionCount - 1);
                float angle = Mathf.Lerp(-1.05f, 1.05f, t);
                Vector3 point = center
                    + forward * (Mathf.Cos(angle) * arcRadius)
                    + right * (Mathf.Sin(angle) * arcRadius);
                point.y += Mathf.Sin(t * Mathf.PI) * 0.12f;
                slashLine.SetPosition(i, point);
            }

            slashLine.enabled = true;
            slashRoutine = StartCoroutine(HideSlashAfter(0.18f));
        }

        private IEnumerator HideSlashAfter(float duration)
        {
            yield return new WaitForSeconds(duration);
            if (slashLine != null)
                slashLine.enabled = false;
            slashRoutine = null;
        }

        private static Material CreateEffectMaterial()
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            return new Material(shader) { color = Color.white };
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
