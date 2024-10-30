using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FadeInTitleScreen : MonoBehaviour
{
    public TextMeshProUGUI titleScreenText; // Reference to the title screen TextMeshPro text
    public Button startButton;              // Reference to the Start button
    public Button controlButton;            // Reference to the Control button
    public Button exitButton;               // Reference to the Exit button

    public float fadeDuration = 2f;         // Duration of each fade-in in seconds
    public float delayBetweenFades = 0.5f;  // Delay before fading in buttons after title

    void Start()
    {
        // Set initial alpha of each UI element and their text components to 0 (invisible)
        SetUIElementAlpha(titleScreenText, 0f);
        SetButtonAndTextAlpha(startButton, 0f);
        SetButtonAndTextAlpha(controlButton, 0f);
        SetButtonAndTextAlpha(exitButton, 0f);

        // Start the fade-in sequence
        StartCoroutine(FadeInSequence());
    }

    // Coroutine to handle fading in title and then buttons simultaneously
    IEnumerator FadeInSequence()
    {
        // Fade in the title screen text
        yield return FadeInUIElement(titleScreenText);

        // Wait a bit, then fade in all buttons and their text at the same time
        yield return new WaitForSeconds(delayBetweenFades);
        StartCoroutine(FadeInButtonAndTextSimultaneously(startButton));
        StartCoroutine(FadeInButtonAndTextSimultaneously(controlButton));
        StartCoroutine(FadeInButtonAndTextSimultaneously(exitButton));
    }

    // Function to fade in a UI element's alpha value from 0 to 1
    IEnumerator FadeInUIElement(Graphic uiElement)
    {
        float elapsedTime = 0f;
        Color color = uiElement.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = alpha;
            uiElement.color = color;
            yield return null;
        }

        // Ensure the alpha is fully set to 1 after fading in
        color.a = 1f;
        uiElement.color = color;
    }

    // Method to fade in both button image and button text at the same time
    IEnumerator FadeInButtonAndTextSimultaneously(Button button)
    {
        float elapsedTime = 0f;
        
        // Get the button image and its TextMeshProUGUI text
        Graphic buttonImage = button.image;
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        // Ensure both components exist
        if (buttonImage != null && buttonText != null)
        {
            Color buttonColor = buttonImage.color;
            Color textColor = buttonText.color;

            // Fade in both image and text simultaneously
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                
                buttonColor.a = alpha;
                buttonImage.color = buttonColor;

                textColor.a = alpha;
                buttonText.color = textColor;

                yield return null;
            }

            // Set final alpha to 1 for both
            buttonColor.a = 1f;
            buttonImage.color = buttonColor;
            textColor.a = 1f;
            buttonText.color = textColor;
        }
    }

    // Helper function to set the alpha of a UI element immediately
    void SetUIElementAlpha(Graphic uiElement, float alpha)
    {
        Color color = uiElement.color;
        color.a = alpha;
        uiElement.color = color;
    }

    // Helper function to set alpha for both button image and its text
    void SetButtonAndTextAlpha(Button button, float alpha)
    {
        SetUIElementAlpha(button.image, alpha);
        
        // Set alpha for the button's TextMeshProUGUI text
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            SetUIElementAlpha(buttonText, alpha);
        }
    }
}
