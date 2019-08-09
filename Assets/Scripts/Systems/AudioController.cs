using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController instance = null;
    
    public static AudioController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioController>();

                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "LevelManager";
                    instance = go.AddComponent<AudioController>();

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
}
