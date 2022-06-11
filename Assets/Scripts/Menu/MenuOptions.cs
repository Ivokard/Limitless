using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MenuOptions : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text Txtwelcome;
    [SerializeField]
    public GameObject StatsPanel;

    public GameObject ScoreBoardPanel;
    
    void Start()
    {
        Txtwelcome.text= "Bienvenido " + PlayerPrefs.GetString("username");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Iniciar ()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Salir ()
    {
        Application.Quit();
        Debug.Log("Saliendo");
    }

    public void CerrarSesion()
    {
        SceneManager.LoadScene("Login");
    }

    public void CloseStatsPanel()
    {
        StatsPanel.SetActive(false);
    }
}
