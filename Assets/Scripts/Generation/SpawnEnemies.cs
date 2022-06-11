using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject respawnSlime;
    public GameObject respawn;
    int spawnChances;
   
    private void Update() {  
        respawn = GameObject.FindWithTag("EnemySP");
        if(respawn != null)
        {
            spawnChances = Random.Range(0,3);

            if(spawnChances == 1)
            {
                Instantiate(respawnSlime, respawn.transform.position, respawn.transform.rotation);
                Destroy(respawn); 
            }

        }
    }
}
