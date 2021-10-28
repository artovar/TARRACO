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

    private int feet = 0;

    // Start is called before the first frame update
    void Start() {}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Foot"))
        {
            feet++;
            Transform jugador = other.transform.parent.parent.parent; //El jugador es el bisabuelo de las piernas

            ARP.APR.Scripts.APRController playerController = jugador.GetComponent<ARP.APR.Scripts.APRController>(); //Obtenemos el controlador ARP

            if(originalSpeed == 0){
                originalSpeed = playerController.moveSpeed; //Guarda la velocidad original para devolversela al salir
            }


            switch(type){
                case trapType.MUD: 
                    isMud(playerController);
                    break;
                case trapType.ICE:
                    isIce(playerController);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Foot"))
        {
            feet--;

            Transform jugador = other.transform.parent.transform.parent.transform.parent; //El jugador es el bisabuelo de las piernas
            ARP.APR.Scripts.APRController playerController = jugador.GetComponent<ARP.APR.Scripts.APRController>(); //Obtenemos el controlador ARP
            
            if (feet == 0) {
                playerController.moveSpeed = originalSpeed; //Reestablecemos la velocidad al salir
            } else if (feet == 1) {
                playerController.moveSpeed = originalSpeed - 3;
            }
        }
    }

    private void isMud(ARP.APR.Scripts.APRController playerController) {        
        playerController.moveSpeed -= 3;
    }

    private void isIce(ARP.APR.Scripts.APRController playerController) {        
        playerController.moveSpeed += 7;
    }
}
