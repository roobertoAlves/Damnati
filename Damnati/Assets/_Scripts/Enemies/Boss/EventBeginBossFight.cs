using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBeginBossFight : MonoBehaviour
{
    private WorldEventManager _worldEventManager;

    private void Awake()
    {
        _worldEventManager = FindObjectOfType<WorldEventManager>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            _worldEventManager.ActivateBossFight();
        }    
    }
}
