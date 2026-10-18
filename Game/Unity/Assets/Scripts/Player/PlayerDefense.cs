using ElijahBlaze.Core;
using UnityEngine;

namespace ElijahBlaze.Player
{
    [RequireComponent(typeof(Health))]
    public class PlayerDefense : MonoBehaviour, IDamageInterceptor
    {
        [SerializeField, Min(0.05f)] private float recoveryAfterBlock = 0.38f;

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
            // Holding guard blocks the next incoming Shardling hit from any side.
            // A short recovery prevents one held key from canceling every attack.
            if (!IsGuarding || attacker == null)
                return false;

            guardAvailableAt = Time.time + recoveryAfterBlock;
            if (guardVisual != null)
                guardVisual.SetActive(false);
            return true;
        }
    }
}
