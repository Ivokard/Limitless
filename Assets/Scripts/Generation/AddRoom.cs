using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;

    void Start() {
        try{
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            templates.rooms.Add(this.gameObject);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}
