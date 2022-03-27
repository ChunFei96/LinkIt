using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;


public class Score{
    public int Id;
    public string PatientId;
    public string PatientName;
    public string GameMode;
    public string TimeTaken;
    //public string CreatedOn;
    public DateTime CreatedOn;
    
    public Score(string _PatientId, string _GameMode, string _TimeTaken, DateTime _CreatedOn)
    {
        PatientId = _PatientId;
        GameMode = _GameMode;
        TimeTaken = _TimeTaken;
        CreatedOn = _CreatedOn;
    }

    public Score(int _Id, string _PatientId, string _GameMode, string _TimeTaken, DateTime _CreatedOn)
    {
        Id = _Id;
        PatientId = _PatientId;
        GameMode = _GameMode;
        TimeTaken = _TimeTaken;
        CreatedOn = _CreatedOn;
    }

   
}

public static class ScoreExtensions
{
    public static List<Score> SetPatientNameByPatientIds(this List<Score> scores, List<Patient> patients)
    {
        if (patients != null && patients.Count > 0)
        {
            foreach (var i in scores)
            {
                Debug.Log("i.PatientId: " + i.PatientId);
                Patient patient = patients.Where(c => c.PatientId == i.PatientId).FirstOrDefault();

                if (patient != null)
                {
                    i.PatientName = patient.Name;
                }
            }
        }
        
        return scores;
    }
}
