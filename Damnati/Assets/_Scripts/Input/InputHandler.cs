using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{
    private GameControls _gameControls;
    private CameraHandler _cameraHandler;
    private PlayerInventory _playerInventory;
    private PlayerManager _playerManager;
    private PlayerAttacker _playerAttacker;
    private PlayerLocomotion _playerLocomotion;
    private WeaponSlotManager _weaponSlotManager;

    private float _horizontalMovement;
    private float _verticalMovement;
    private float _moveAmount;
   
   #region Input Flags

    private bool _runInput;
    private bool _sbInput;
    private bool _rbAttackInput;
    private bool _lbAttackInput;
    private bool _ltInput;
    private bool _criticalAttackInput;
    private bool _thEquipInput;
    private bool _comboFlag;
    private bool _interactInput;
    private bool _pauseInput;
    private bool _lockOnInput;
    private bool _lockOnFlag;
    private bool _rStickInput;
    private bool _lStickInput;
    private bool _caInput;
   #endregion

   #region Input Variables

    private Vector2 _cameraMoveInput;
    private Vector2 _walkMoveInput;

   #endregion



   #region GET & SET

    public float HorizontalMovement { get { return _horizontalMovement; } set { _horizontalMovement = value; }}
    public float VerticalMovement { get { return _verticalMovement; } set { _verticalMovement = value; }}
    public float HorizontalCameraMovement { get { return _cameraMoveInput.x; } set { _cameraMoveInput.x = value; }}
    public float VerticalCameraMovement { get { return _cameraMoveInput.y; } set { _cameraMoveInput.y = value; }}
    public float MoveAmount { get { return _moveAmount; } set { _moveAmount = value; }}


    public bool SBFlag { get { return _sbInput; } set { _sbInput = value; }}
    public bool RunFlag { get { return _runInput; } set { _runInput = value; }}
    public bool RBAttackFlag { get { return _rbAttackInput; } set { _rbAttackInput = value; }}
    public bool LBAttackFlag { get { return _lbAttackInput; } set { _lbAttackInput = value; }}
    public bool THEquipFlag { get { return _thEquipInput; } set { _thEquipInput = value; }}
    public bool ComboFlag { get { return _comboFlag; } set { _comboFlag = value; }}
    public bool InteractInput { get { return _interactInput; } set { _interactInput = value; }}
    public bool PauseInput { get { return _pauseInput; } set { _pauseInput = value; }}
    public bool CriticalAttackFlag { get { return _criticalAttackInput; } set { _criticalAttackInput = value; }}
    public bool LockOnFlag { get { return _lockOnFlag; } set { _lockOnFlag = value; }}
    public bool RightStickInput { get { return _rStickInput; } set { _rStickInput = value; }}
    public bool LeftStickInput { get { return _lStickInput; } set { _lStickInput = value; }}
    public bool CriticalAttackInput { get { return _caInput; } set { _caInput = value; }}
    #endregion

    private void Awake() 
    {
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _playerInventory = FindObjectOfType<PlayerInventory>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _playerAttacker = FindObjectOfType<PlayerAttacker>();
        _playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        _weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
    }
    
    #region Input Management

    public GameControls GameControls
    {
        get { return _gameControls; }
    }

    private void OnEnable() 
    {
        if(_gameControls == null)
        {
            _gameControls = new GameControls();
            _gameControls.PlayerMovement.View.performed += CameraView;
            _gameControls.PlayerMovement.View.canceled += CameraView;

            _gameControls.PlayerMovement.LockOnTargetLeft.performed += ctx => _lStickInput = true;
            _gameControls.PlayerMovement.LockOnTargetRight.performed += ctx => _rStickInput = true;

            _gameControls.PlayerMovement.Walk.performed += OnWalk;
            _gameControls.PlayerMovement.Walk.canceled += OnWalk;

            _gameControls.PlayerActions.Run.performed += OnRun;
            _gameControls.PlayerActions.Run.canceled += OnRun;

            _gameControls.PlayerActions.LB.performed += OnLightAttack;
            _gameControls.PlayerActions.LB.canceled += OnLightAttack;

            _gameControls.PlayerActions.RB.performed += OnHeavyAttack;
            _gameControls.PlayerActions.RB.canceled += OnHeavyAttack;

            _gameControls.PlayerActions.LT.performed += OnWeaponArt;
            _gameControls.PlayerActions.LT.canceled += OnWeaponArt;

            _gameControls.PlayerActions.CriticalAttack.performed += OnCriticalAttack;
            _gameControls.PlayerActions.CriticalAttack.canceled += OnCriticalAttack;

            _gameControls.PlayerActions.TH.performed += OnTwoHandEquiped;
            _gameControls.PlayerActions.TH.canceled += OnTwoHandEquiped;

            _gameControls.PlayerActions.StepBack.performed += OnDodge;
            _gameControls.PlayerActions.StepBack.canceled += OnDodge;

            _gameControls.PlayerActions.Interact.performed += OnInteract;
            _gameControls.PlayerActions.Interact.canceled += OnInteract;

            _gameControls.PlayerActions.Pause.performed += OnPause;
            _gameControls.PlayerActions.Pause.canceled += OnPause;

            _gameControls.PlayerActions.CameraLockOn.performed += OnCameraLockOn;

        }
        _gameControls.Enable();    
    }
    private void OnDisable() 
    {
        _gameControls.Disable();   
    }

    #endregion  
    
    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleLockOnInput();
        HandleAttackInput(delta);
        HandleTwoHandWeapon();
        CriticalAttack();
        //Debug.Log("Tick Input: _lockOnInput = " + _lockOnInput + ", _lockOnFlag = " + _lockOnFlag);
    }

    private void MoveInput(float delta)
    {
        _horizontalMovement = _walkMoveInput.x;
        _verticalMovement = _walkMoveInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalMovement) + Mathf.Abs(_verticalMovement));

    }
    private void HandleLockOnInput()
    {
        //Debug.Log("Handling Lock On Input: _lockOnInput = " + _lockOnInput + ", _lockOnFlag = " + _lockOnFlag);

        if(_lockOnInput && _lockOnFlag == false)
        {
            //Debug.Log("Activating Lock On");
            _lockOnInput = false;
            _cameraHandler.HandleLockOn();
            
            if(_cameraHandler.NearestLockOnTarget != null)
            {
                _cameraHandler.CurrentLockOnTarget = _cameraHandler.NearestLockOnTarget;
                _lockOnFlag = true;
                //Debug.Log("Lock On Activated");
            }
        }
        else if(_lockOnInput && _lockOnFlag)
        {
            //Debug.Log("Deactivating Lock On");
            _lockOnInput = false;
            _lockOnFlag = false;
            _cameraHandler.ClearLockOnTargets();
            //Debug.Log("Lock On Deactivated");
        }
        
        if(_lockOnFlag && _lStickInput)
        {
            _lStickInput = false;
            _cameraHandler.HandleLockOn();

            if(_cameraHandler.LeftLockOnTarget != null)
            {
                _cameraHandler.CurrentLockOnTarget = _cameraHandler.LeftLockOnTarget;
            }
        }
        if(_lockOnFlag && _rStickInput)
        {
            _rStickInput = false;
            _cameraHandler.HandleLockOn();
            
            if(_cameraHandler.RightLockOnTarget != null)
            {
                _cameraHandler.CurrentLockOnTarget = _cameraHandler.RightLockOnTarget;
            }
        }

        _cameraHandler.SetCameraHeight();
    }
    private void HandleAttackInput(float delta)
    {
        if(_lbAttackInput)
        {
            _playerAttacker.HandleLBAction();
        }
        if(_rbAttackInput)
        {
            _playerAttacker.HandleRBAction();
        }
        
        if(_ltInput)
        {
            if(_thEquipInput)
            {

            }
            else
            {
                _playerAttacker.HandleLTAction();
            }
        }
    }
    public void HandleTwoHandWeapon()
    {
        if(_thEquipInput)
        {
           _thEquipInput = false;
            _playerManager.TwoHandFlag = !_playerManager.TwoHandFlag;

            if(_playerManager.TwoHandFlag)
            {
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.rightHandWeapon, false);
            }
            else
            {
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.rightHandWeapon, false);
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.leftHandWeapon, true);
            }
        }
    }
    private void CriticalAttack()
    {
        if(_criticalAttackInput)
        {
           _criticalAttackInput = false;
            _playerAttacker.AttemptRiposte();
        }
    }  

    #region Input Methods
    public void CameraView(InputAction.CallbackContext ctx)
    {
        _cameraMoveInput = ctx.ReadValue<Vector2>();
    }
    public void OnCameraLockOn(InputAction.CallbackContext ctx)
    {
        _lockOnInput = ctx.ReadValueAsButton();
    }
    private void OnWalk(InputAction.CallbackContext ctx)
    {
        _walkMoveInput = ctx.ReadValue<Vector2>();
    }
    private void OnRun(InputAction.CallbackContext ctx)
    {
        _runInput = ctx.ReadValueAsButton();
    }

    private void OnDodge(InputAction.CallbackContext ctx)
    {
       _sbInput = ctx.ReadValueAsButton();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        _interactInput = ctx.ReadValueAsButton();
    }

    private void OnLightAttack(InputAction.CallbackContext ctx)
    {
        _lbAttackInput = ctx.ReadValueAsButton();
    }

    private void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        _rbAttackInput = ctx.ReadValueAsButton();
    }
    private void OnWeaponArt(InputAction.CallbackContext ctx)
    {
        _ltInput = ctx.ReadValueAsButton();
    }
    private void OnCriticalAttack(InputAction.CallbackContext ctx)
    {
        _criticalAttackInput = ctx.ReadValueAsButton();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        _pauseInput = ctx.ReadValueAsButton();
    }
    private void OnTwoHandEquiped(InputAction.CallbackContext ctx)
    {
        _thEquipInput = ctx.ReadValueAsButton();
    }

    #endregion
  
}
