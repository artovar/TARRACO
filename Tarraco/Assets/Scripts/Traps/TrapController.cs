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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            CharacterClass playerController = other.GetComponentInParent<CharacterClass>();

            /*feet--;
            if (feet == 0)
            {
                playerController.moveSpeed = originalSpeed; //Reestablecemos la velocidad al salir
            }
            else if (feet == 1)
            {*/
            playerController.moveSpeed *= 2f;
            //}
        }
    }

    private void IsMud(CharacterClass playerController)
    {
        playerController.moveSpeed /= 2;
    }

    private void IsIce(CharacterClass playerController)
    {
        playerController.moveSpeed += 7;
    }

    private IEnumerator IsSpikes(CharacterClass controller, float time)
    {
        while (true)
        {
            if (controller.gameObject == null) StopAllCoroutines();
            controller.Damage(1, Characters.Enemy);
            Debug.Log("Me estoy clavando los pinchos :( \n me queda esta vida: " + controller.life);
            yield return new WaitForSeconds(time);
        }
    }
}