using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DatabaseController db = new DatabaseController();

        #region Patient Test Data
        db.SelectAllPatients();
        db.SelectPatientById(2);

        Patient patient_a = new Patient("Jack");
        db.AddPatient(patient_a);

        Patient existing_patient = new Patient(4,"Bollo");
        db.UpdatePatient(existing_patient);

        db.DeletePatient(5);
        #endregion

        #region Score Test Data

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
