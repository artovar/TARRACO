using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public enum trapType
    {
        MUD,
        ICE,
        FIRE
    }
    public trapType type;

    private float originalSpeed = 0;

    // Start is called before the first frame update
    void Start() {}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Transform jugador = other.transform.parent.transform.parent.transform.parent; //El jugador es el bisabuelo de las piernas

            ARP.APR.Scripts.APRController playerController = jugador.GetComponent<ARP.APR.Scripts.APRController>(); //Obtenemos el controlador ARP

            if(originalSpeed == 0){
                originalSpeed = playerController.moveSpeed;
            }

            switch(type.ToString()){
                case "MUD": 
                    isMud(playerController);
                    break;
                case "ICE":
                    isIce(playerController);
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

    private void isIce(ARP.APR.Scripts.APRController playerController) {        
        playerController.moveSpeed += 7;
    }
}
