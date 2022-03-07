using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Networking;

public class LeaderboardController : MonoBehaviour
{
    private Int16 recordsPerPage = 10;
    private Transform entryTable; // cont
    private Transform entryRow; // row
    private DatabaseController db;
    private List<Score> scores;
    private List<Patient> patients;

    List<Transform> rowTransform = new List<Transform>();

    private InputField inputSearch;
    

    private void Awake() {
        db = new DatabaseController();
        db.InitDb();
        //Debug.Log("LeaderboardController connecting db...");
    }

    // Start is called before the first frame update
    void Start()
    {
        inputSearch = GameObject.Find("input_Search").GetComponent<InputField>();

         // Create 10 rows
        entryTable = GameObject.Find("Table").transform;
        entryRow = GameObject.Find("Row").transform;
        entryRow.gameObject.SetActive(false);
        float rowHeight = 35f;

        //db.SelectPatientsByIds(new List<int>() { 1, 3 });

        for (int i = 0; i < recordsPerPage ; i++)
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
        String searchUserID = inputSearch.text;
        
        if(searchUserID.Length>0)
        {
            setList(db.SelectScoresByPatientId(int.Parse(searchUserID)) );
        }
        else{
            setList(db.SelectAllScores());
        }
    }

    private void setList(List<Score> _score)
    {
        int LeaderboardLength = _score.Count;

        for (int i = 0; i < LeaderboardLength; i++)
        {
            
            Transform childRank = rowTransform[i].GetChild(0);
            Transform childName = rowTransform[i].GetChild(1);
            Transform childTest= rowTransform[i].GetChild(2);
            Transform childTime = rowTransform[i].GetChild(3);

            //if (i < LeaderboardLength)
            if (i < LeaderboardLength)
            {
                childRank.gameObject.GetComponent<Text>().text = (i + 1).ToString();
                childName.gameObject.GetComponent<Text>().text = _score[i].PatientName;
                childTest.gameObject.GetComponent<Text>().text = _score[i].GameMode;               
                childTime.gameObject.GetComponent<Text>().text = _score[i].TimeTaken.ToString();
            }
            else
            {
                childRank.gameObject.GetComponent<Text>().text = "";
                childName.gameObject.GetComponent<Text>().text = "";
                childTest.gameObject.GetComponent<Text>().text = "";     
                childTime.gameObject.GetComponent<Text>().text = "";
            }

            if(i+1 == recordsPerPage){
                break;
            }

        }
    }

    void OnDestroy()
    {
        
    }
}
