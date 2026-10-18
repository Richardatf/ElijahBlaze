using UnityEngine;

namespace ElijahBlaze.Player
{
    /// <summary>
    /// Gives the builder-created guardian a quiet, readable gait without requiring an animator asset.
    /// Limb meshes are attached to shoulder and hip pivots by HollowCourtBuilder.
    /// </summary>
    public class PlayerProceduralMotion : MonoBehaviour
    {
        [SerializeField] private float walkCyclesPerSecond = 1.35f;
        [SerializeField] private float legSwingDegrees = 19f;
        [SerializeField] private float armSwingDegrees = 12f;
        [SerializeField] private float bodyBobHeight = 0.025f;

        private PlayerController movement;
        private Transform upperBody;
        private Transform headPivot;
        private Transform leftArmPivot;
        private Transform rightArmPivot;
        private Transform leftLegPivot;
        private Transform rightLegPivot;

        private Vector3 upperBodyRestPosition;
        private Quaternion upperBodyRestRotation;
        private Quaternion headRestRotation;
        private Quaternion leftArmRestRotation;
        private Quaternion rightArmRestRotation;
        private Quaternion leftLegRestRotation;
        private Quaternion rightLegRestRotation;
        private float gaitPhase;

        private void Awake()
        {
            movement = GetComponent<PlayerController>();
            upperBody = transform.Find("Guardian_Upper_Body");
            if (upperBody == null)
                return;

            headPivot = upperBody.Find("Guardian_Head_Pivot");
            leftArmPivot = upperBody.Find("Left_Arm_Pivot");
            rightArmPivot = upperBody.Find("Right_Arm_Pivot");
            leftLegPivot = transform.Find("Left_Leg_Pivot");
            rightLegPivot = transform.Find("Right_Leg_Pivot");

            upperBodyRestPosition = upperBody.localPosition;
            upperBodyRestRotation = upperBody.localRotation;
            headRestRotation = headPivot != null ? headPivot.localRotation : Quaternion.identity;
            leftArmRestRotation = leftArmPivot != null ? leftArmPivot.localRotation : Quaternion.identity;
            rightArmRestRotation = rightArmPivot != null ? rightArmPivot.localRotation : Quaternion.identity;
            leftLegRestRotation = leftLegPivot != null ? leftLegPivot.localRotation : Quaternion.identity;
            rightLegRestRotation = rightLegPivot != null ? rightLegPivot.localRotation : Quaternion.identity;
        }

        private void LateUpdate()
        {
            if (upperBody == null || movement == null)
                return;

            float blend = movement.MovementBlend;
            float deltaTime = Time.deltaTime;
            gaitPhase += deltaTime * walkCyclesPerSecond * Mathf.PI * 2f * blend;

            float stride = Mathf.Sin(gaitPhase) * blend;
            float breathing = Mathf.Sin(Time.time * 1.15f) * 0.008f * (1f - blend);
            float bob = Mathf.Abs(Mathf.Sin(gaitPhase)) * bodyBobHeight * blend + breathing;

            upperBody.localPosition = upperBodyRestPosition + Vector3.up * bob;
            upperBody.localRotation = upperBodyRestRotation
                * Quaternion.AngleAxis(1.4f * blend, Vector3.right)
                * Quaternion.AngleAxis(Mathf.Sin(gaitPhase * 0.5f) * 0.8f * blend, Vector3.forward);

            if (leftLegPivot != null)
                leftLegPivot.localRotation = leftLegRestRotation * Quaternion.AngleAxis(stride * legSwingDegrees, Vector3.right);
            if (rightLegPivot != null)
                rightLegPivot.localRotation = rightLegRestRotation * Quaternion.AngleAxis(-stride * legSwingDegrees, Vector3.right);
            if (leftArmPivot != null)
                leftArmPivot.localRotation = leftArmRestRotation * Quaternion.AngleAxis(-stride * armSwingDegrees, Vector3.right);
            if (rightArmPivot != null)
                rightArmPivot.localRotation = rightArmRestRotation * Quaternion.AngleAxis(stride * armSwingDegrees, Vector3.right);
            if (headPivot != null)
            {
                float headSway = Mathf.Sin(Time.time * 0.85f) * 0.7f * (0.35f + blend);
                headPivot.localRotation = headRestRotation * Quaternion.AngleAxis(headSway, Vector3.forward);
            }
        }
    }
}
