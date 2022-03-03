using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Networking;

public class LeaderboardController : MonoBehaviour
{
    private Transform entryTable; // cont
    private Transform entryRow; // row
    private DatabaseController db;
    private List<Score> scores;
    private List<Patient> patients;

    List<Transform> rowTransform = new List<Transform>();

    private void Awake() {
        db = new DatabaseController();
        db.InitDb();
        //Debug.Log("LeaderboardController connecting db...");
    }

    // Start is called before the first frame update
    void Start()
    {
         // Create 10 rows
        entryTable = GameObject.Find("Table").transform;
        entryRow = GameObject.Find("Row").transform;
        entryRow.gameObject.SetActive(false);
        float rowHeight = 35f;

        //db.SelectPatientsByIds(new List<int>() { 1, 3 });

        scores = db.dbScores;

        for (int i = 0; i < scores.Count; i++)
        {
            Transform entryTransform = Instantiate(entryRow, entryTable);
            RectTransform entryRecTransform = entryTransform.GetComponent<RectTransform>();
            entryRecTransform.anchoredPosition = new Vector2(0, -rowHeight * i);
            entryTransform.gameObject.SetActive(true);
            rowTransform.Add(entryTransform);
        }


        refreshList();
        //Debug.Log("LeaderboardController Start()");

        //db.TerminatetDb();
    }
    public void refreshList()
    {
        setList();
    }

    private void setList()
    {
        int LeaderboardLength = scores.Count;

        Debug.Log("LeaderboardLength: " + LeaderboardLength);
        for (int i = 0; i < LeaderboardLength; i++)
        {
            Transform childRank = rowTransform[i].GetChild(0);
            Transform childName = rowTransform[i].GetChild(1);
            Transform childTime = rowTransform[i].GetChild(2);

            if (i < LeaderboardLength)
            {
                childRank.gameObject.GetComponent<Text>().text = (i + 1).ToString();
                childName.gameObject.GetComponent<Text>().text = scores[i].PatientName;
                childTime.gameObject.GetComponent<Text>().text = scores[i].TimeTaken.ToString();
            }
            else
            {
                childRank.gameObject.GetComponent<TextMeshProUGUI>().text = "";
                childName.gameObject.GetComponent<TextMeshProUGUI>().text = "";
                childTime.gameObject.GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    void OnDestroy()
    {
        
    }
}
