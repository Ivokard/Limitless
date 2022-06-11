/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FirebaseOnGame : MonoBehaviour
{
    private FirebaseApp app;
    private String username;
    private Firebase.Auth.FirebaseAuth auth;
    private FirebaseFirestore db ;
    public static int puntuacion_alta, enemigos_eliminados,n_partidas;
    // Start is called before the first frame update
    void Start()
    {            
        db = FirebaseFirestore.DefaultInstance;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        #if UNITY_EDITOR
        #endif
        try{
            username = PlayerPrefs.GetString("username");
            GetStats();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    void GetStats()
    {

        try{

            DocumentReference docRef = db.Collection("Usuarios").Document(username);
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists) {
                    Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                    Dictionary<string, object> user = snapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in user) {

                        if(pair.Key == "puntuacion_alta")
                        {
                            puntuacion_alta = (int)pair.Value;
                        }

                        if(pair.Key == "enemigos_eliminados")
                        {
                            enemigos_eliminados = (int)pair.Value;
                        }

                        if(pair.Key == "n_partidas")
                        {
                            n_partidas = (int)pair.Value;
                        }
                    }

                } else {
                    Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                }
            });
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    
    }

    public void UpdateStats()
    {            
           try{
        
            DocumentReference docRef = db.Collection("Stats").Document(username);
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "puntuacion_alta",puntuacion_alta},
                { "enemigos_eliminados",enemigos_eliminados},
                { "n_partidas",n_partidas},
            };
            docRef.UpdateAsync(user).ContinueWithOnMainThread(task => {
                Debug.Log("Stats personales actualizadas.");
            });

            DocumentReference docScoreBoard = db.Collection("Tabla_Puntuaciones").Document(username);
            Dictionary<string, object> userScoreBoard = new Dictionary<string, object>
            {
                { "puntuacion",puntuacion_alta},
            };
            docScoreBoard.UpdateAsync(userScoreBoard).ContinueWithOnMainThread(task => {
                    Debug.Log("Stats tabla actualizadas.");
            });




        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

    }

    public void ScoreboardData()
    {
        try{
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Query Query = db.Collection("Tabla_Puntuaciones").OrderByDescending("puntuacion").Limit(10);
            Query.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            QuerySnapshot QuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in QuerySnapshot.Documents) {
                Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> user = documentSnapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in user) {             
                        if(pair.Key == "username")
                        {
                            Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                        }

                        if(pair.Key == "puntuacion")
                        {
                            Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                        }
                }

                // Newline to separate entries
                Debug.Log("");
            };
            }); 
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

}
*/