using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private GameObject txt_ProjectName;

    // Start is called before the first frame update
    void Start()
    {
        txt_ProjectName = GameObject.Find("txt_ProjectName");        
        txt_ProjectName.GetComponent<Text>().text = Global_Var.projectName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
