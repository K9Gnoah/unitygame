using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MapManagement : MonoBehaviour
{
    public MapDatabase mapDB;
    public SpriteRenderer artworkSprite;
    public int selectedOption = 0;

    public Vector2 fixedMapSize = new Vector2(100, 200);
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedMap"))
        {
            selectedOption = 0;
        }

        else
        {
            Load();
        }

        UpdateMap(selectedOption);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            NextOption();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            BackOption();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeScene(2);
        }
    }

    public void NextOption()
    {
        selectedOption++;

        if (selectedOption >= mapDB.mapCount)
        {
            selectedOption = 0;
        }

        UpdateMap(selectedOption);
        Save();
    }

    public void BackOption()
    {
        selectedOption--;

        if (selectedOption < 0)
        {
            selectedOption = mapDB.mapCount - 1;
        }

        UpdateMap(selectedOption);
        Save();
    }

    private void UpdateMap(int selectedOption)
    {
        Map map = mapDB.getMap(selectedOption);
        artworkSprite.sprite = map.mapSprite;

        // Calculate the scale to maintain aspect ratio
        Vector2 spriteSize = artworkSprite.sprite.bounds.size;
        float scaleX = fixedMapSize.x / spriteSize.x;
        float scaleY = fixedMapSize.y / spriteSize.y;
        float scale = Mathf.Min(scaleX, scaleY);

        // Apply the scale to the artwork sprite
        artworkSprite.transform.localScale = new Vector3(scale, scale, 1);
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedMap");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectedMap", selectedOption);
    }

    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
