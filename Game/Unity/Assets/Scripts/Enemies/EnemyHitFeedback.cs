using System.Collections;
using ElijahBlaze.Core;
using UnityEngine;

namespace ElijahBlaze.Enemies
{
    [RequireComponent(typeof(Health))]
    public class EnemyHitFeedback : MonoBehaviour
    {
        [SerializeField, Min(0.03f)] private float flashDuration = 0.12f;
        [SerializeField] private Color hitColor = new Color(0.75f, 0.9f, 1f, 1f);

        private Health health;
        private Renderer[] renderers;
        private MaterialPropertyBlock propertyBlock;
        private Coroutine flashRoutine;

        private void Awake()
        {
            health = GetComponent<Health>();
            renderers = GetComponentsInChildren<Renderer>();
            propertyBlock = new MaterialPropertyBlock();
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
