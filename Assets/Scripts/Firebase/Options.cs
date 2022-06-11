using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class Options : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseFirestore.DefaultInstance.Settings.PersistenceEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
