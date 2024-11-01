using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifACE : MonoBehaviour
{
    [SerializeField] private Sprite[] gifFrames; // Các frame của GIF
    [SerializeField] private float frameRate = 0.1f; // Tốc độ phát từng frame
    private Image gifImage;

    private void Awake()
    {
        gifImage = GetComponent<Image>();
        gifImage.enabled = false; // Ẩn ảnh GIF ban đầu
    }

    // Coroutine để phát GIF
    public IEnumerator PlayGIF()
    {
        gifImage.enabled = true;

        // Phát từng frame của GIF
        for (int i = 0; i < gifFrames.Length; i++)
        {
            gifImage.sprite = gifFrames[i];
            yield return new WaitForSeconds(frameRate);
        }

        gifImage.enabled = false;
    }
}

