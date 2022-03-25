using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[SerializeField]
public class ColorEvent : UnityEvent<Color> { }
public class colorPickerController : MonoBehaviour
{
    RectTransform Rect;
    Texture2D colorTexture;
    private static GameObject colorPick;

    public ColorEvent OnColorPreview;
    public ColorEvent OnColorSelect;
    

    // Start is called before the first frame update
    void Start()
    {
        Rect = GameObject.Find("colorPicker").GetComponent<RectTransform>();


        colorTexture = GameObject.Find("colorPicker").GetComponent<Image>().mainTexture as Texture2D;

        colorPick = GameObject.Find("txt_colorPick");
        
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 delta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out delta);

        
        colorPick.GetComponent<Text>().text = "Position: " + delta;

        float width = Rect.rect.width;
        
        float height = Rect.rect.height;
        

        delta += new Vector2(width * .5f, height * .5f);
        
        float x = Mathf.Clamp(delta.x / width, 0f, 1f);
        
        float y = Mathf.Clamp(delta.x / height, 0f, 1f);


        int texX = Mathf.RoundToInt(x * colorTexture.width);
        int texY = Mathf.RoundToInt(y * colorTexture.width);

        Color color = colorTexture.GetPixel(texX, texY);

        colorPick.GetComponent<Text>().text = "Pick: " + delta;
        colorPick.GetComponent<Text>().text = "x: " + texX+ "y:" + texY;


        //colorPreview.GetComponent<Image>().color = color;

        OnColorPreview?.Invoke(color);
        if(Input.GetMouseButton(0)){
            OnColorSelect?.Invoke(color);
        }

    }
}
