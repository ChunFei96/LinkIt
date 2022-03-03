using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public class DatabaseController : MonoBehaviour
{

    public static IDbConnection dbconn;
    public List<Score> dbScores;
    public List<Patient> dbPatients;

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
        //Debug.Log("test ConnectDb");
        var database_name = "LinkItDb.db";
        var conn = "URI=file:" + Application.dataPath + "/database/" + database_name;

        //Debug.Log("conn: " + conn);
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        //Init data pulling for Patients + Scores table
        if (dbPatients == null || dbPatients.Count == 0)
        {
            dbPatients = SelectAllPatients();

            Debug.Log("dbPatients count ---> " + dbPatients.Count);
            Debug.Log("ConnectDb do SelectAllPatients()");
        }

        if (dbScores == null || dbScores.Count == 0)
        {
            dbScores = SelectAllScores();
            Debug.Log("dbScores count ---> " + dbScores.Count);
            //Debug.Log("ConnectDb do SelectAllScores()");
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
        string query = Constants.SqliteCommand.SelectAll + Constants.PatientTable.TABLE_NAME + " where PatientId=" + id;

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

        Debug.Log("Total patients: " + patients.Count);
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
        while (data.Read())
        {
            int id = data.GetInt32(0);
            int patientId = data.GetInt32(1);
            string gameMode = data.GetString(2);
            string timeTaken = data.GetString(3);

            Score score = new Score(id, patientId, gameMode, timeTaken);
            scores.Add(score);

            //Debug.Log("Print id: " + id + "  patientId: " + patientId 
            //    + "  gameMode: " + gameMode + "  timeTaken: " + timeTaken);
        }

        data.Close();

        //todo update Patient name for display
        scores = scores.SetPatientNameByPatientIds(dbPatients);

        Debug.Log("Total Scores: " + scores.Count);
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
            Debug.Log("ExecuteQuery Exception: " + e.Message);
        }
        return dbcmd.ExecuteReader();
    }

    //ExecuteNonQuery for queries that does not return any data. Such as update, insert, delete
    private void ExecuteNonQuery(IDbCommand dbcmd)
    {
        bool querystatus = dbcmd.ExecuteNonQuery() != 0; //result '0' indicates the sql operation has failed
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
