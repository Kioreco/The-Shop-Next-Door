using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporal : MonoBehaviour
{
    public GameObject NPC;
    public GameObject posicionNPC;
    public void instanciarNPC()
    {
        Instantiate(NPC, posicionNPC.transform.position, posicionNPC.transform.rotation);
    }
}
