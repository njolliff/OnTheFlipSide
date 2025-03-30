using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    void Start()
    {
        if (instance == null)
        {
            // Initialize as singleton in DontDestroyOnload
            instance = this;
            DontDestroyOnLoad(gameObject);
        
            // Initialize PlayerPrefs level storage
            // -1 = locked | 0 = unlocked | 1 = completed
            for (int i = 1; i < 11; i++)
            {
                if (!PlayerPrefs.HasKey(i.ToString()))
                    if (i == 1)
                        PlayerPrefs.SetInt("1", 0); // Initialize level 1 as unlocked
                    else
                        PlayerPrefs.SetInt(i.ToString(), -1); // Initialize all other levels as locked
            }
        }
        else
            Destroy(gameObject);
    }
    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void UnlockLevel(int level)
    {
        if (PlayerPrefs.HasKey(level.ToString()))
            PlayerPrefs.SetInt(level.ToString(), 0);
    }

    public void CompleteLevel(int level)
    {
        if (PlayerPrefs.HasKey(level.ToString()))
            PlayerPrefs.SetInt(level.ToString(), 1);
    }
}