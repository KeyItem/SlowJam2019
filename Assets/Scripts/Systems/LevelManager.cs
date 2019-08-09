using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour // Referenced from : http://www.unitygeek.com/unity_c_singleton/
{
    private static LevelManager instance = null;
    
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();

                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "LevelManager";
                    instance = go.AddComponent<LevelManager>();

                    DontDestroyOnLoad(go);
                }
            }

            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevelAtIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCount <= nextScene)
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public void LoadPreviousLevel()
    {
        int lastScene = SceneManager.GetActiveScene().buildIndex - 1;

        if (SceneManager.sceneCount >= lastScene)
        {
            SceneManager.LoadScene(lastScene);
        }
    }

    public void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene);
    }
}