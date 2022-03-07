using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float timeValue;
    public GameObject timerGO;
    public bool isStop;

    #region Singleton

    public static TimerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        timeValue = 0;
        isStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStop)
            timeValue += Time.deltaTime;

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
            timeToDisplay = 0;
        else if(timeToDisplay > 0)
            timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerGO.GetComponent<Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds); 
    }

    public void StopTimer()
    {
        isStop = true;
    }

    public string GetTime()
    {
        Debug.Log(timerGO.GetComponent<Text>().text);
        return timerGO.GetComponent<Text>().text;
    }
}
