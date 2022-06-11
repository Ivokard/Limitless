using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SQLite : MonoBehaviour
{

    [SerializeField]
    public InputField TxtUser;
    [SerializeField]
    public InputField TxtPassword;

    [SerializeField]
    public Text textErrorDialog;

    [SerializeField]
    public GameObject panel;


    // Start is called before the first frame update
    void Start()
    {
        try{
        
        // Create database
		string connection = "URI=file:" + Application.persistentDataPath + "/" + "PruebaUsuarios";
        Debug.Log(Application.persistentDataPath);
		
		// Open connection
		IDbConnection dbcon = new SqliteConnection(connection);
		dbcon.Open();


		// Create table
		IDbCommand dbcmd;
		dbcmd = dbcon.CreateCommand();
		string q_createTable = "CREATE TABLE IF NOT EXISTS users (username VARCHAR NOT NULL PRIMARY KEY , password VARCHAR NOT NULL)";

		dbcmd.CommandText = q_createTable;
		dbcmd.ExecuteReader();

		// Close connection
		dbcon.Close();

        }catch(Exception e)
        {
            Debug.Log(e);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void BotonRegistrar()
    {
        String username = TxtUser.text;       
        String pass = TxtPassword.text;
        try{
        // Create database
		string connection = "URI=file:" + Application.persistentDataPath + "/" + "PruebaUsuarios";
        //Debug.Log(Application.persistentDataPath);
		
		// Open connection
		IDbConnection dbcon = new SqliteConnection(connection);
		dbcon.Open();
       
        IDbCommand cmnd = dbcon.CreateCommand();
		cmnd.CommandText = "INSERT INTO users (username, password) VALUES ('"+ username +"', '"+ pass+"')";
		cmnd.ExecuteNonQuery();	
        Debug.Log("Usuario registrado");
        panel.SetActive(true);
        textErrorDialog.text = "Usuario registrado";
		// Close connection
		dbcon.Close();
        

        }
        catch(Mono.Data.Sqlite.SqliteException)
        {
            Debug.Log("El usuario ya existe");
            panel.SetActive(true);
            textErrorDialog.text = "El usuario ya existe";
        }
        
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void CerrarErrorDialog()
    {
        panel.SetActive(false);
    }
    public void BotonLogin()
    {
        String usernameDado = TxtUser.text;       
        String passDado = TxtPassword.text;
        String usernameBD = "", passBD = "";
        try{
        // Create database
		string connection = "URI=file:" + Application.persistentDataPath + "/" + "PruebaUsuarios";
        //Debug.Log(Application.persistentDataPath);
		
		// Open connection
		IDbConnection dbcon = new SqliteConnection(connection);
		dbcon.Open();

		// Read and print all values in table
		IDbCommand cmnd_read = dbcon.CreateCommand();
		IDataReader reader;
		string query ="SELECT username,password FROM USERS WHERE USERNAME = '" + usernameDado +"' AND PASSWORD= '"+ passDado + "'" ;
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();           
  
        while (reader.Read())
        {
    
            usernameBD = reader[0].ToString();
            passBD = reader[1].ToString();
            
        }

       
       
        if(usernameDado.Equals(usernameBD)  && passDado.Equals(passBD))
        {
            Debug.Log("Bienvenido " + usernameDado);
            PlayerPrefs.SetString("username",usernameDado);
            SceneManager.LoadScene("Menu");
        }
        else
        {
            Debug.Log("Datos incorrectos");
            panel.SetActive(true);
            textErrorDialog.text = "Datos incorrectos";
        }

		// Close connection
		dbcon.Close(); 
        }
        
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}
