using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedstairs;
    public GameObject stairs;

	void Update(){

		if(waitTime <= 0 && spawnedstairs == false){
			/*
			Instantiate(stairs, rooms[rooms.Count-1].transform.position, Quaternion.identity);
			spawnedstairs = true;
			*/
			for (int i = 0; i < rooms.Count; i++) {
				if(i == rooms.Count-1){
					Instantiate(stairs, rooms[i].transform.position, Quaternion.identity);
					spawnedstairs = true;
				}
			}
		} else {
			if(waitTime >= 0)
			{
			waitTime -= Time.deltaTime;
			}
		}
	}
}
