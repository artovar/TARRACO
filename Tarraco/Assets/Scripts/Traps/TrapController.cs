using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public enum trapType
    {
        MUD,
        ICE,
        SPIKES
    }
    public trapType type;

    private float originalSpeed = 0;

    private IEnumerator coroutine;

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
                case trapType.SPIKES:
                    coroutine = IsSpikes(playerController, 4);
                    StartCoroutine(coroutine);
                    break;
            }
            //print("My new speed is " + playerController.moveSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            CharacterClass playerController = other.GetComponentInParent<CharacterClass>();

            if (type == trapType.MUD || type == trapType.ICE) {
                print("I'm exiting bro");
                playerController.moveSpeed *= 2f;
                print("My new speed is " + playerController.moveSpeed);
            } else if (type == trapType.SPIKES) {
                Debug.Log("Salgo de los pinchos");
                StopAllCoroutines();
            }
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

    private IEnumerator IsSpikes(CharacterClass controller, float time) {
        while (true) {
            if (controller.gameObject == null) StopAllCoroutines();
            controller.damage(1);
            Debug.Log("Me estoy clavando los pinchos :( \n me queda esta vida: "+controller.life);
            yield return new WaitForSeconds(time);
        }
    }
}