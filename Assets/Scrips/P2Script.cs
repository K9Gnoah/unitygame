using UnityEngine;

public class P2Script : MonoBehaviour
{
    public CharDatabase charDB;
    private GameObject currentCharacterInstance;
    private int selectedOption;

    void Start()
    {
        // Tải nhân vật đã chọn cho Player 2
        selectedOption = PlayerPrefs.GetInt("selectedP2", 0);

        // Tạo nhân vật dựa trên prefab đã chọn
        SpawnCharacter(selectedOption);
    }

    private void SpawnCharacter(int selectedOption)
    {
        Char character = charDB.GetChar(selectedOption);
        if (character.charPrefab != null)
        {
            if (currentCharacterInstance != null)
            {
                Destroy(currentCharacterInstance);
            }

            currentCharacterInstance = Instantiate(character.charPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Prefab không được thiết lập cho nhân vật này!");
        }
    }
}