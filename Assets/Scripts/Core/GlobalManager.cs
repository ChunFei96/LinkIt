using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    #region Singleton

    public static GlobalManager Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    #endregion

    public GameRule selectedGameRule;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedLevel(GameRule gameRule)
    {
        selectedGameRule = gameRule;
    }

    public GameRule GetSelectedLevel()
    {
        return selectedGameRule;
    }
}
