using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtPiso;
    [SerializeField]
    private TextMeshProUGUI txtFloorLoad;
    [SerializeField]
    private Image imgFloorLoad;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject EndRunPanel;

    private GameObject[] enemies;
    private GameObject[] closedRooms;
    [SerializeField]
    private GameObject entryRoom;
    private GameObject entryRoom_Temp;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject roomTemplate;
    private RoomTemplates templates;
    private GameObject RoomTemplates_Temp;
    public static bool gamePaused;
    private FirebaseInit dataOnGame = new FirebaseInit();
    private bool dataUpdated = false;
    int piso;
    IEnumerator Fade()
    {
        imgFloorLoad.gameObject.SetActive(true);
        float alphaImg = imgFloorLoad.GetComponent<CanvasGroup>().alpha;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
        {
            alphaImg = alpha;
            imgFloorLoad.GetComponent<CanvasGroup>().alpha = alphaImg;
            //yield return new WaitForSeconds(.1f); 
            yield return null;
        }
        imgFloorLoad.gameObject.SetActive(false); 
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            EndRunPanel.SetActive(false);
            dataOnGame = FindObjectOfType<FirebaseInit>();
            Time.timeScale = 1f;
            Application.targetFrameRate = 60;
            imgFloorLoad.gameObject.SetActive(true);
            RoomTemplates_Temp = Instantiate (roomTemplate);
            entryRoom_Temp = Instantiate(entryRoom);
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();   
            piso = 0;
            txtFloorLoad.text = "Piso " + piso;
            txtPiso.text = "Piso " + piso;
            Stairs.onPlayerEnter = false;
            StartCoroutine(Fade()); 
        }     
        catch(System.Exception)  
        {
            throw;
        }
     
    }

    void Update()
    {
        try
        {
            if(Player.PlayerHP <=0)
            {
                EndRunPanel.SetActive(true);
                Time.timeScale = 0f;

                if(dataUpdated == false){
                    if(piso> FirebaseInit.puntuacion_alta)
                    {
                        FirebaseInit.puntuacion_alta = piso;
                    }
                    FirebaseInit.n_partidas++;
                    dataOnGame.UpdateStats();
                    dataUpdated = true;
                }
            }




            if(Stairs.onPlayerEnter == true)
            {           

                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++)
                {
                    Destroy(enemies[i]);
                }



                StartCoroutine(Fade()); 

                templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();   
                Destroy(entryRoom_Temp);
                player.transform.position = new Vector3(0,2,0);
                closedRooms = GameObject.FindGameObjectsWithTag("EmptyRoom");
                for(int i = 0; i< closedRooms.Length; i++)
                {
                    Destroy(closedRooms[i]);
                }

                foreach(GameObject go in templates.rooms)
                {
                    Destroy(go);
                }           
                templates.rooms.Clear();
                Destroy(RoomTemplates_Temp);


                RoomTemplates_Temp = Instantiate (roomTemplate);
                entryRoom_Temp = Instantiate(entryRoom);      
                piso++;      
                txtFloorLoad.text = "Piso " + piso;              
                txtPiso.text = "Piso " + piso;
                Stairs.onPlayerEnter = false;
        }            
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

    }

}
