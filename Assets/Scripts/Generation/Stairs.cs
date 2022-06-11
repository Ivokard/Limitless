using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public static bool onPlayerEnter = false;

    public void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Player")){
            Debug.Log("Escalera tocada por el jugador");
            onPlayerEnter = true;
		}
	}

}
