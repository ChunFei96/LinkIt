using System.Collections;
using System.Collections.Generic;


public static class Constants 
{
    public static class ScoreTable
    {
        public const string TABLE_NAME = "Score";
        public const string ID = "PatientId";
        public const string GAME_MODE = "GameMode";
        public const string TIME = "TimeTaken";
    }

    public static class PatientTable
    {
        public const string TABLE_NAME = "Patient";
        public const string ID = "PatientId";
        public const string NAME = "Name";
    }


    public static class SqliteCommand
    {
        public const string SelectAll = "SELECT * FROM ";
        public const string InsertInto = "INSERT INTO ";
        public const string Values = "VALUES ";
        public const string AUTO_INCREMENTAL = null; //pass null to AI field        

        public const string InsertPatient = "INSERT INTO Patient (PatientId,Name) VALUES (@id,@name)";
        public const string UpdatePatient = "UPDATE Patient SET Name = @name WHERE PatientId = @id ";
        public const string DeletePatient = "DELETE FROM Patient WHERE PatientId = @id";

        public const string InsertScore = "INSERT INTO Score (Id,PatientId,GameMode,TimeTaken) VALUES (@id,@patientId,@gameMode,@timeTaken)";
        public const string UpdateScore = "UPDATE Score SET GameMode = @gameMode, TimeTaken = @timeTaken WHERE PatientId = @patientId ";
        public const string DeleteScore = "DELETE FROM Score WHERE Id = @id";


    }
}
