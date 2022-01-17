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

    private IEnumerator coroutine;


    // Start is called before the first frame update
    void Start() 
    {
        // Nos guardamos todos los jugadores en la lista
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            CharacterClass playerController = other.GetComponentInParent<CharacterClass>();

            switch (type)
            {
                case trapType.MUD:
                    IsMud(playerController);
                    break;
                case trapType.ICE:
                    IsIce(playerController);
                    break;
                case trapType.SPIKES:
                    IsSpikes(playerController);
                    /*
                    coroutine = IsSpikes(playerController, 10);
                    StopAllCoroutines();
                    StartCoroutine(coroutine);*/
                    break;
            }

            if (other.gameObject.layer == 7/*"Enemies"*/) {
                //Si esta en la capa enemigos, es un enemigo
                EnemyController py = other.gameObject.GetComponentInParent<EnemyController>();
                if (py == null || py.player == null) return;
                Transform t = py.player.GetComponentInParent<PlayerController>().Root.transform;
                //Buscamos el jugador más cerca de la trampa (que será el que ha metido alli el enemigo)
                if ((py.enemyScript.Root.transform.position - t.position).magnitude < 10) {
                    if (type == trapType.MUD) OvationSingleton.Instance.IncreaseMeter(2f, py.player.GetComponentInParent<CharacterClass>().character);
                    if (type == trapType.SPIKES) OvationSingleton.Instance.IncreaseMeter(5f, py.player.GetComponentInParent<CharacterClass>().character);
                    //Debug.Log("Me ha metido en este berenjenal: "+fromPlayer.GetComponent<CharacterClass>().character.ToString());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            CharacterClass playerController = other.GetComponentInParent<CharacterClass>();
            if (type == trapType.MUD) playerController.FootOut(.7f);
            
        }
    }

    private void IsMud(CharacterClass playerController)
    {
        playerController.FootIn(.7f);
    }

    private void IsIce(CharacterClass playerController)
    {
        playerController.moveSpeed += 7;
    }

    private void IsSpikes(CharacterClass controller)
    {
        controller.Damage(1, Characters.Enemy, .5f);
        Rigidbody rootRigidbody;
        switch(controller.character) {
            case Characters.None:
                break;
            case Characters.Enemy:
                rootRigidbody = controller.GetComponent<BasicEnemyController>().Root.GetComponent<Rigidbody>();
                controller.GetComponent<BasicEnemyController>().ActivateRagdoll();
                rootRigidbody.velocity = (rootRigidbody.transform.position - transform.position).normalized * 50;
                break;
            default:
                rootRigidbody = controller.GetComponent<PlayerController>().Root.GetComponent<Rigidbody>();
                controller.GetComponent<PlayerController>().ActivateRagdoll();
                rootRigidbody.velocity = (rootRigidbody.transform.position - transform.position).normalized * 50;
                break;
        }
    }

    /*private IEnumerator IsSpikes(CharacterClass controller, float time)
    {
        while (true)
        {
            if (controller.gameObject == null) StopAllCoroutines();
            controller.Damage(1, Characters.Enemy);
            Debug.Log("Aun me he pinchado me queda esta vida: "+controller.life);
            yield return new WaitForSeconds(time);
        }
    }*/
}