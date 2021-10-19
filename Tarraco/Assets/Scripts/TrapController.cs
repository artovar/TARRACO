using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private float originalSpeed = 0;
    public Trap theTrap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Transform jugador = other.transform.parent.transform.parent.transform.parent; //El jugador es el bisabuelo de las piernas

            ARP.APR.Scripts.APRController playerController = jugador.GetComponent<ARP.APR.Scripts.APRController>(); //Obtenemos el controlador ARP
            
            if(playerController.moveSpeed > originalSpeed){
                originalSpeed = playerController.moveSpeed;
            }

            switch(theTrap.name){
                case "mud": 
                    isMud(playerController);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Transform jugador = other.transform.parent.transform.parent.transform.parent; //El jugador es el bisabuelo de las piernas
            ARP.APR.Scripts.APRController playerController = jugador.GetComponent<ARP.APR.Scripts.APRController>(); //Obtenemos el controlador ARP
            
            playerController.moveSpeed = originalSpeed; //Reestablecemos la velocidad al salir
        }
    }

    private void isMud(ARP.APR.Scripts.APRController playerController) {        
        playerController.moveSpeed -= 3;
    }
}
