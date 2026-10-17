using UnityEngine;

namespace ElijahBlaze.Enemies
{
    public class EnemyChase : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed = 2.7f;
        [SerializeField] private float stopDistance = 1.8f;
        [SerializeField] private float turnSpeed = 8f;

        public void SetTarget(Transform chaseTarget) => target = chaseTarget;

        private void Update()
        {
            if (target == null)
                return;

            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;
            if (distance <= stopDistance || distance < 0.01f)
                return;

            Vector3 direction = toTarget / distance;
            transform.position += direction * moveSpeed * Time.deltaTime;
            Quaternion desired = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desired, turnSpeed * Time.deltaTime);
        }
    }
}
