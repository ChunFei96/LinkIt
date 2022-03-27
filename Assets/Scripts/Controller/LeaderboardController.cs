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
    private List<Patient> patients;
    
    List<Transform> rowTransform = new List<Transform>();

    private InputField inputSearch;
    private GameObject errorMsg;
    private GameObject sortOptionTestDate;
    private GameObject sortOptionTimeTaken;

    private void Awake() {
        db = new DatabaseController();
        db.InitDb();
        //Debug.Log("LeaderboardController connecting db...");
        
    }

    // Start is called before the first frame update
    void Start()
    {
        inputSearch = GameObject.Find("input_Search").GetComponent<InputField>();
        errorMsg = GameObject.Find("txt_ErrorMsg");
        errorMsg.GetComponent<Text>().text = "";

        sortOptionTestDate = GameObject.Find("sortBy_TestDate");
        sortOptionTimeTaken = GameObject.Find("sortBy_Time");

         // Create 10 rows
        entryTable = GameObject.Find("Table").transform;
        entryRow = GameObject.Find("Row").transform;
        entryRow.gameObject.SetActive(false);
        float rowHeight = 35f;

        for (int i = 0; i < recordsPerPage ; i++)
        {
            Transform entryTransform = Instantiate(entryRow, entryTable);
            RectTransform entryRecTransform = entryTransform.GetComponent<RectTransform>();
            entryRecTransform.anchoredPosition = new Vector2(0, -rowHeight * i);
            entryTransform.gameObject.SetActive(true);
            rowTransform.Add(entryTransform);

            // clear text
            rowTransform[i].GetChild(0).gameObject.GetComponent<Text>().text = "";
            rowTransform[i].GetChild(1).gameObject.GetComponent<Text>().text = "";
            rowTransform[i].GetChild(2).gameObject.GetComponent<Text>().text = "";
            rowTransform[i].GetChild(3).gameObject.GetComponent<Text>().text = "";
            rowTransform[i].GetChild(4).gameObject.GetComponent<Text>().text = "";
            rowTransform[i].GetChild(5).gameObject.GetComponent<Text>().text = "";
        }
        
        refreshList();
    }
    public void refreshList()
    {
        String searchText = inputSearch.text;

        if(!string.IsNullOrEmpty(searchText)){
            if(!db.ValidPatientId(searchText)){
                errorMsg.GetComponent<Text>().text = "Error: Invalid id!";               

                Debug.Log("Invalid id: " + searchText);
                return;
            }
        }

        String sortBy = Constants.ScoreTable.CREATE_ON;
        bool isSortByDate = sortOptionTestDate.GetComponent<Toggle>().isOn;
        if(isSortByDate)
        {
            sortBy = Constants.ScoreTable.CREATE_ON;        
        }        
        else{
            sortBy = Constants.ScoreTable.TIME;        
        }

        Debug.Log("searchText: " + searchText + " sortBy: " + sortBy);
        setList(db.SelectScoreLeaderboard(searchText, sortBy));  //Display all scores 
    }

    private void ClearAllRows()
    {
        Debug.Log("childCount" + entryTable.childCount.ToString());

        //Remove all rows
        for (int i = 0; i < entryTable.childCount; i++)
        {
            Destroy(entryTable.GetChild(i));
        }
    }

    private void setList(List<Score> _score)
    {
        int LeaderboardLength = _score.Count;

        //ClearAllRows();

        for (int i = 0; i < recordsPerPage; i++)
        {
            
            Transform childRank = rowTransform[i].GetChild(0);
            Transform childID = rowTransform[i].GetChild(1);
            Transform childName = rowTransform[i].GetChild(2);
            Transform childTest= rowTransform[i].GetChild(3);
            Transform childTime = rowTransform[i].GetChild(4);
            Transform childCreatedOn = rowTransform[i].GetChild(5);

            if (i < LeaderboardLength)
            {
                childRank.gameObject.GetComponent<Text>().text = (i + 1).ToString();
                childID.gameObject.GetComponent<Text>().text = _score[i].PatientId.ToString();
                childName.gameObject.GetComponent<Text>().text = _score[i].PatientName;
                childTest.gameObject.GetComponent<Text>().text = _score[i].GameMode;               
                childTime.gameObject.GetComponent<Text>().text = _score[i].TimeTaken.ToString();
                childCreatedOn.gameObject.GetComponent<Text>().text = _score[i].CreatedOn.ToString();
            }
            else
            {
                childRank.gameObject.GetComponent<Text>().text = "";                
                childID.gameObject.GetComponent<Text>().text = "";
                childName.gameObject.GetComponent<Text>().text = "";
                childTest.gameObject.GetComponent<Text>().text = "";     
                childTime.gameObject.GetComponent<Text>().text = "";
                childCreatedOn.gameObject.GetComponent<Text>().text = "";
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
