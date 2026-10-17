using System.Collections;
using ElijahBlaze.Core;
using UnityEngine;

namespace ElijahBlaze.Combat
{
    [RequireComponent(typeof(Health))]
    public class DeathFeedback : MonoBehaviour
    {
        [SerializeField, Min(0.05f)] private float duration = 0.45f;
        [SerializeField] private bool destroyAfterFeedback = true;
        [SerializeField] private ParticleSystem deathParticles;
        [SerializeField] private AudioSource deathAudio;

        private Health health;
        private bool playing;
        private Vector3 startingScale;

        private void Awake()
        {
            health = GetComponent<Health>();
            startingScale = transform.localScale;
        }

        private void OnEnable()
        {
            health.Died += PlayDeath;
        }

        private void OnDisable()
        {
            health.Died -= PlayDeath;
        }

        public void PlayDeath()
        {
            if (playing)
                return;

            playing = true;
            DisableGameplayComponents();

            if (deathParticles != null)
                deathParticles.Play();

            if (deathAudio != null)
                deathAudio.Play();

            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localScale = Vector3.Lerp(startingScale, Vector3.zero, t);
                yield return null;
            }

            if (destroyAfterFeedback)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }

        private void DisableGameplayComponents()
        {
            foreach (Collider colliderComponent in GetComponentsInChildren<Collider>())
                colliderComponent.enabled = false;

            foreach (MonoBehaviour behaviour in GetComponents<MonoBehaviour>())
            {
                if (behaviour == this || behaviour == health)
                    continue;

                behaviour.enabled = false;
            }
        }
    }
}
