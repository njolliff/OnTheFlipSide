using UnityEngine;
using UnityEngine.UI;

public class LevelUIInitializer : MonoBehaviour
{
    // PRIVATE
    [SerializeField] private Image sprite;
    [SerializeField] private Button button;
    private string level;
    private int levelValue;
    
    void Start()
    {
        levelValue = -1;
        
        // Extract last number from object name
        if (int.TryParse(gameObject.name.Replace("Level ", ""), out levelValue))
        {
            level = levelValue.ToString();
        }

        Debug.Log("Extracted Level: " + level);
        
        ApplySprite();
    }

    void ApplySprite()
    {
        if (PlayerPrefs.HasKey(level))
        {
            string filePath = "Numbers/" + level + "/" + level;

            if (PlayerPrefs.GetInt(level) == -1)
            {
                filePath += "_unlocked";
                button.interactable = false;
            }
            else if (PlayerPrefs.GetInt(level) == 0)
            {
                filePath += "_unlocked";
                button.interactable = true;
            }
            else if (PlayerPrefs.GetInt(level) == 1)
            {
                filePath += "_completed";
                button.interactable = true;
            }

            Debug.Log("Loading Sprite from: " + filePath);

            Sprite loadedSprite = Resources.Load<Sprite>(filePath);
            if (loadedSprite != null)
            {
                sprite.sprite = loadedSprite;
            }
            else
            {
                Debug.LogError("Failed to load sprite at: " + filePath);
            }
        }
    }
}