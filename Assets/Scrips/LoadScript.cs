using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScript : MonoBehaviour
{
    public Image filledImage;  // Reference to the filled image (the one that will fill up)
    public Text loadingText;   // Reference to the text that displays the percentage
    public float loadDuration = 5f; // The duration over which the loading occurs (5 seconds)

    private float loadProgress = 0f;

    void Update()
    {
        if (loadProgress < 1f)
        {
            loadProgress += Time.deltaTime / loadDuration; // Increment progress over time.
            loadProgress = Mathf.Clamp01(loadProgress); // Ensure the progress does not exceed 1.

            filledImage.fillAmount = loadProgress;  // Update the fill amount of the image.
            loadingText.text = Mathf.RoundToInt(loadProgress * 100) + "%"; // Display the percentage.
        }
        else
        {
            SceneManager.LoadScene("FightSence");
        }
    }


}
