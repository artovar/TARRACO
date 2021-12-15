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

    //Ovaciones jugadores
    private GameObject[] players = new GameObject[4];
    private GameObject fromPlayer;

    // Start is called before the first frame update
    void Start() {
        // Nos guardamos todos los jugadores en la lista
        players = GameObject.FindGameObjectsWithTag("Player");
    }

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
                    coroutine = IsSpikes(playerController, 10);
                    StopAllCoroutines();
                    StartCoroutine(coroutine);
                    break;
            }

            if (other.gameObject.layer == 7/*"Enemies"*/) {
                //Si esta en la capa enemigos, es un enemigo
                GameObject py = other.gameObject.GetComponentInParent<EnemyController>().player;
                if (py == null) return;

                int layerPlayer = py.layer; //El jugador al que persigue
                foreach(GameObject player in players) {
                    if(layerPlayer == player.layer) fromPlayer = player;
                }

                //Buscamos el jugador más cerca de la trampa (que será el que ha metido alli el enemigo)
                if (fromPlayer != null && Distance(this.gameObject, fromPlayer) < 40) {
                    if (type == trapType.MUD) OvationSingleton.Instance.IncreaseMeter(5f, fromPlayer.GetComponent<CharacterClass>().character);
                    if (type == trapType.SPIKES) OvationSingleton.Instance.IncreaseMeter(10f, fromPlayer.GetComponent<CharacterClass>().character);
                    Debug.Log("Me ha metido en este berenjenal: "+fromPlayer.GetComponent<CharacterClass>().character.ToString());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            CharacterClass playerController = other.GetComponentInParent<CharacterClass>();

            if (type == trapType.MUD) playerController.moveSpeed *= 2f;
            
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
            Debug.Log("Aun me he pinchado me queda esta vida: "+controller.life);
            yield return new WaitForSeconds(time);
        }
    }

    private float Distance(GameObject p1, GameObject p2) {
        float x = (p1.transform.position.x - p2.transform.position.x);
        float y = (p1.transform.position.y - p2.transform.position.y);
        float z = (p1.transform.position.z - p2.transform.position.z);
        return (float)Mathf.Sqrt(x*x + y*y + z*z);
    }
}