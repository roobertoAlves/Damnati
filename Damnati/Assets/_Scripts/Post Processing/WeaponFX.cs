using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("Weapon FX")]
    [SerializeField] private ParticleSystem _normalWeaponTrail;


    #region GET & SET
    public ParticleSystem NormalWeaponTrail { get { return _normalWeaponTrail; }}
    #endregion

    public void PlayWeaponFX()
    {
        _normalWeaponTrail.Stop();

        if(_normalWeaponTrail.isStopped)
        {
            _normalWeaponTrail.Play();
        }
    }
}
