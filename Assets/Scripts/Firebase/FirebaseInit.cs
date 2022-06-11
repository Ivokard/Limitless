using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class FirebaseInit : MonoBehaviour
{

    public DependencyStatus dependencyStatus;

    [SerializeField]
    public InputField TxtUser;
    [SerializeField]
    public InputField TxtPassword;

    [SerializeField]
    public Text textErrorDialog;

    [SerializeField]
    public GameObject panel;


    [SerializeField]
    public GameObject StatsPanel;

    [SerializeField]
    public TextMeshProUGUI txtEnemies;

    [SerializeField]
    public TextMeshProUGUI txtNpartidas;

    [SerializeField]
    public TextMeshProUGUI txtPuntuacion;

    public static string username;
    
    public static Firebase.Auth.FirebaseAuth auth;
    public static FirebaseFirestore db ;
    public static int puntuacion_alta, enemigos_eliminados,n_partidas;
    public static bool isLogged = false;
    
    private FirebaseApp app;
    // Start is called before the first frame update

    private void Awake() {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }
    void Start()
    {
        try{


            Debug.Log(isLogged);    
            if(PlayerPrefs.HasKey("username") == false && PlayerPrefs.HasKey("puntuacion_alta") == false &&
            PlayerPrefs.HasKey("enemigos_eliminados") == false && PlayerPrefs.HasKey("n_partidas") == false)
            {
                PlayerPrefs.SetString("username","NoLogin");
                PlayerPrefs.SetInt("puntuacion_alta",0);
                PlayerPrefs.SetInt("enemigos_eliminados",0);
                PlayerPrefs.SetInt("n_partidas",0);
            }

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                AnonLogin();
                if(isLogged==true)
                {                   
                    Debug.Log(username);

                    username = PlayerPrefs.GetString("Esta logusername");
                    GetStats();
                }
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
            });
        }        
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void AnonLogin()
    {
        try{
        if (auth.CurrentUser != null)
        {
            Debug.Log("Usuario ya autentificado");
            return;
        }

        auth.SignInAnonymouslyAsync().ContinueWith(task => {
        if (task.IsCanceled) {
            Debug.LogError("SignInAnonymouslyAsync was canceled.");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
            return;
        }

        Firebase.Auth.FirebaseUser newUser = task.Result;
        Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);
        });
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void CreateUsersSchema()
    {
        try{


            DocumentReference docRef = db.Collection("Usuarios").Document(TxtUser.text);
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                    { "id_jugador", auth.CurrentUser.UserId },
                    { "username", TxtUser.text },
                    { "password", TxtPassword.text },
            };
            docRef.SetAsync(user).ContinueWithOnMainThread(task => {
                    Debug.Log("Added data [Usuarios].");
            });
        }

        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void CreateComprasSchema()
    {
        try{

            DocumentReference docRef = db.Collection("Compras").Document(TxtUser.text);
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "id_jugador", auth.CurrentUser.UserId },
                { "username", TxtUser.text },
                { "version_juego", "Pago"},
            };
            docRef.SetAsync(user).ContinueWithOnMainThread(task => {
                    Debug.Log("Added data [Compras].");
            });
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void CreateTPuntuacionesSchema()
    {
        try{

            DocumentReference docRef = db.Collection("Tabla_Puntuaciones").Document(TxtUser.text);
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "id_jugador", auth.CurrentUser.UserId },
                { "username", TxtUser.text },
                { "puntuacion", 0},
            };
            docRef.SetAsync(user).ContinueWithOnMainThread(task => {
                    Debug.Log("Added data [Tabla_Puntuaciones].");
            });
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void CreateStatsSchema()
    {
        try{

            DocumentReference docRef = db.Collection("Stats").Document(TxtUser.text);
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "id_jugador", auth.CurrentUser.UserId },
                { "username", TxtUser.text },
                { "puntuacion_alta", 0},
                { "enemigos_eliminados",0},
                { "n_partidas",0},
            };
            docRef.SetAsync(user).ContinueWithOnMainThread(task => {
                    Debug.Log("Added data [Stats].");
            });
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
    
    public void Register()
    {
        try{
            String checkUsername = "";

            DocumentReference docRef = db.Collection("Usuarios").Document(TxtUser.text);
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists) {
                    Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                    Dictionary<string, object> user = snapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in user) {
                        if(pair.Key == "username")
                        {
                            checkUsername = pair.Value.ToString();
                        }
                    }


                    if(checkUsername == TxtUser.text )
                    {
                        
                        Debug.Log("Error: ya existe un usuario con ese nombre");
                        panel.SetActive(true);
                        textErrorDialog.text = "Error: ya existe un usuario con ese nombre";
                    }
            
                } else {
                    if (TxtUser.text.Length>=4 && TxtPassword.text.Length>=4)
                    {
                        Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));  
                        CreateUsersSchema();
                        CreateComprasSchema();
                        CreateTPuntuacionesSchema();
                        CreateStatsSchema();
                        panel.SetActive(true);
                        textErrorDialog.text = "Usuario registrado";
                    }
                    else
                    {
                        Debug.Log("Usuario o contraseña no válidos");
                    }
                }
            });
        }
        catch(Exception e)
        {
            panel.SetActive(true);
            textErrorDialog.text = "Error: los datos no son válidos."; 
            Debug.Log(e);
        }
 
    }

    public void CheckLogin()
    {
        try{

            String checkUsername = "";
            String checkPassword = "";
            DocumentReference docRef = db.Collection("Usuarios").Document(TxtUser.text);
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists) {
                    Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                    Dictionary<string, object> user = snapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in user) {

                    if(pair.Key == "username")
                    {
                        checkUsername = pair.Value.ToString();
                    }

                    if(pair.Key == "password")
                    {
                        checkPassword = pair.Value.ToString();
                    }

                    //Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                    }


                    if(checkUsername == TxtUser.text && checkPassword == TxtPassword.text)
                    {
                        Debug.Log("Login correcto");
                        username = TxtUser.text;
                        PlayerPrefs.SetString("username",username);
                        Debug.Log("USERNAME LOGIN" + username);
                        isLogged = true;
                        GetStats();
                        SceneManager.LoadScene("Menu");
                    }
                    else
                    {
                        Debug.Log("Error");  
                        panel.SetActive(true);
                        textErrorDialog.text = "Error: datos incorrectos."; 
                                  
                    }
                } else {
                    Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                }
            });
        }
        catch(Exception e)
        {
            panel.SetActive(true);
            textErrorDialog.text = "Error: datos incorrectos."; 
            Debug.Log(e);
        }
    }

   
    void GetStats()
    {

            DocumentReference docRef = db.Collection("Stats").Document(username);
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists) {
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> users = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in users) {
                    if(pair.Key == "username")
                    {
                        Debug.Log("rwar uawenMW");
                    }
                
                    if(pair.Key == "puntuacion_alta")
                    {
                        puntuacion_alta = Int32.Parse(pair.Value.ToString());
                        Debug.Log(puntuacion_alta);
                    }

                    if(pair.Key == "enemigos_eliminados")
                    {
                        enemigos_eliminados = Int32.Parse(pair.Value.ToString());
                        Debug.Log(enemigos_eliminados);
                    }

                    if(pair.Key == "n_partidas")
                    {
                        n_partidas = Int32.Parse(pair.Value.ToString());
                        Debug.Log(n_partidas);
                    }
                }
                txtEnemies.text = enemigos_eliminados.ToString();
                txtNpartidas.text = n_partidas.ToString();
                txtPuntuacion.text = puntuacion_alta.ToString();
            } else {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
            });
    
    }

    public void LoadStatsMenu()
    {
        StatsPanel.SetActive(true);
        GetStats();
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

