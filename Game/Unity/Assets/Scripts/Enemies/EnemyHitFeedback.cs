using System.Collections;
using ElijahBlaze.Core;
using UnityEngine;

namespace ElijahBlaze.Enemies
{
    [RequireComponent(typeof(Health))]
    public class EnemyHitFeedback : MonoBehaviour
    {
        [SerializeField, Min(0.03f)] private float flashDuration = 0.18f;
        [SerializeField] private Color hitColor = new Color(0.75f, 0.9f, 1f, 1f);

        private Health health;
        private Renderer[] renderers;
        private MaterialPropertyBlock propertyBlock;
        private Coroutine flashRoutine;
        private GameObject stunRing;
        private float stunnedUntil;

        private void Awake()
        {
            health = GetComponent<Health>();
            renderers = GetComponentsInChildren<Renderer>();
            propertyBlock = new MaterialPropertyBlock();
            CreateStunRing();
        }

        private void OnEnable()
        {
            health.Damaged += HandleDamaged;
        }

        private void OnDisable()
        {
            health.Damaged -= HandleDamaged;
            if (flashRoutine != null)
                StopCoroutine(flashRoutine);
            ClearFlash();

            if (stunRing != null)
                stunRing.SetActive(false);
        }

        private void Update()
        {
            bool stunned = Time.time < stunnedUntil;
            if (stunRing != null && stunRing.activeSelf != stunned)
                stunRing.SetActive(stunned);
        }

        public void ShowStunned(float duration)
        {
            stunnedUntil = Mathf.Max(stunnedUntil, Time.time + duration);
            if (stunRing != null)
                stunRing.SetActive(true);
        }

        private void HandleDamaged(int currentHealth, int maxHealth)
        {
            if (flashRoutine != null)
                StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer == null)
                    continue;
                renderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor("_Color", hitColor);
                propertyBlock.SetColor("_BaseColor", hitColor);
                renderer.SetPropertyBlock(propertyBlock);
            }

            yield return new WaitForSeconds(flashDuration);
            ClearFlash();
            flashRoutine = null;
        }

        private void CreateStunRing()
        {
            stunRing = new GameObject("Stun_Indicator_Ring");
            stunRing.transform.SetParent(transform, false);
            stunRing.transform.localPosition = new Vector3(0f, -0.69f, 0f);

            LineRenderer ring = stunRing.AddComponent<LineRenderer>();
            ring.useWorldSpace = false;
            ring.loop = true;
            ring.positionCount = 32;
            ring.startWidth = 0.075f;
            ring.endWidth = 0.075f;
            ring.numCapVertices = 4;
            ring.sharedMaterial = CreateEffectMaterial();
            ring.startColor = new Color(0.18f, 0.92f, 1f, 1f);
            ring.endColor = new Color(0.18f, 0.92f, 1f, 1f);

            const float ringRadius = 0.82f;
            for (int i = 0; i < ring.positionCount; i++)
            {
                float angle = Mathf.PI * 2f * i / ring.positionCount;
                ring.SetPosition(i, new Vector3(Mathf.Cos(angle) * ringRadius, 0.025f, Mathf.Sin(angle) * ringRadius));
            }

            stunRing.SetActive(false);
        }

        private static Material CreateEffectMaterial()
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            return new Material(shader) { color = Color.white };
        }

        private void ClearFlash()
        {
            if (renderers == null)
                return;

            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                    renderer.SetPropertyBlock(null);
            }
        }
    }
}
