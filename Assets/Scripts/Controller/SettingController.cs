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
    private GameObject nodeColor;


    private string type;


    // Start is called before the first frame update
    void Start()
    {
        backgroundColor = GameObject.Find("img_BackgroundColor");
        nodeColor = GameObject.Find("img_NodeColor");
        colorPickerPopup = GameObject.Find("colorPickerPanel");
        colorPickerPopup.SetActive(false);

        backgroundColor.GetComponent<Image>().color = Global_Var.BGColor;
        nodeColor.GetComponent<Image>().color = Global_Var.NodeColor;
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
        type = "BG";
    }

    public void onClickNode()
    {
        Debug.Log("onClickNode");
        colorPickerPopup.SetActive(true);
        colorPreview = GameObject.Find("colorPreview");
        colorPreview.GetComponent<Image>().color = Global_Var.NodeColor;
        type = "NODE";

    }

    public void onClickOk()
    {
        if (type == "BG")
        {
            Global_Var.BGColor = Global_Var.tempColor;
            backgroundColor.GetComponent<Image>().color = Global_Var.BGColor;

            PageController.SetBGColor();
        }

        else if (type == "NODE")
        {
            Global_Var.NodeColor = Global_Var.tempColor;
            nodeColor.GetComponent<Image>().color = Global_Var.NodeColor;
        }

        colorPickerPopup.SetActive(false);
    }


    public void onClickCancel()
    {
        colorPickerPopup.SetActive(false);
    }

}
