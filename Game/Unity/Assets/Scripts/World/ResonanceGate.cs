using System.Collections;
using UnityEngine;

namespace ElijahBlaze.World
{
    public class ResonanceGate : MonoBehaviour
    {
        private enum GateState { Dormant, Awakening, Open }

        [SerializeField] private HollowCourtEncounter encounter;
        [SerializeField] private Transform gateBody;
        [SerializeField] private Vector3 openOffset = new Vector3(0f, 4f, 0f);
        [SerializeField, Min(0.1f)] private float openingDuration = 1.5f;

        private GateState state = GateState.Dormant;
        private Vector3 closedPosition;

        private void Awake()
        {
            if (gateBody == null)
                gateBody = transform;

            closedPosition = gateBody.localPosition;
        }

        private void OnEnable()
        {
            if (encounter != null)
                encounter.EncounterCleared += BeginOpening;
        }

        private void Start()
        {
            if (encounter != null && encounter.IsCleared)
                BeginOpening();
        }

        private void OnDisable()
        {
            if (encounter != null)
                encounter.EncounterCleared -= BeginOpening;
        }

        public void BeginOpening()
        {
            if (state != GateState.Dormant)
                return;

            state = GateState.Awakening;
            StartCoroutine(OpenRoutine());
        }

        private IEnumerator OpenRoutine()
        {
            Vector3 openPosition = closedPosition + openOffset;
            float elapsed = 0f;

            while (elapsed < openingDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / openingDuration));
                gateBody.localPosition = Vector3.Lerp(closedPosition, openPosition, t);
                yield return null;
            }

            gateBody.localPosition = openPosition;
            state = GateState.Open;
            Debug.Log("Resonance gate open.", this);
        }
    }
}
