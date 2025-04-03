using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool retainProgress = true;

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
            {
                instance = null;

                // Reset PlayerPrefs if retainProgress is set to false
                if (!retainProgress)
                {
                    for (int i = 1; i < 11; i++)
                    {
                        if (PlayerPrefs.HasKey(i.ToString()))
                        {
                            if (i != 1)
                                PlayerPrefs.SetInt(i.ToString(), -1); // Set all levels other than 1 to -1 (locked)
                            else
                                PlayerPrefs.SetInt(i.ToString(), 0); // Set Level 1 to 0 (unlocked)
                        }
                    }
                }
            }
    }

    public void UnlockLevel(int level)
    {
        if (PlayerPrefs.HasKey(level.ToString()))
            if (PlayerPrefs.GetInt(level.ToString()) == -1) // Only unlock if it's locked
                PlayerPrefs.SetInt(level.ToString(), 0);
    }

    public void CompleteLevel(int level)
    {
        if (PlayerPrefs.HasKey(level.ToString()))
            PlayerPrefs.SetInt(level.ToString(), 1);
    }
}