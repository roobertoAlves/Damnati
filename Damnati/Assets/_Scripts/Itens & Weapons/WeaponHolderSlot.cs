using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public GameObject currentWeaponModel {get; private set;}
    private WeaponItem _currentWeapon;

    [Header("Hands")]
    [Space(15)]
    public bool isRightHandSlot;
    public bool isLeftHandSlot;
    public bool isBackSlot;
    [SerializeField] private Transform _parentOverride;


    #region GET & SET

    public WeaponItem CurrentWeapon { get { return _currentWeapon; } set { _currentWeapon = value; }}

    #endregion

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadWeaponAndDestroy();

        if(weaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;

        if(model != null)
        {
            if(_parentOverride != null)
            {
                model.transform.parent = _parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;

    }

    public void UnloadWeapon()
    {
        if(currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    public void UnloadWeaponAndDestroy()
    {
        if(currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }
}
