using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float _timeUntillDestroy = 2;
    private void Awake() 
    {
        Destroy(gameObject, _timeUntillDestroy);
    }
}
