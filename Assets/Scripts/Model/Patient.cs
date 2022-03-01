using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    
public class Patient{
    public int PatientId;
    public string Name;

    public Patient(string _Name)
    {
        Name = _Name;
    }

    public Patient(int _PatientId, string _Name){
        PatientId = _PatientId;
        Name = _Name;
    }


}