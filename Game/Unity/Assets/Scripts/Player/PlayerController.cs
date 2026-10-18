using UnityEngine;

namespace ElijahBlaze.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float acceleration = 16f;
        [SerializeField] private float deceleration = 20f;
        [SerializeField] private float turnRateDegrees = 540f;
        [SerializeField] private float gravity = -25f;

        private CharacterController controller;
        private Transform cameraTransform;
        private Vector3 planarVelocity;
        private float verticalVelocity;

        public float MovementBlend
        {
            get
            {
                if (moveSpeed <= 0f)
                    return 0f;

                return Mathf.Clamp01(planarVelocity.magnitude / moveSpeed);
            }
        }

        public float ForwardAcceleration { get; private set; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            cameraTransform = Camera.main != null ? Camera.main.transform : null;
        }

        private void Update()
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 input = Vector3.ClampMagnitude(new Vector3(horizontal, 0f, vertical), 1f);

            Vector3 moveDirection = input;
            if (cameraTransform != null)
            {
                Vector3 forward = cameraTransform.forward;
                Vector3 right = cameraTransform.right;
                forward.y = 0f;
                right.y = 0f;

                if (forward.sqrMagnitude > 0.001f)
                    forward.Normalize();
                if (right.sqrMagnitude > 0.001f)
                    right.Normalize();

                moveDirection = forward * input.z + right * input.x;
            }

            if (moveDirection.sqrMagnitude > 1f)
                moveDirection.Normalize();

            float inputAmount = input.magnitude;
            float deltaTime = Time.deltaTime;
            Vector3 previousVelocity = planarVelocity;
            Vector3 targetVelocity = moveDirection * (moveSpeed * inputAmount);
            float speedChange = inputAmount > 0f ? acceleration : deceleration;
            planarVelocity = Vector3.MoveTowards(planarVelocity, targetVelocity, speedChange * deltaTime);

            Vector3 planarAcceleration = (planarVelocity - previousVelocity) / Mathf.Max(deltaTime, 0.0001f);
            ForwardAcceleration = Vector3.Dot(planarAcceleration, transform.forward);

            if (moveDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    turnRateDegrees * deltaTime);
            }

            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;

            verticalVelocity += gravity * deltaTime;
            Vector3 velocity = planarVelocity + Vector3.up * verticalVelocity;
            controller.Move(velocity * deltaTime);
        }
    }
}
