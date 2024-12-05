using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity;
using ClickClick.GestureTracking;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using ClickClick.Tool;
namespace ClickClick.Photograph
{
    public class Photograph : MonoBehaviour
    {
        [SerializeField] private MultiHandLandmarkListAnnotation handLandmarkAnnotation;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private float countdownDuration = 3f;
        [SerializeField] private UnityEvent onCountdownComplete;
        [Header("Photography")]
        [SerializeField] private GameObject photographContainer;
        [SerializeField] private Image flashEffect;
        [SerializeField] private RawImage screenshotSource;
        [SerializeField] private float flashDuration = 0.5f;
        [Header("Photo Display")]
        [SerializeField] private Image capturedPhotoImage;
        [SerializeField] private float photoDisplayDuration = 1f;

        [Header("Photo Mask")]
        [SerializeField] private Image photoMask;
        [SerializeField] private CanvasGroup photoMaskCanvasGroup;

        [Header("Scene Transition")]
        [SerializeField] private float delayBeforeTransition = 1f;
        [SerializeField] private GameObject transitionButtonContainer;
        [SerializeField] private SingleButtonProgress transitionButton;

        private HandGestureDetector gestureDetector;
        private HandLandmarkerResult currentResult;
        private bool needsUpdate = false;
        private float currentCountdown;
        private bool isCountingDown;
        private HandGesture? leftHandGesture;
        private HandGesture? rightHandGesture;
        private bool hasPhotoBeenTaken = false;

        private void Start()
        {
            gestureDetector = new HandGestureDetector();
            if (flashEffect != null)
            {
                flashEffect.gameObject.SetActive(false);
                var color = flashEffect.color;
                color.a = 0;
                flashEffect.color = color;
            }
            if (transitionButton != null)
            {
                transitionButton.progressApproval = false;
            }
        }

        private void Update()
        {
            if (needsUpdate)
            {
                UpdateGestureObjectsInternal(currentResult);
                needsUpdate = false;
            }

            UpdateCountdown();
        }

        public void UpdateGestureObjects(HandLandmarkerResult result)
        {
            currentResult = result;
            needsUpdate = true;
        }

        private void UpdateGestureObjectsInternal(HandLandmarkerResult result)
        {
            if (hasPhotoBeenTaken) return;

            leftHandGesture = null;
            rightHandGesture = null;

            if (ReferenceEquals(result, null) || result.handLandmarks == null || result.handLandmarks.Count == 0)
            {
                // ResetCountdown();
                return;
            }

            for (int i = 0; i < result.handLandmarks.Count; i++)
            {
                if (!ValidateHandIndex(i, result)) continue;

                var landmarks = result.handLandmarks[i];
                var handedness = result.handedness[i];
                bool isRightHand = handedness.categories[0].categoryName.ToLower().Contains("right");

                var gesture = gestureDetector.DetectGesture(landmarks);

                if (isRightHand)
                {
                    rightHandGesture = gesture;
                }
                else
                {
                    leftHandGesture = gesture;
                }
            }

            if (leftHandGesture == HandGesture.Scissors && rightHandGesture == HandGesture.Scissors)
            {
                OnBothHandsScissors();
            }
            // else
            // {
            //     ResetCountdown();
            // }
        }

        private bool ValidateHandIndex(int index, HandLandmarkerResult result)
        {
            return result.handedness != null &&
                   index < result.handedness.Count &&
                   result.handedness[index].categories != null &&
                   result.handedness[index].categories.Count > 0;
        }

        private void OnBothHandsScissors()
        {
            if (!isCountingDown)
            {
                isCountingDown = true;
                currentCountdown = countdownDuration;
            }
        }

        private void ResetCountdown()
        {
            isCountingDown = false;
            currentCountdown = countdownDuration;
            countdownText.text = "";
        }

        private void UpdateCountdown()
        {
            if (!isCountingDown) return;

            currentCountdown -= Time.deltaTime;

            if (currentCountdown <= 0)
            {
                isCountingDown = false;
                countdownText.text = "";
                TakePhotograph();
                return;
            }

            countdownText.text = Mathf.Ceil(currentCountdown).ToString();
        }

        private void TakePhotograph()
        {
            if (hasPhotoBeenTaken) return;

            hasPhotoBeenTaken = true;
            StartCoroutine(PhotoEffect());
        }

        private IEnumerator PhotoEffect()
        {
            flashEffect.gameObject.SetActive(true);

            // Flash in
            float elapsed = 0;
            while (elapsed < flashDuration / 2)
            {
                elapsed += Time.deltaTime;
                float alpha = elapsed / (flashDuration / 2);
                var color = flashEffect.color;
                color.a = alpha;
                flashEffect.color = color;
                yield return null;
            }

            photographContainer.SetActive(false);
            CaptureScreenshot();

            // Flash out
            elapsed = 0;
            while (elapsed < flashDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = 1 - (elapsed / (flashDuration / 2));
                var color = flashEffect.color;
                color.a = alpha;
                flashEffect.color = color;
                yield return null;
            }

            flashEffect.gameObject.SetActive(false);
        }

        private void CaptureScreenshot()
        {
            if (screenshotSource == null || capturedPhotoImage == null) return;

            Texture sourceTexture = screenshotSource.texture;
            if (sourceTexture == null)
            {
                Debug.LogError("Screenshot source has no texture");
                return;
            }

            // Create a temporary RenderTexture
            RenderTexture tempRT = RenderTexture.GetTemporary(
                sourceTexture.width,
                sourceTexture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear
            );

            // Copy the source texture to the temporary RenderTexture with flipping
            Graphics.Blit(sourceTexture, tempRT, new Vector2(-1, 1), new Vector2(1, 0));

            // Create a new Texture2D and read the RenderTexture content
            Texture2D texture = new Texture2D(
                tempRT.width,
                tempRT.height,
                TextureFormat.RGB24,
                false
            );

            // Store the current active RenderTexture
            RenderTexture previousActive = RenderTexture.active;
            RenderTexture.active = tempRT;

            // Read the pixels
            texture.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
            texture.Apply();

            // Restore the previous active RenderTexture
            RenderTexture.active = previousActive;

            // Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tempRT);

            // Convert to sprite
            Sprite photoSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            // Display the photo and start animation
            StartCoroutine(DisplayPhotoAnimation(photoSprite));
        }

        private IEnumerator DisplayPhotoAnimation(Sprite photoSprite)
        {
            // Setup initial state
            capturedPhotoImage.sprite = photoSprite;
            capturedPhotoImage.gameObject.SetActive(true);

            // Set to native size
            capturedPhotoImage.SetNativeSize();

            // Center the image
            capturedPhotoImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            capturedPhotoImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            capturedPhotoImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            capturedPhotoImage.rectTransform.anchoredPosition = Vector2.zero;

            // Calculate scale to fit screen while maintaining aspect ratio
            float screenWidth = UnityEngine.Screen.width;
            float screenHeight = UnityEngine.Screen.height;
            float imageWidth = capturedPhotoImage.rectTransform.rect.width;
            float imageHeight = capturedPhotoImage.rectTransform.rect.height;

            float scaleRatio = Mathf.Min(
                screenWidth / imageWidth,
                screenHeight / imageHeight
            ) * 0.8f; // 80% of screen size

            capturedPhotoImage.rectTransform.localScale = new Vector3(scaleRatio, scaleRatio, 1);

            // Wait for display duration
            yield return new WaitForSeconds(photoDisplayDuration);

            // Fade in photo mask
            photoMaskCanvasGroup.gameObject.SetActive(true);
            float elapsed = 0;
            float fadeDuration = 0.5f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = elapsed / fadeDuration;
                photoMaskCanvasGroup.alpha = alpha;
                yield return null;
            }

            photoMaskCanvasGroup.alpha = 1;

            // Set photo as child of mask
            capturedPhotoImage.transform.SetParent(photoMask.transform, true);
            capturedPhotoImage.transform.SetSiblingIndex(0);

            // Set to stretch and fill the mask
            // capturedPhotoImage.rectTransform.anchorMin = Vector2.zero;
            // capturedPhotoImage.rectTransform.anchorMax = Vector2.one;
            // capturedPhotoImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            // capturedPhotoImage.rectTransform.offsetMin = Vector2.zero;
            // capturedPhotoImage.rectTransform.offsetMax = Vector2.zero;
            // capturedPhotoImage.rectTransform.localScale = Vector3.one;

            onCountdownComplete?.Invoke();

            // Add delay before showing transition button
            yield return new WaitForSeconds(delayBeforeTransition);

            // Activate the transition button
            if (transitionButton != null)
            {
                transitionButton.progressApproval = true;
                transitionButtonContainer.SetActive(true);
            }

            // Remove the direct scene transition code since it will now be handled by SingleButtonProgress
        }
    }
}