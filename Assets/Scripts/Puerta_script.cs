using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Puerta_script : MonoBehaviour
{
    public Animator puerta;
    private bool enZona;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (enZona == true)
            {
                puerta.SetBool("", true);
            }
            if (enZona == false)
            {
                puerta.SetBool("", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            enZona = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enZona = false;
        }
    }
}
