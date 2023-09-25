using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    private CharacterManager _character;

    [Header("Current FX")]
    [Space(15)]
    [SerializeField] private GameObject _instatiateFXModel;

    [Header("Damage FX")]
    [Space(15)]
    [SerializeField] private GameObject _bloodSplatterFX;

    [Header("Weapon FX")]
    [Space(15)]
    [SerializeField] private WeaponFX _rightWeaponFX;
    [SerializeField] private WeaponFX _leftWeaponFX;

    #region GET & SET
    public WeaponFX RightWeaponFX { get { return _rightWeaponFX; } set { _rightWeaponFX = value; }}
    public WeaponFX LeftWeaponFX { get { return _leftWeaponFX; } set { _leftWeaponFX = value; }}
    public GameObject BloodSplatterFX { get { return _bloodSplatterFX; } set { _bloodSplatterFX = value; }}
    public GameObject CurrentRangeFX { get { return _instatiateFXModel; } set { _instatiateFXModel = value; }}
    #endregion
    
    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
    }
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

    public virtual void HandleAllBuildUpEffects()
    {
        if(_character.IsDead)
        {
            return;
        }
    }
    public virtual void InterruptEffect()
    {
        //Pode ser usado para destruir os modelos (flechas, etc..)
        if(_instatiateFXModel != null)
        {
            Destroy(_instatiateFXModel);
        }

        if(_character.IsHoldingArrow)
        {
            _character.Animator.SetBool("IsHoldingArrow", false);
            Animator rangedWeaponAnimator = _character.CharacterWeaponSlot.RightHandSlot.currentWeaponModel.GetComponentInChildren<Animator>();

            if(rangedWeaponAnimator != null)
            {
                rangedWeaponAnimator.SetBool("IsDrawn", false);
                rangedWeaponAnimator.Play("Bow Fire");
            }
        }

        if(_character.IsAiming)
        {
            _character.Animator.SetBool("IsAiming", false);
        }
    }
}
