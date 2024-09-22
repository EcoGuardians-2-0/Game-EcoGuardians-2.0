
using UnityEngine;


public class WallColliderScript : MonoBehaviour
{
    public GameObject Alerta;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Alerta.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Alerta.SetActive(false);
    }
}

