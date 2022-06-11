using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        try{
            if(other.gameObject.tag!="Player"){
                Destroy(other.gameObject);
                StartCoroutine(DestroyThis());
            }
        }

        catch(Exception e)
        {
            Debug.Log(e);
        }
    }


    private IEnumerator DestroyThis()
    {              
        yield return new WaitForSeconds(0.3f);
        Destroy (this.gameObject);
    }

    private void Start() {
        try{
            StartCoroutine(DestroyThis());
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

}
