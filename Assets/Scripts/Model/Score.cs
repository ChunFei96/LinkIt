using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;


public class Score{
    public int Id;
    public int PatientId;
    public string PatientName;
    public string GameMode;
    public string TimeTaken;
    
    public Score(int _PatientId, string _GameMode, string _TimeTaken){
        PatientId = _PatientId;
        GameMode = _GameMode;
        TimeTaken = _TimeTaken;
    }

    public Score(int _Id, int _PatientId, string _GameMode, string _TimeTaken)
    {
        Id = _Id;
        PatientId = _PatientId;
        GameMode = _GameMode;
        TimeTaken = _TimeTaken;
    }

   
}

public static class ScoreExtensions
{
    public static List<Score> SetPatientNameByPatientIds(this List<Score> scores, List<Patient> patients)
    {

        foreach(var i in scores)
        {
            Debug.Log("i.PatientId: " + i.PatientId);
            Patient patient = patients.Where(c => c.PatientId == i.PatientId).FirstOrDefault();

            if (patient != null)
            {
                i.PatientName = patient.Name;
            }
        }

        return scores;
    }
}
