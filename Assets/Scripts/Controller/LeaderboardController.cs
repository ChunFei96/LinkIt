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

    List<Transform> rowTransform = new List<Transform>();

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
         // Create 10 rows
        entryTable = GameObject.Find("Table").transform;
        entryRow = GameObject.Find("Row").transform;
        entryRow.gameObject.SetActive(false);
        float rowHeight = 35f;

       for (int i = 0; i < 5; i++)
        {
            Transform entryTransform = Instantiate(entryRow, entryTable);
            RectTransform entryRecTransform = entryTransform.GetComponent<RectTransform>();
            entryRecTransform.anchoredPosition = new Vector2(0, -rowHeight * i);
            entryTransform.gameObject.SetActive(true);

            rowTransform.Add(entryTransform);
        }


        refreshList();
    }
    public void refreshList()
    {
        setList(/*list*/);
    }

    private void setList()
    {
        int LeaderboardLenght = 5;

        for (int i = 0; i < 5; i++)
        {
            Transform childRank = rowTransform[i].GetChild(0);
            Transform childName = rowTransform[i].GetChild(1);
            Transform childTime = rowTransform[i].GetChild(2);

            if (i < LeaderboardLenght)
            {
                childRank.gameObject.GetComponent<Text>().text = (i + 1).ToString();
                childName.gameObject.GetComponent<Text>().text = "User " + (i + 1);
                childTime.gameObject.GetComponent<Text>().text = "22:00";
            }
            else
            {
                childRank.gameObject.GetComponent<TextMeshProUGUI>().text = "";
                childName.gameObject.GetComponent<TextMeshProUGUI>().text = "";
                childTime.gameObject.GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
}
