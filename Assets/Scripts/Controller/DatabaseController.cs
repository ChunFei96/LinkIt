using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.Linq;


public class DatabaseController : MonoBehaviour
{

    public static IDbConnection dbconn;
    public static List<Score> dbScores;
    public static List<Patient> dbPatients;

    public DatabaseController()
    {
        
    }

    //https://answers.unity.com/questions/743400/database-sqlite-setup-for-unity.html


    public void InitDb()
    {
        if (dbconn == null)
        {
            //Debug.Log("dbconn is null");
            ConnectDb();
        }
        //Debug.Log("test DatabaseController");
    }

    public void ConnectDb()
    {
        var database_name = "LinkItDb.db";
        var conn = "URI=file:" + Application.dataPath + "/database/" + database_name;

        //Debug.Log("conn: " + conn);
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        //Init data pulling for Patients + Scores table
        if (dbPatients == null || dbPatients.Count == 0)
        {
            dbPatients = SelectAllPatients();
            //Debug.Log("dbPatients count ---> " + dbPatients.Count);
        }

        if (dbScores == null || dbScores.Count == 0)
        {
            dbScores = SelectAllScores();
            //Debug.Log("dbScores count ---> " + dbScores.Count);
        }
    }

    ~DatabaseController() 
    {
        //dbconn.Close();
        //dbconn = null;
        //Debug.Log("calling destructor");
    }

    public void TerminatetDb()
    {
        dbconn.Close();
        dbconn = null;
        Debug.Log("TerminatetDb");
    }

    #region Patients
    public List<Patient> SelectAllPatients()
    {
        string query = Constants.SqliteCommand.SelectAll + Constants.PatientTable.TABLE_NAME;

        IDataReader data = ExecuteQuery(query);
        return PatientsToViewModel(data);
    }

    public Patient SelectPatientById(int id)
    {
        Debug.Log("SelectPatientById id:" + id);
        string query = Constants.SqliteCommand.SelectAll + Constants.PatientTable.TABLE_NAME + String.Format(" where PatientId='{0}'", id);

        IDataReader data = ExecuteQuery(query);
        return PatientsToViewModel(data)?.ToArray()[0];
    }

    public List<Patient> SelectPatientsByIds(List<int> ids)
    {
        Debug.Log("SelectPatientsByIds id:" + ids);
        string query = Constants.SqliteCommand.SelectAll + Constants.PatientTable.TABLE_NAME + " where PatientId in (" + string.Join(",", ids) + ")";

        IDataReader data = ExecuteQuery(query);
        return PatientsToViewModel(data);
    }

    public int? FindPatientIdByPatientName(string name)
    {
        string query = Constants.SqliteCommand.SelectAll + Constants.PatientTable.TABLE_NAME + String.Format(" where Name='{0}'", name);
        Debug.Log("FindPatientIdByPatientName query:" + query);

        IDataReader data = ExecuteQuery(query);

        var patients = PatientsToViewModel(data);
        if (patients != null && patients.Count > 0)
        {
            var Patient = patients.ToList()[0];
            return Patient.PatientId; 
        }
        else
        {
            return null;
        }
    }

    public void AddPatient(Patient patient)
    {
        string query = Constants.SqliteCommand.InsertPatient;

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@id", Constants.SqliteCommand.AUTO_INCREMENTAL));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@name", patient.Name));
        ExecuteNonQuery(dbcmd);
        Debug.Log("Added new patient:" + patient.Name);
    }

    public void UpdatePatient(Patient patient)
    {
        string query = Constants.SqliteCommand.UpdatePatient;

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@id", patient.PatientId.ToString()));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@name", patient.Name));
        ExecuteNonQuery(dbcmd);

        Debug.Log("Updated patient " + patient.Name + "'s info.");
    }

    public void DeletePatient(int id)
    {
        string query = Constants.SqliteCommand.DeletePatient;

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@id", id.ToString()));
        ExecuteNonQuery(dbcmd);

        Debug.Log("PatientId " + id + "has been deleted.");
    }

    private List<Patient> PatientsToViewModel(IDataReader data)
    {
        List<Patient> patients = new List<Patient>();
        while (data.Read())
        {
            int id = data.GetInt32(0);
            string name = data.GetString(1);

            Patient patient = new Patient(id, name);
            patients.Add(patient);

            //Debug.Log("Print id: " + id + "  name: " + name);
        }

        data.Close();

        //Debug.Log("Total patients: " + patients.Count);
        return patients;
    }
    #endregion

    #region Scores
    public List<Score> SelectAllScores()
    {
        string query = Constants.SqliteCommand.SelectAll + Constants.ScoreTable.TABLE_NAME;
        //Debug.Log("SelectAllScores query " + query);
        IDataReader data = ExecuteQuery(query);
        return ScoresToViewModel(data);
    }

    public List<Score> SelectScoresByPatientId(int id)
    {
        Debug.Log("SelectScoresByPatientId id:" + id);
        string query = Constants.SqliteCommand.SelectAll + Constants.ScoreTable.TABLE_NAME + " where PatientId=" + id;

        IDataReader data = ExecuteQuery(query);
        return ScoresToViewModel(data);
    }

    public List<Score>? SelectTopScoresByPatientName(string name, int counts)
    {
        var PatientId = FindPatientIdByPatientName(name);
        if (PatientId != null)
        {
            var scores = SelectScoresByPatientId((int)PatientId);
            List<Score> scoreList = scores?.OrderByDescending(c => DateTime.Parse(c.CreatedOn)).Take(10).ToList();
            return scoreList;
        }
        
        return null;
    }

     public List<Score> SelectScoreLeaderboard(string name, string sortBy)
    {
        Debug.Log("SelectScoreLeaderboard");

        string where = "";
        var PatientId = FindPatientIdByPatientName(name);
        if(PatientId.HasValue) {
            where = " where PatientId=" + PatientId;
        }

        string orderBy = "";

        if(sortBy.Length>0 && sortBy == Constants.ScoreTable.TIME){
            orderBy = " order by " + Constants.ScoreTable.TIME + " ASC";
        }

        else if(sortBy.Length>0 && sortBy == Constants.ScoreTable.CREATE_ON){
            orderBy = " order by " + Constants.ScoreTable.CREATE_ON + " DESC";
        }
         
        string query = Constants.SqliteCommand.SelectAll + Constants.ScoreTable.TABLE_NAME + where + orderBy;
        Debug.Log("query: " + query);

        IDataReader data = ExecuteQuery(query);
        List<Score> scoreList = ScoresToViewModel(data);

        return scoreList;
    }

    public List<Score> SelectScoresByGameMode(string gameMode)
    {
        Debug.Log("SelectScoresByGameMode: " + gameMode);
        string query = Constants.SqliteCommand.SelectAll + Constants.ScoreTable.TABLE_NAME + " where GameMode=" + gameMode;

        IDataReader data = ExecuteQuery(query);
        return ScoresToViewModel(data);
    }

    public void AddScore(Score score)
    {
        string query = Constants.SqliteCommand.InsertScore;

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@id", Constants.SqliteCommand.AUTO_INCREMENTAL));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@patientId", score.PatientId.ToString()));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@gameMode", score.GameMode));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@timeTaken", score.TimeTaken));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@createdOn", score.CreatedOn));
        ExecuteNonQuery(dbcmd);
        Debug.Log("Added new score for patientId :" + score.PatientId);
    }

    public void UpdateScore(Score score)
    {
        string query = Constants.SqliteCommand.UpdateScore;

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@id", score.Id.ToString()));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@patientId", score.PatientId.ToString()));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@gameMode", score.GameMode));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@timeTaken", score.TimeTaken));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@createdOn", score.CreatedOn));
        ExecuteNonQuery(dbcmd);

        Debug.Log("Updated Score Id " + score.Id + "'s info.");
    }

    public void DeleteScore(int id)
    {
        string query = Constants.SqliteCommand.DeleteScore;

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@id", id.ToString()));
        ExecuteNonQuery(dbcmd);

        Debug.Log("Score Id " + id + "has been deleted.");
    }

    private List<Score> ScoresToViewModel(IDataReader data)
    {
        List<Score> scores = new List<Score>();
        try
        {
            while (data.Read())
            {
                int id = data.GetInt32(0);
                int patientId = data.GetInt32(1);
                string gameMode = data.GetString(2);
                string timeTaken = data.GetString(3);
                string createdOn = data.GetString(4);

                Score score = new Score(id, patientId, gameMode, timeTaken, createdOn);
                scores.Add(score);
            }

            data.Close();
            scores = scores.SetPatientNameByPatientIds(dbPatients);
            Debug.Log("Total Scores: " + scores.Count);
        }
        catch (Exception e)
        {
            Debug.Log("ScoresToViewModel exception: " + e.Message);
        }
        return scores;
    }

    #endregion

    #region Common Methods
    //Reference: http://csharp.net-informations.com/data-providers/csharp-executereader-executenonquery.htm

    //ExecuteQuery for queries that getting data from database.
    private IDataReader ExecuteQuery(string query)
    {
        IDbCommand dbcmd = null;

        try
        {
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = query;
            
        }
        catch(Exception e)
        {
            dbcmd.Transaction.Rollback();
            Debug.Log("ExecuteQuery Exception: " + e.Message);
        }
        return dbcmd.ExecuteReader();
    }

    //ExecuteNonQuery for queries that does not return any data. Such as update, insert, delete
    private void ExecuteNonQuery(IDbCommand dbcmd)
    {
        try
        {
            bool querystatus = dbcmd.ExecuteNonQuery() != 0; //result '0' indicates the sql operation has failed
        }
        catch (Exception e)
        {
            dbcmd.Transaction.Rollback();
            Debug.Log("ExecuteQuery Exception: " + e.Message);
        }
    }

    private IDbDataParameter CreateParameter(IDbCommand dbcmd, string columnName, string value)
    {
        IDbDataParameter parameter = dbcmd.CreateParameter();
        parameter.ParameterName = columnName;
        parameter.Value = value;
        return parameter;
    }

    #endregion

    //parameterized sqlite 
    //https://stackoverflow.com/questions/2662999/system-data-sqlite-parameterized-queries-with-multiple-values

    //Best practices
    //https://stackoverflow.com/questions/8234971/adding-parameters-to-idbcommand
    //https://github.com/ktaranov/naming-convention/blob/master/C%23%20Coding%20Standards%20and%20Naming%20Conventions.md
}
