using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageCollider : MonoBehaviour
{
    [SerializeField] private Transform _criticalDamagerStandPosition;

    #region GET & SET
    public Transform CriticalDamagerStandPosition { get { return _criticalDamagerStandPosition; } set { _criticalDamagerStandPosition = value; }}
    
    #endregion
}
