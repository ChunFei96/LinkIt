using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    #region Singleton

    public static GlobalManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public GameRule selectedGameRule;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
