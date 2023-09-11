using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{

    [Header("Damage FX")]
    [Space(15)]
    [SerializeField] GameObject _bloodSplatterFX;

    [Header("Weapon FX")]
    [Space(15)]
    [SerializeField] private WeaponFX _rightWeaponFX;
    [SerializeField] private WeaponFX _leftWeaponFX;

    #region GET & SET
    public WeaponFX RightWeaponFX { get { return _rightWeaponFX; } set { _rightWeaponFX = value; }}
    public WeaponFX LeftWeaponFX { get { return _leftWeaponFX; } set { _leftWeaponFX = value; }}
    public GameObject BloodSplatterFX { get { return _bloodSplatterFX; } set { _bloodSplatterFX = value; }}
    #endregion
    public virtual void PlayWeaponFX(bool isLeft)
    {
        if(isLeft == false)
        {
            if(_rightWeaponFX != null)
            {
                _rightWeaponFX.PlayWeaponFX();
                Debug.Log(_rightWeaponFX.NormalWeaponTrail.name);
                Debug.Log("We Playing");
            }
        }
        else
        {
            if(_leftWeaponFX != null)
            {
                _leftWeaponFX.PlayWeaponFX();
            }
        }
    }

    public virtual void PlayerBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(_bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
}
