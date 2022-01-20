using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOTHStand : MonoBehaviour
{
    private int pCount = 0;
    List<Collider> players = new List<Collider>();
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
        for(int f = 0; f < 4; f++)
        {
            if(players.Count > 0 && players[0] == null)
            {
                players.RemoveAt(0);
            }
        }
        if (players.Count == 1)
        {
            PlayerController player = players[0].GetComponentInParent<PlayerController>();
            if(player != null && player.character != Characters.Enemy && player.character != Characters.None)
            {
                ovationS.AccumulatePoints(Time.deltaTime * 4, player.character);
            }
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
            players.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerChest"))
        {
            players.Remove(other);
        }
    }
}
