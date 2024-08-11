using UnityEngine;

public class Door : InteractableObject
{
    override
    public void Interact()
    {
        Debug.Log("Interactuando con " + itemName);
    }

    public Animator puerta;
    private bool enZona;
    private bool activa;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && enZona)
        {
            activa = !activa;
            if (activa == true)
            {
                puerta.SetBool("door", true);
            }
            if (activa == false)
            {
                puerta.SetBool("door", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
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
