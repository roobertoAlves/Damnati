using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingCollider : MonoBehaviour
{
    private BoxCollider _blockingCollider;

    private float _blockingPhysicalDamageAbsorption;
    private float _blockingFireDamageAbsorption;

    #region  GET & SET

    public BoxCollider BlockCollider { get { return _blockingCollider; } set { _blockingCollider = value; }}
    public float BlockingPhysicalDamageAbsorption { get { return _blockingPhysicalDamageAbsorption; } set { _blockingPhysicalDamageAbsorption = value; }}
    public float BlockingFireDamageAbsorption { get { return _blockingFireDamageAbsorption; } set {_blockingFireDamageAbsorption = value; }}
    #endregion

    private void Awake() 
    {
        _blockingCollider = GetComponent<BoxCollider>();    
    }

    public void SetColliderDamageAbsorption(WeaponItem weapon)
    {
        if(weapon != null)
        {
            _blockingPhysicalDamageAbsorption = weapon.physicalDamageAbsorption;
        }
    }

    public void EnableBlockingCollider()
    {
        _blockingCollider.enabled = true;
    }
    public void DisableBlockingCollider()
    {
        _blockingCollider.enabled = false;
    }
}
