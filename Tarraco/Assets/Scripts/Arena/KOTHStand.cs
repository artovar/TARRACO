using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHStand : MonoBehaviour
{
    private int pCount = 0;
    List<Characters> players = new List<Characters>();
    ArenaOvationSingleton ovationS;
    [SerializeField]
    private ParticleSystem particles;
    private bool emmiting = true;

    // Start is called before the first frame update
    void Start()
    {
        ovationS = OvationSingleton.Instance.GetComponent<ArenaOvationSingleton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pCount == 1)
        {
            ovationS.AccumulatePoints(Time.deltaTime * 4, players[0]);
            if (!emmiting)
            {
                particles.Play();
                emmiting = true;
            }
        }
        else
        {
            if (emmiting)
            {
                particles.Stop();
                emmiting = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerChest"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if(player != null)
            {
                players.Add(player.character);
                pCount++;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerChest"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                players.Remove(player.character);
                pCount--;
            }
        }
    }
}
