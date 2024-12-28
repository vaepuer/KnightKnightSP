using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalkedIntoWall : MonoBehaviour
{
    public Transform canvasTransform;
    public string[] obstacleMessages;
    public GameObject floatingUIPrefab;
    public GameObject uiSpawner;
    public float fadeDuration;
    public float floatDuration;

    public float maxQueueSize;

    public AudioSource soundEffectAudioSource;

    [SerializeField] private Queue<string> messageQueue = new Queue<string>(); // Queue for messages
    private GameObject currentFloatingUI;
    private TextMeshProUGUI uiText; // Add this variable for TextMeshPro text
    private Coroutine fadeOutCoroutine;

    private void Start()
    {
        HideFloatingUI();
    }

    public void ShowFloatingUI(string message)
    {
        messageQueue.Enqueue(message); // Add the message to the queue

        if (messageQueue.Count > maxQueueSize)
        {
            // If the queue exceeds the maximum size, remove the oldest message
            messageQueue.Dequeue();
        }

        if (currentFloatingUI == null)
        {
            // If there's no current message, show the next message in the queue
            ShowNextMessage();
            PlaySoundEffect(); // Play the sound
        }
    }

    private void PlaySoundEffect()
    {
        if (soundEffectAudioSource != null && soundEffectAudioSource.clip != null)
        {
            soundEffectAudioSource.PlayOneShot(soundEffectAudioSource.clip);
        }
    }

    private void ShowNextMessage()
    {
        if (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue(); // Get the next message in the queue

            // Spawn the UI Prefab at the specified position
            currentFloatingUI = Instantiate(floatingUIPrefab, uiSpawner.transform.position, Quaternion.identity);
            currentFloatingUI.transform.SetParent(canvasTransform, true);

            uiText = currentFloatingUI.GetComponentInChildren<TextMeshProUGUI>(); // Initialize uiText
            uiText.text = message;

            if (fadeOutCoroutine != null)
                StopCoroutine(fadeOutCoroutine);

            fadeOutCoroutine = StartCoroutine(FadeOutUI());
        }
    }

    IEnumerator FadeOutUI()
    {
        float startTime = Time.time;

        if (uiText != null)
        {
            while (Time.time - startTime < fadeDuration)
            {
                // Calculate the current alpha value for fading
                float alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration);

                // Update the text's alpha
                uiText.CrossFadeAlpha(alpha, 0.1f, false);

                if (alpha <= 0f)
                {
                    // If alpha reaches 0, remove the message from the queue
                    messageQueue.Dequeue();
                    currentFloatingUI = null;
                    ShowNextMessage();
                }

                // Calculate the current Y position for floating
                float initialY = uiText.rectTransform.localPosition.y;
                float targetY = initialY + (1f * (Time.time - startTime) / floatDuration); // Adjust the float speed as needed

                // Update the text's Y position
                uiText.rectTransform.localPosition = new Vector3(uiText.rectTransform.localPosition.x, targetY, uiText.rectTransform.localPosition.z);

                yield return null;
            }

            // Destroy the UI element
            Destroy(currentFloatingUI);
        }
    }

    void HideFloatingUI()
    {
        if (currentFloatingUI != null)
            Destroy(currentFloatingUI);
    }
}
