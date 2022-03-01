using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Score{
    public int Id;
    public int PatientId;
    public string GameMode;
    public DateTime TimeTaken;
    
    public Score(int _PatientId, string _GameMode,DateTime _TimeTaken){
        PatientId = _PatientId;
        GameMode = _GameMode;
        TimeTaken = _TimeTaken;
    }

    public Score(int _Id, int _PatientId, string _GameMode, DateTime _TimeTaken)
    {
        Id = _Id;
        PatientId = _PatientId;
        GameMode = _GameMode;
        TimeTaken = _TimeTaken;
    }


}