using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection;


    private RoomTemplates templates;
    private int rng;
    private bool spawned = false;
	

    public float waitTime = 4f;
    void Start() {
		try{
			Destroy(gameObject,waitTime);
			templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
			Invoke("Spawn",0.1f);
		}
		catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }
    // Update is called once per frame
    void Spawn()
    {
		try{
			if(spawned == false)
			{
				if(openingDirection == 1){
					// Need to spawn a room with a BOTTOM door.
					rng = UnityEngine.Random.Range(0, templates.bottomRooms.Length);
					Instantiate(templates.bottomRooms[rng], transform.position, templates.bottomRooms[rng].transform.rotation);
				} else if(openingDirection == 2){
					// Need to spawn a room with a TOP door.
					rng = UnityEngine.Random.Range(0, templates.topRooms.Length);
					Instantiate(templates.topRooms[rng], transform.position, templates.topRooms[rng].transform.rotation);
				} else if(openingDirection == 3){
					// Need to spawn a room with a LEFT door.
					rng = UnityEngine.Random.Range(0, templates.leftRooms.Length);
					Instantiate(templates.leftRooms[rng], transform.position, templates.leftRooms[rng].transform.rotation);
				} else if(openingDirection == 4){
					// Need to spawn a room with a RIGHT door.
					rng = UnityEngine.Random.Range(0, templates.rightRooms.Length);
					Instantiate(templates.rightRooms[rng], transform.position, templates.rightRooms[rng].transform.rotation);
				}
				spawned = true;
			}
		}
		catch(Exception e)
        {
            Debug.Log(e);
        }

    }

	void OnTriggerEnter2D(Collider2D other){
		try{
			if(other.CompareTag("SpawnPoint")){
				if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false){
					templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
					Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
					Destroy(gameObject);
				} 
				spawned = true;
			}
		}
		catch(Exception e)
        {
            Debug.Log(e);
        }
	}
}
