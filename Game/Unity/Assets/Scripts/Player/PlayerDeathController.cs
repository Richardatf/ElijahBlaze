using System.Collections;
using ElijahBlaze.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElijahBlaze.Player
{
    [RequireComponent(typeof(Health))]
    public class PlayerDeathController : MonoBehaviour
    {
        [SerializeField] private float restartDelay = 1.5f;
        private Health health;
        private bool restarting;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.Died += HandleDeath;
        }

        private void OnDisable()
        {
            health.Died -= HandleDeath;
        }

        private void HandleDeath()
        {
            if (restarting)
                return;

            restarting = true;
            foreach (MonoBehaviour behaviour in GetComponents<MonoBehaviour>())
            {
                if (behaviour != this && behaviour != health)
                    behaviour.enabled = false;
            }

            StartCoroutine(RestartRoutine());
        }

        private IEnumerator RestartRoutine()
        {
            yield return new WaitForSeconds(restartDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
