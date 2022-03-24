using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
  public Color red = Color.red;
    public Camera cam;
 
     void Start()
     {
         cam = GetComponent<Camera>();
     }
 
     void Update()
     {
        
             cam.backgroundColor = red;
     }
    
}
