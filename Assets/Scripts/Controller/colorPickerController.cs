using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;


[Serializable]
public class ColorEvent : UnityEvent<Color> { }

public class ColorPickerController : MonoBehaviour
{

    private Global_Var globalVar;
    RectTransform Rect;
    private GameObject colorPickTxt;
    Texture2D ColorTexture;
    //public ColorEvent OnColorPreview;
    public ColorEvent OnColorSelect;

    private GameObject colorPicker;

    private Boolean onClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        Rect = GetComponent<RectTransform>();
        ColorTexture = GetComponent<Image>().mainTexture as Texture2D;
        colorPicker = GameObject.Find("colorPicker");
    }

    // Update is called once per frame
    void AwakeXX()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition))
        {
            //Debug.Log("choose color");
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out delta);

            float width = Rect.rect.width;
            float height = Rect.rect.height;

            delta += new Vector2(width * .5f, height * .5f);

            float x = Mathf.Clamp(delta.x / width, 0f, 1f);
            float y = Mathf.Clamp(delta.y / height, 0f, 1f);

            int texX = Mathf.RoundToInt(x * ColorTexture.width);
            int texY = Mathf.RoundToInt(y * ColorTexture.height);


            Color color = ColorTexture.GetPixel(texX, texY);

            if (Input.GetMouseButton(0))
            {
                OnColorSelect?.Invoke(color);
                Debug.Log("click color" + color);
                onClicked = false;
            }
        }
    }

    public void OnMouseDown()
    {

        if (RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition))
        {
            Debug.Log("choose color");
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out delta);

            float width = Rect.rect.width;
            float height = Rect.rect.height;

            delta += new Vector2(width * .5f, height * .5f);

            float x = Mathf.Clamp(delta.x / width, 0f, 1f);
            float y = Mathf.Clamp(delta.y / height, 0f, 1f);

            int texX = Mathf.RoundToInt(x * ColorTexture.width);
            int texY = Mathf.RoundToInt(y * ColorTexture.height);

            Color color = ColorTexture.GetPixel(texX, texY);

            OnColorSelect?.Invoke(color);

            Global_Var.tempColor = color;

            Debug.Log("click color" + color);
        }
    }

    // public void OnMouseDown()
    //  {
    //     onClicked = true;

    //    globalVar.g_texture_X = texture_X;
    //    globalVar.g_texture_X = texture_X;

    //}

    public Color getColorPixel(int texX, int texY)
    {
        return ColorTexture.GetPixel(texX, texY);
    }
}
