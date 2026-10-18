using ElijahBlaze.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ElijahBlaze.World
{
    [RequireComponent(typeof(Canvas))]
    public class HollowCourtHud : MonoBehaviour
    {
        [SerializeField] private HollowCourtEncounter encounter;
        [SerializeField] private Health playerHealth;

        private Text chapterLabel;
        private Text vitalityLabel;
        private Text objectiveLabel;
        private Text instructionLabel;

        public void Configure(HollowCourtEncounter source, Health health)
        {
            encounter = source;
            playerHealth = health;
        }

        private void Awake()
        {
            BuildHud();
        }

        private void OnEnable()
        {
            if (encounter == null)
                return;

            encounter.EncounterStarted += Refresh;
            encounter.ProgressChanged += HandleProgressChanged;
            encounter.EncounterCleared += Refresh;
            encounter.LevelCompleted += Refresh;

            if (playerHealth != null)
            {
                playerHealth.Damaged += HandlePlayerHealthChanged;
                playerHealth.Died += Refresh;
            }
        }

        private void Start()
        {
            Refresh();
        }

        private void OnDisable()
        {
            if (encounter == null)
                return;

            encounter.EncounterStarted -= Refresh;
            encounter.ProgressChanged -= HandleProgressChanged;
            encounter.EncounterCleared -= Refresh;
            encounter.LevelCompleted -= Refresh;

            if (playerHealth != null)
            {
                playerHealth.Damaged -= HandlePlayerHealthChanged;
                playerHealth.Died -= Refresh;
            }
        }

        private void HandleProgressChanged(int defeated, int total)
        {
            Refresh();
        }

        private void HandlePlayerHealthChanged(int currentHealth, int maxHealth)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (playerHealth != null && vitalityLabel != null)
            {
                vitalityLabel.text = playerHealth.IsDead
                    ? "VITALITY  0 / " + playerHealth.MaxHealth
                    : "VITALITY  " + playerHealth.CurrentHealth + " / " + playerHealth.MaxHealth;
            }

            if (encounter == null || objectiveLabel == null)
                return;

            if (encounter.IsComplete)
            {
                chapterLabel.text = "HOLLOW COURT  /  FIRST RESONANCE";
                objectiveLabel.text = "Hollow Court cleared.";
                instructionLabel.text = "The path through the first court is open.";
            }
            else if (encounter.IsCleared)
            {
                chapterLabel.text = "THE COURT ANSWERS";
                objectiveLabel.text = "Cross to the far threshold.";
                instructionLabel.text = "The Resonance Gate is rising.";
            }
            else if (encounter.IsActive)
            {
                chapterLabel.text = "THE SHARDLINGS STIR";
                objectiveLabel.text = $"Defeat the Shardlings  ·  {encounter.TotalCount - encounter.RemainingCount}/{encounter.TotalCount}";
                instructionLabel.text = "Hold left mouse or Space to strike.\nHold Shift or right mouse to guard.";
            }
            else
            {
                chapterLabel.text = "HOLLOW COURT  /  FIRST RESONANCE";
                objectiveLabel.text = "Follow the resonance mark.";
                instructionLabel.text = "Approach the glowing seal at the center.\nWASD / arrow keys to move.";
            }
        }

        private void BuildHud()
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20;

            CanvasScaler scaler = GetComponent<CanvasScaler>();
            if (scaler == null)
                scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            if (GetComponent<GraphicRaycaster>() == null)
                gameObject.AddComponent<GraphicRaycaster>();

            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font == null)
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            RectTransform panel = CreateRect("Objective_Panel", transform);
            SetTopLeft(panel, new Vector2(28f, -26f), new Vector2(500f, 158f));
            Image background = panel.gameObject.AddComponent<Image>();
            background.color = new Color(0.025f, 0.045f, 0.065f, 0.86f);
            background.raycastTarget = false;

            RectTransform accent = CreateRect("Resonance_Accent", panel);
            SetTopLeft(accent, new Vector2(0f, 0f), new Vector2(5f, 158f));
            Image accentImage = accent.gameObject.AddComponent<Image>();
            accentImage.color = new Color(0.24f, 0.82f, 0.92f, 1f);
            accentImage.raycastTarget = false;

            chapterLabel = CreateLabel("Chapter_Label", panel, font, 14, FontStyle.Bold, new Color(0.42f, 0.88f, 0.96f, 1f));
            SetTopLeft(chapterLabel.rectTransform, new Vector2(22f, -14f), new Vector2(330f, 24f));

            vitalityLabel = CreateLabel("Vitality_Label", panel, font, 14, FontStyle.Bold, new Color(1f, 0.78f, 0.36f, 1f));
            vitalityLabel.alignment = TextAnchor.MiddleRight;
            SetTopRight(vitalityLabel.rectTransform, new Vector2(-20f, -14f), new Vector2(138f, 24f));

            objectiveLabel = CreateLabel("Objective_Label", panel, font, 23, FontStyle.Bold, Color.white);
            SetTopLeft(objectiveLabel.rectTransform, new Vector2(22f, -43f), new Vector2(456f, 48f));

            instructionLabel = CreateLabel("Instruction_Label", panel, font, 16, FontStyle.Normal, new Color(0.82f, 0.88f, 0.92f, 1f));
            SetTopLeft(instructionLabel.rectTransform, new Vector2(22f, -99f), new Vector2(456f, 52f));
        }

        private static Text CreateLabel(string name, Transform parent, Font font, int fontSize, FontStyle style, Color color)
        {
            RectTransform rect = CreateRect(name, parent);
            Text label = rect.gameObject.AddComponent<Text>();
            label.font = font;
            label.fontSize = fontSize;
            label.fontStyle = style;
            label.color = color;
            label.alignment = TextAnchor.MiddleLeft;
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            label.raycastTarget = false;
            return label;
        }

        private static RectTransform CreateRect(string name, Transform parent)
        {
            GameObject item = new GameObject(name, typeof(RectTransform));
            item.transform.SetParent(parent, false);
            return item.GetComponent<RectTransform>();
        }

        private static void SetTopLeft(RectTransform rect, Vector2 position, Vector2 size)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private static void SetTopRight(RectTransform rect, Vector2 position, Vector2 size)
        {
            rect.anchorMin = new Vector2(1f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }
    }
}
