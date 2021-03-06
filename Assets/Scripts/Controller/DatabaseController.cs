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
        var conn = "URI=file:" + Environment.CurrentDirectory + "/Assets/database/" + database_name;

        Debug.Log("dataPath: " + Environment.CurrentDirectory);
        Debug.Log("dataPath: " + Environment.CurrentDirectory + "/Assets/database/" + database_name);
        Debug.Log("URI=file:" + Environment.CurrentDirectory + "/Assets/database/" + database_name);
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

    public bool ValidPatientId(string patientId){

        Debug.Log("validPatientId patientId:" + patientId);
        string p = FindPatientIdByPatientId(patientId);

        if( p != null){
            
            Debug.Log("validPatientId:" + true);

            return true;
        }
        else {
            Debug.Log("validPatientId:" + false);
            return false;
        }        
    }

    public Patient FindPatientByPatientId(string patientId)
    {
        string query = Constants.SqliteCommand.SelectAll + Constants.PatientTable.TABLE_NAME + String.Format(" where PatientId='{0}'", patientId);
        Debug.Log("FindPatientIdByPatientName query:" + query);

        IDataReader data = ExecuteQuery(query);
        return PatientsToViewModel(data)?.ToArray()[0];
    }

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
        Debug.Log("query:" + query);
        IDataReader data = ExecuteQuery(query);
        return PatientsToViewModel(data);
    }

    public string? FindPatientIdByPatientName(string name)
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
    
    public string? FindPatientIdByPatientId(string patientId)
    {
        string query = Constants.SqliteCommand.SelectAll + Constants.PatientTable.TABLE_NAME + String.Format(" where PatientId='{0}'", patientId);
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
            int Id = data.GetInt32(0);
            string patientId = data.GetString(1);
            string name = data.GetString(2);

            Patient patient = new Patient(Id, patientId, name);
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
        return ScoresToViewModel(data, Constants.ScoreTable.TIME);
    }

    public List<Score> SelectScoresByPatientId(string id)
    {
        Debug.Log("SelectScoresByPatientId id:" + id);
        string query = Constants.SqliteCommand.SelectAll + Constants.ScoreTable.TABLE_NAME + " where PatientId=" + id;

        IDataReader data = ExecuteQuery(query);
        return ScoresToViewModel(data, Constants.ScoreTable.TIME);
    }

    public List<Score>? SelectTopScoresByPatientName(string name, int counts)
    {
        var PatientId = FindPatientIdByPatientName(name);
        if (PatientId != null)
        {
            var scores = SelectScoresByPatientId((string)PatientId);
            List<Score> scoreList = scores?.OrderByDescending(c => c.CreatedOn).Take(10).ToList();
            return scoreList;
        }
        
        return null;
    }

     public List<Score> SelectScoreLeaderboard(string patientId, string sortBy)
    {
        Debug.Log("SelectScoreLeaderboard");

        string where = "";
        if( !string.IsNullOrEmpty(patientId)) {
            where = String.Format(" where PatientId='{0}'", patientId);
        }

        string query = Constants.SqliteCommand.SelectAll + Constants.ScoreTable.TABLE_NAME + where;
        Debug.Log("query: " + query);

        IDataReader data = ExecuteQuery(query);
        List<Score> scoreList = ScoresToViewModel(data, sortBy);

        return scoreList;
    }

    public List<Score> SelectScoresByGameMode(string gameMode)
    {
        Debug.Log("SelectScoresByGameMode: " + gameMode);
        string query = Constants.SqliteCommand.SelectAll + Constants.ScoreTable.TABLE_NAME + " where GameMode=" + gameMode;

        IDataReader data = ExecuteQuery(query);
        return ScoresToViewModel(data, Constants.ScoreTable.TIME);
    }

    public void AddScore(Score score)
    {
        string query = Constants.SqliteCommand.InsertScore;
        string strCreatedOn = score.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@id", Constants.SqliteCommand.AUTO_INCREMENTAL));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@patientId", score.PatientId.ToString()));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@gameMode", score.GameMode));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@timeTaken", score.TimeTaken));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@createdOn", strCreatedOn));
        ExecuteNonQuery(dbcmd);
        Debug.Log("Added new score for patientId :" + score.PatientId);
    }

    public void UpdateScore(Score score)
    {
        string query = Constants.SqliteCommand.UpdateScore;
        string strCreatedOn = score.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@id", score.Id.ToString()));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@patientId", score.PatientId.ToString()));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@gameMode", score.GameMode));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@timeTaken", score.TimeTaken));
        dbcmd.Parameters.Add(CreateParameter(dbcmd, "@createdOn", strCreatedOn));
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

    private List<Score> ScoresToViewModel(IDataReader data, string sortby)
    {
        List<Score> scores = new List<Score>();
        try
        {
            while (data.Read())
            {
                int id = data.GetInt32(0);
                string patientId = data.GetString(1);
                string gameMode = data.GetString(2);
                string timeTaken = data.GetString(3);
                string createdOn = data.GetString(4);

                DateTime ConvertcreatedOnDateTime = Convert.ToDateTime(createdOn);

                Score score = new Score(id, patientId, gameMode, timeTaken, ConvertcreatedOnDateTime);
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

        //Apply Sorting: Constants.ScoreTable.TIME or Constants.ScoreTable.CREATE_ON
        if (sortby == Constants.ScoreTable.CREATE_ON)
        {
            scores = scores.OrderByDescending(c => c.CreatedOn).ToList();
        }
        else
        {
            scores = scores.OrderBy(c => c.TimeTaken).ToList();
        }

        if (scores.Count > 10)
        {
            scores = scores.Take(10).ToList();
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
