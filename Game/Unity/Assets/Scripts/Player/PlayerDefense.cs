using ElijahBlaze.Core;
using UnityEngine;

namespace ElijahBlaze.Player
{
    [RequireComponent(typeof(Health))]
    public class PlayerDefense : MonoBehaviour, IDamageInterceptor
    {
        [SerializeField, Min(0.05f)] private float recoveryAfterBlock = 0.38f;
        [SerializeField, Range(-1f, 1f)] private float minimumFacingDot = 0.2f;

        private GameObject guardVisual;
        private float guardAvailableAt;

        public bool IsGuarding
        {
            get
            {
                bool held = Input.GetKey(KeyCode.LeftShift)
                    || Input.GetKey(KeyCode.RightShift)
                    || Input.GetMouseButton(1);
                return held && Time.time >= guardAvailableAt;
            }
        }

        public void Configure(GameObject visual)
        {
            guardVisual = visual;
            if (guardVisual != null)
                guardVisual.SetActive(false);
        }

        private void Update()
        {
            if (guardVisual != null && guardVisual.activeSelf != IsGuarding)
                guardVisual.SetActive(IsGuarding);
        }

        private void OnDisable()
        {
            if (guardVisual != null)
                guardVisual.SetActive(false);
        }

        public bool TryInterceptDamage(int amount, Transform attacker)
        {
            if (!IsGuarding || attacker == null)
                return false;

            Vector3 directionToAttacker = attacker.position - transform.position;
            directionToAttacker.y = 0f;
            if (directionToAttacker.sqrMagnitude < 0.001f)
                return false;

            float facing = Vector3.Dot(transform.forward, directionToAttacker.normalized);
            if (facing < minimumFacingDot)
                return false;

            guardAvailableAt = Time.time + recoveryAfterBlock;
            if (guardVisual != null)
                guardVisual.SetActive(false);
            return true;
        }
    }
}
