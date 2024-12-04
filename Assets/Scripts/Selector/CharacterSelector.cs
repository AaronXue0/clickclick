using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Mediapipe.Tasks.Vision.HandLandmarker;
using ClickClick.GestureTracking;
using Mediapipe.Unity;
using TMPro;
using DG.Tweening;

namespace ClickClick.Tool
{
    [System.Serializable]
    public class CharacterData
    {
        public Sprite characterSprite;
        public string characterName;
        public Button characterButton;
        public Image characterImage;
        public Image progressImage;
    }

    public class CharacterSelector : CircularProgressOnHold
    {
        [Header("Character Selection")]
        [SerializeField] private Sprite defaultPreviewSprite;
        [SerializeField] private List<CharacterData> characters = new List<CharacterData>();
        [SerializeField] private Image previewImage;
        [SerializeField] private TMP_Text characterNameText;
        [SerializeField] private RectTransform screenshotArea;

        private CharacterData currentTarget;
        private Dictionary<Button, Vector3> originalButtonScales = new Dictionary<Button, Vector3>();
        private bool isSelectionLocked = false;

        protected override float ProgressFillAmount
        {
            get => currentTarget?.progressImage.fillAmount ?? 0f;
            set
            {
                if (currentTarget != null)
                {
                    currentTarget.progressImage.fillAmount = value;
                }
            }
        }

        protected override void InitializeTargetButton()
        {
            UpdatePreview(null);
            foreach (var character in characters)
            {
                originalButtonScales[character.characterButton] = character.characterButton.transform.localScale;
                character.characterImage.sprite = character.characterSprite;
                character.characterButton.GetComponentInChildren<TMP_Text>().text = character.characterName;
            }
        }

        protected override void HandleProgressComplete()
        {
            if (currentTarget != null && !isSelectionLocked)
            {
                // Lock the selection
                isSelectionLocked = true;

                // Update preview
                previewImage.sprite = currentTarget.characterSprite;
                characterNameText.text = currentTarget.characterName;

                // Trigger the button click
                currentTarget.characterButton.onClick.Invoke();

                // Transition to the next scene after a short delay
                StartCoroutine(TransitionAfterDelay());
            }
        }

        private IEnumerator TransitionAfterDelay()
        {
            // Wait for a short moment to show the final state
            yield return new WaitForSeconds(0.5f);
            // Transition to the next scene
            SceneTransition.Instance.TransitionToScene(sceneToTransitionTo);
        }

        protected override bool IsOverlappingTargetButton(GameObject gestureObject)
        {
            // If selection is locked, prevent any further changes
            if (isSelectionLocked)
            {
                return currentTarget != null;
            }

            if (gestureObject == null || gameObject.activeSelf == false)
            {
                return false;
            }

            RectTransform gestureRect = gestureObject.GetComponent<RectTransform>();
            CharacterData previousTarget = currentTarget;

            foreach (var character in characters)
            {
                RectTransform buttonRect = character.characterButton.GetComponent<RectTransform>();

                if (gestureRect != null && buttonRect != null)
                {
                    Vector3[] gestureCorners = new Vector3[4];
                    Vector3[] buttonCorners = new Vector3[4];

                    gestureRect.GetWorldCorners(gestureCorners);
                    buttonRect.GetWorldCorners(buttonCorners);

                    Rect gestureRectangle = new Rect(gestureCorners[0].x, gestureCorners[0].y,
                        gestureCorners[2].x - gestureCorners[0].x, gestureCorners[2].y - gestureCorners[0].y);

                    Rect buttonRectangle = new Rect(buttonCorners[0].x, buttonCorners[0].y,
                        buttonCorners[2].x - buttonCorners[0].x, buttonCorners[2].y - buttonCorners[0].y);

                    if (gestureRectangle.Overlaps(buttonRectangle))
                    {
                        if (previousTarget != null && previousTarget != character)
                        {
                            ResetProgress();
                        }

                        currentTarget = character;
                        UpdatePreview(character);
                        return true;
                    }
                }
            }

            currentTarget = null;
            UpdatePreview(null);
            return false;
        }

        protected override void UpdateTargetButtonScale(bool isOverlapping)
        {
            // Reset all buttons to original scale except the current target
            foreach (var character in characters)
            {
                if (character != currentTarget)
                {
                    character.characterButton.transform.DOScale(
                        originalButtonScales[character.characterButton],
                        stateChangeDuration
                    );
                    character.progressImage.fillAmount = 0f;
                }
            }

            // Scale the current target button if there is one
            if (currentTarget != null)
            {
                Vector3 originalScale = originalButtonScales[currentTarget.characterButton];
                Vector3 targetScale = isOverlapping ?
                    originalScale * targetButtonScaleDownFactor :
                    originalScale;

                currentTarget.characterButton.transform.DOScale(targetScale, stateChangeDuration);
            }
        }

        protected override void ResetProgress()
        {
            // Only reset if selection is not locked
            if (!isSelectionLocked)
            {
                base.ResetProgress();
                currentTarget = null;
                UpdatePreview(null);
            }
        }

        private void UpdatePreview(CharacterData character)
        {
            Debug.Log($"UpdatePreview called with character: {character}");
            Debug.Log($"previewImage reference: {previewImage}");

            if (character != null)
            {
                Debug.Log($"Character sprite: {character.characterSprite}");
                previewImage.sprite = character.characterSprite;
                characterNameText.text = character.characterName;
            }
            else
            {
                Debug.Log($"Default preview sprite: {defaultPreviewSprite}");
                previewImage.sprite = defaultPreviewSprite;
                characterNameText.text = "";
            }

            Debug.Log($"Preview image current sprite: {previewImage.sprite}");
        }
    }
}
