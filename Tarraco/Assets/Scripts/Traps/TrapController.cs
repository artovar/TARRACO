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
    void Start() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            print("I'm entering bro");
            //feet++;
            CharacterClass playerController = other.GetComponentInParent<CharacterClass>();

            if (originalSpeed == 0)
            {
                originalSpeed = playerController.moveSpeed; //Guarda la velocidad original para devolversela al salir
            }


            switch (type)
            {
                case trapType.MUD:
                    IsMud(playerController);
                    break;
                case trapType.ICE:
                    IsIce(playerController);
                    break;
            }
            print("My new speed is " + playerController.moveSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            CharacterClass playerController = other.GetComponentInParent<CharacterClass>();

            print("I'm exiting bro");
            /*feet--;
            if (feet == 0)
            {
                playerController.moveSpeed = originalSpeed; //Reestablecemos la velocidad al salir
            }
            else if (feet == 1)
            {*/
            playerController.moveSpeed *= 2f;
            //}
            print("My new speed is " + playerController.moveSpeed);
        }
    }

    private void IsMud(CharacterClass playerController)
    {
        playerController.moveSpeed /=2;
    }

    private void IsIce(CharacterClass playerController)
    {
        playerController.moveSpeed += 7;
    }
}