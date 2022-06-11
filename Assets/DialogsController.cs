using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogsController : MonoBehaviour
{


    [SerializeField]
    public GameObject panel;

    [SerializeField]
    public Text textDialog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Aceptar()
    {
        panel.SetActive(false);
    }
}
