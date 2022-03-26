using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;


[Serializable]
public class ColorSelEvent : UnityEvent<Color> { }



public class SettingController : MonoBehaviour
{
    private GameObject colorPickerPopup;
    private GameObject colorPreview;
    private GameObject backgroundColor;
    private GameObject curPositionNodeColor;
    private GameObject unsuccessNodeColor;

    private string type;
    private string type_BG = "BG";
    private string type_cNode = "cNode";
    private string type_uNode = "uNode";


    // Start is called before the first frame update
    void Start()
    {
        backgroundColor = GameObject.Find("img_BackgroundColor");
        curPositionNodeColor = GameObject.Find("img_CurPositionNodeColor");
        unsuccessNodeColor = GameObject.Find("img_UnsuccessNodeColor");

        colorPickerPopup = GameObject.Find("colorPickerPanel");
        colorPickerPopup.SetActive(false);

        backgroundColor.GetComponent<Image>().color = Global_Var.BGColor;
        curPositionNodeColor.GetComponent<Image>().color = Global_Var.CurPositionNodeColor;
        unsuccessNodeColor.GetComponent<Image>().color = Global_Var.UnsuccessNodeColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClickBackground()
    {
        Debug.Log("onClickBackground");
        colorPickerPopup.SetActive(true);
        colorPreview = GameObject.Find("colorPreview");
        colorPreview.GetComponent<Image>().color = Global_Var.BGColor;
        type = type_BG;
    }

    public void onClickCurPositionNode()
    {
        Debug.Log("onClickCurPositionNode");
        colorPickerPopup.SetActive(true);
        colorPreview = GameObject.Find("colorPreview");
        colorPreview.GetComponent<Image>().color = Global_Var.CurPositionNodeColor;
        type = type_cNode;
    }

    public void onClickUnsuccessNode()
    {
        Debug.Log("onClickUnsuccessNode");
        colorPickerPopup.SetActive(true);
        colorPreview = GameObject.Find("colorPreview");
        colorPreview.GetComponent<Image>().color = Global_Var.UnsuccessNodeColor;
        type = type_uNode;
    }

    public void onClickOk()
    {
        if (type == type_BG)
        {
            Global_Var.BGColor = Global_Var.tempColor;
            backgroundColor.GetComponent<Image>().color = Global_Var.BGColor;
            PageController.SetBGColor();
        }

        else if (type == type_cNode)
        {
            Global_Var.CurPositionNodeColor = Global_Var.tempColor;
            curPositionNodeColor.GetComponent<Image>().color = Global_Var.CurPositionNodeColor;
        }

        else if (type == type_uNode)
        {
            Global_Var.UnsuccessNodeColor = Global_Var.tempColor;
            unsuccessNodeColor.GetComponent<Image>().color = Global_Var.UnsuccessNodeColor;
        }

        colorPickerPopup.SetActive(false);
    }


    public void onClickCancel()
    {
        colorPickerPopup.SetActive(false);
    }

}
