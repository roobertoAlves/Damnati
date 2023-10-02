using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{
    private GameControls _gameControls;
    private PlayerManager _player;

    private float _horizontalMovement;
    private float _verticalMovement;
    private float _moveAmount;
   
    #region Input Flags
    private bool _rbInput;
    private bool _rbHoldInput;
    private bool _lbInput;
    private bool _lbHoldInput;
    private bool _zInput;
    private bool _rtInput;
    private bool _rInput;
    
    private bool _runInput;
    private bool _dodgeInput;

    private bool _gHoldInput;
    private bool _comboFlag;
    private bool _blockInput;

    private bool _interactInput;
    private bool _pauseInput;

    private bool _thEquipInput;
    private bool _lockOnInput;

    private bool _rStickInput;
    private bool _lStickInput;


    #endregion

    #region Input Qued Flags
    private bool _inputHasBeenQued;
    private float _currentQuedInputTimer;
    private float _defaultQuedInputTime = .36f;

    private bool _quedRBInput;
    private bool _quedHoldRBInput;
    private bool _quedLBInput;
    private bool _quedHoldLBInput;
    #endregion

    #region Player Flags

    private bool _twoHandFlag;
    private bool _isDodge;
    private bool _isHitEnemy;
    private bool _isInRage;
    private bool _fireFlag;
    private bool _lockOnFlag;
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


    public bool DogdeFlag { get { return _dodgeInput; } set { _dodgeInput = value; }}
    public bool RunFlag { get { return _runInput; } set { _runInput = value; }}
    public bool RBInput { get { return _rbInput; } set { _rbInput = value; }}
    public bool LBInput { get { return _lbInput; } set { _lbInput = value; }}
    public bool THEquipFlag { get { return _thEquipInput; } set { _thEquipInput = value; }}
    public bool ComboFlag { get { return _comboFlag; } set { _comboFlag = value; }}
    public bool InteractInput { get { return _interactInput; } set { _interactInput = value; }}
    public bool PauseInput { get { return _pauseInput; } set { _pauseInput = value; }}
    public bool CriticalAttackFlag { get { return _gHoldInput; } set { _gHoldInput = value; }}
    public bool LockOnFlag { get { return _lockOnFlag; } set { _lockOnFlag = value; }}
   
    public bool RightStickInput { get { return _rStickInput; } set { _rStickInput = value; }}
    public bool LeftStickInput { get { return _lStickInput; } set { _lStickInput = value; }}
    public bool RBHoldInput { get { return _rbHoldInput; } set { _rbHoldInput = value; }}
    public bool LBHoldInput { get { return _lbHoldInput; } set { _lbHoldInput = value; }}
    public bool BlockInput { get { return _blockInput; } set { _blockInput = value; }}
    public bool TwoHandFlag { get { return _twoHandFlag; } set { _twoHandFlag = value; }}
    public bool IsDodge { get { return _isDodge; } set { _isDodge = value; }}
    public bool IsHitEnemy { get { return _isHitEnemy; } set { _isHitEnemy = value; }}
    public bool IsInRage { get { return _isInRage; } set { _isInRage = value; }}
    public bool FireFlag { get { return _fireFlag; } set { _fireFlag = value; }}
    
    public UIManager UIManager { get { return _player.UIManager; }}
    #endregion

    private void Awake() 
    {
        _player = FindObjectOfType<PlayerManager>();
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
            
            _gameControls.PlayerActions.Dodge.performed += OnDodge;
            _gameControls.PlayerActions.Dodge.canceled += OnDodge;

            _gameControls.PlayerActions.HoldRB.performed += OnAiming;
            _gameControls.PlayerActions.HoldRB.canceled += OnAiming;
            _gameControls.PlayerActions.HoldRB.canceled += ctx => _fireFlag = true;

            _gameControls.PlayerActions.HoldLB.performed += OnChargeAttack;
            _gameControls.PlayerActions.HoldLB.canceled += OnChargeAttack;

            _gameControls.PlayerMovement.Walk.performed += OnWalk;
            _gameControls.PlayerMovement.Walk.canceled += OnWalk;

            _gameControls.PlayerActions.Run.performed += OnRun;
            _gameControls.PlayerActions.Run.canceled += OnRun;

            _gameControls.PlayerActions.LB.performed += OnLightAttack;
            _gameControls.PlayerActions.LB.canceled += OnLightAttack;

            _gameControls.PlayerActions.RB.performed += OnHeavyAttack;
            _gameControls.PlayerActions.RB.canceled += OnHeavyAttack;

            _gameControls.PlayerActions.Reload.performed += OnDrawArrow;
            _gameControls.PlayerActions.Reload.canceled += OnDrawArrow;

            _gameControls.PlayerActions.WeaponArt.performed += OnWeaponArt;
            _gameControls.PlayerActions.WeaponArt.canceled += OnWeaponArt;

            _gameControls.PlayerActions.Block.performed += OnDefense;
            _gameControls.PlayerActions.Block.canceled += OnDefense;

            _gameControls.PlayerActions.Critical.performed += OnCriticalAttack;
            _gameControls.PlayerActions.Critical.canceled += OnCriticalAttack;

            _gameControls.PlayerActions.TH.performed += OnTwoHandEquiped;
            _gameControls.PlayerActions.TH.canceled += OnTwoHandEquiped;

            _gameControls.PlayerActions.Interact.performed += OnInteract;
            _gameControls.PlayerActions.Interact.canceled += OnInteract;

            _gameControls.PlayerActions.Pause.performed += OnPause;
            _gameControls.PlayerActions.Pause.canceled += OnPause;

            _gameControls.PlayerActions.CameraLockOn.performed += OnCameraLockOn;


            #region Qued Input Actions

            _gameControls.PlayerActions.QuedRB.performed += ctx => QuedInput(ref _quedRBInput);
            _gameControls.PlayerActions.QuedHoldRB.performed += ctx => QuedInput(ref _quedHoldRBInput);
            _gameControls.PlayerActions.QuedLB.performed += ctx => QuedInput(ref _quedLBInput);
            _gameControls.PlayerActions.QuedHoldLB.performed += ctx => QuedInput(ref _quedHoldLBInput);
            _gameControls.PlayerActions.QuedRB.performed += ctx => QuedInput(ref _quedRBInput);
            #endregion
        }
        _gameControls.Enable();    
    }
    private void OnDisable() 
    {
        _gameControls.Disable();   
    }

    #endregion  
    
    public void TickInput()
    {
        if(_player.IsDead)
        {
            return;
        }
        
        HandleQuedInput();
        HandleMoveInput();

        HandleTapRBInput();
        HandleTapLBInput();
        
        HandleReleaseRBInput();

        HandleTapZInput();
        HandleTapRInput();
        HandleTapGInput();
        HandleHoldFInput();
        HandleHoldRBInput();
        HandleHoldLBInput();

        HandleLockOnInput();
        HandleTwoHandInput();


        //Debug.Log("Tick Input: _lockOnInput = " + _lockOnInput + ", _lockOnFlag = " + _lockOnFlag);
    }
    private void HandleMoveInput()
    {
        if(_player.IsHoldingArrow)
        {
            _horizontalMovement = _walkMoveInput.x;
            _verticalMovement = _walkMoveInput.y;
            _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalMovement) + Mathf.Abs(_verticalMovement) / 2);

            if(_moveAmount > 0.5f)
            {
                _moveAmount = 0.5f;
            }
        }
        else
        {
            _horizontalMovement = _walkMoveInput.x;
            _verticalMovement = _walkMoveInput.y;
            _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalMovement) + Mathf.Abs(_verticalMovement));
        }
    }
    private void HandleTapLBInput()
    {
        if(_lbInput)
        {
            _lbInput = false;

            if(_player.PlayerInventory.rightHandWeapon.oh_tap_LB_Action != null)
            {
                _player.UpdateWhichHandCharacterIsUsing(true);
                _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;
                _player.PlayerInventory.rightHandWeapon.oh_tap_LB_Action.PerformAction(_player);
            }
        }
    }
    private void HandleHoldLBInput()
    {
        _player.Animator.SetBool("IsChargingAttack", _lbHoldInput);

        if(_lbHoldInput)
        {
            _player.UpdateWhichHandCharacterIsUsing(true);
            _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;

            if(_player.IsTwoHandingWeapon)
            {
                if(_player.PlayerInventory.rightHandWeapon.th_hold_LB_Action != null)
                {
                    _player.PlayerInventory.rightHandWeapon.th_hold_LB_Action.PerformAction(_player);
                }
                {

                }
            }
            else
            {
                if(_player.PlayerInventory.rightHandWeapon.oh_hold_LB_Action != null)
                {
                    _player.PlayerInventory.rightHandWeapon.oh_hold_LB_Action.PerformAction(_player);
                }
            }
        }
    }
    private void HandleTapRBInput()
    {
        if(_rbInput)
        {
            _rbInput = false;
            if(_player.PlayerInventory.rightHandWeapon.oh_tap_RB_Action != null)
            {
                _player.UpdateWhichHandCharacterIsUsing(true);
                _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;
                _player.PlayerInventory.rightHandWeapon.oh_tap_RB_Action.PerformAction(_player);
            }
        }
    }
    private void HandleHoldRBInput()
    {
        if(!_player.IsGrounded || _player.IsSprinting)
        {
            _rbHoldInput = false;
            return;
        }

        if(_rbHoldInput)
        {
            if(_player.IsTwoHandingWeapon)
            {
                if(_player.PlayerInventory.rightHandWeapon.th_hold_RB_Action != null)
                {
                    _player.UpdateWhichHandCharacterIsUsing(true);
                    _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;
                    _player.PlayerInventory.rightHandWeapon.th_hold_RB_Action.PerformAction(_player);
                }
            }
            else
            {
                if(_player.PlayerInventory.leftHandWeapon.oh_hold_RB_Action != null)
                {
                    _player.UpdateWhichHandCharacterIsUsing(false);
                    _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.leftHandWeapon;
                    _player.PlayerInventory.leftHandWeapon.oh_hold_RB_Action.PerformAction(_player);
                }
            }
        }
        else if(_rbHoldInput == false)
        {
            if(_player.IsAiming)
            {
                _player.IsAiming = false;
                _player.UIManager.CrossHair.SetActive(false);
                _player.PlayerCamera.ResetAimCameraRotations();
            }
        }
    }
    private void HandleReleaseRBInput()
    {
        if(_player.IsSprinting || !_player.IsGrounded)
        {
            _fireFlag = false;
            return;
        }

        if(_fireFlag)
        {
            if(_player.IsTwoHandingWeapon)
            {
                if(_player.PlayerInventory.rightHandWeapon.th_release_RB_Action != null)
                {
                    _fireFlag = false;
                    _player.UpdateWhichHandCharacterIsUsing(true);
                    _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;
                    _player.PlayerInventory.rightHandWeapon.th_release_RB_Action.PerformAction(_player);
                }
            }
        }
    }

    private void HandleTapZInput()
    {
        if (_zInput)
        {
            _zInput = false;

            if (_player.IsTwoHandingWeapon)
            {
                //It will be the right handed weapon
                if (_player.PlayerInventory.rightHandWeapon.th_tap_Z_Action != null)
                {
                    _player.UpdateWhichHandCharacterIsUsing(true);
                    _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;
                    _player.PlayerInventory.rightHandWeapon.th_tap_Z_Action.PerformAction(_player);
                }
            }
            else
            {
                if (_player.PlayerInventory.leftHandWeapon.oh_tap_Z_Action != null)
                {
                    _player.UpdateWhichHandCharacterIsUsing(false);
                    _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.leftHandWeapon;
                    _player.PlayerInventory.leftHandWeapon.oh_tap_Z_Action.PerformAction(_player);
                }
            }
        }
    }
    private void HandleHoldFInput()
    {
        if(!_player.IsGrounded || _player.IsSprinting)
        {
            _blockInput = false;
            return;
        }

        if(_blockInput)
        {
            if(_player.IsTwoHandingWeapon)
            {
                if(_player.PlayerInventory.rightHandWeapon.th_hold_F_Action != null)
                {
                    _player.UpdateWhichHandCharacterIsUsing(true);
                    _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;
                    _player.PlayerInventory.rightHandWeapon.th_hold_F_Action.PerformAction(_player);
                }
            }
            else
            {
                if(_player.PlayerInventory.leftHandWeapon.oh_hold_F_Action != null)
                {
                    _player.UpdateWhichHandCharacterIsUsing(false);
                    _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.leftHandWeapon;
                    _player.PlayerInventory.leftHandWeapon.oh_hold_F_Action.PerformAction(_player);
                }
            }
        }
        else if(_blockInput == false)
        {
            if(_player.IsAiming)
            {
                _player.IsAiming = false;
                _player.UIManager.CrossHair.SetActive(false);
                _player.PlayerCamera.ResetAimCameraRotations();
            }
            if(_player.IsBlocking)
            {
                _player.IsBlocking = false;
            }
        }
    }
    private void HandleLockOnInput()
    {
        //Debug.Log("Handling Lock On Input: _lockOnInput = " + _lockOnInput + ", _lockOnFlag = " + _lockOnFlag);

        if(_lockOnInput && _lockOnFlag == false)
        {
            //Debug.Log("Activating Lock On");
            _lockOnInput = false;
            _player.PlayerCamera.HandleLockOn();
            
            if(_player.PlayerCamera.NearestLockOnTarget != null)
            {
                _player.PlayerCamera.CurrentLockOnTarget = _player.PlayerCamera.NearestLockOnTarget;
                _lockOnFlag = true;
                //Debug.Log("Lock On Activated");
            }
        }
        else if(_lockOnInput && _lockOnFlag)
        {
            //Debug.Log("Deactivating Lock On");
            _lockOnInput = false;
            _lockOnFlag = false;
            _player.PlayerCamera.ClearLockOnTargets();
            //Debug.Log("Lock On Deactivated");
        }
        
        if(_lockOnFlag && _lStickInput)
        {
            _lStickInput = false;
            _player.PlayerCamera.HandleLockOn();

            if(_player.PlayerCamera.LeftLockOnTarget != null)
            {
                _player.PlayerCamera.CurrentLockOnTarget = _player.PlayerCamera.LeftLockOnTarget;
            }
        }
        if(_lockOnFlag && _rStickInput)
        {
            _rStickInput = false;
            _player.PlayerCamera.HandleLockOn();
            
            if(_player.PlayerCamera.RightLockOnTarget != null)
            {
                _player.PlayerCamera.CurrentLockOnTarget = _player.PlayerCamera.RightLockOnTarget;
            }
        }

        _player.PlayerCamera.SetCameraHeight();
    }
    private void HandleTwoHandInput()
    {
        if(_thEquipInput && _player.PlayerInventory.rightHandWeapon.weaponType != WeaponType.Spear)
        {
            Debug.Log("Não é lança");
            return;
        }

        if(_thEquipInput)
        {
            Debug.Log("É uma lança");
            _thEquipInput = false;

            _twoHandFlag = !_twoHandFlag;

            if(_twoHandFlag)
            {
                _player.IsTwoHandingWeapon = true;
                _player.PlayerWeaponSlot.LoadWeaponOnSlot(_player.PlayerInventory.rightHandWeapon, false);
                _player.PlayerWeaponSlot.LoadTwoHandIKTargets(true);
            }
            else
            {
                _player.IsTwoHandingWeapon = false;
                _player.PlayerWeaponSlot.LoadWeaponOnSlot(_player.PlayerInventory.rightHandWeapon, false);
                _player.PlayerWeaponSlot.LoadWeaponOnSlot(_player.PlayerInventory.leftHandWeapon, true);
                _player.PlayerWeaponSlot.LoadTwoHandIKTargets(false);
            }
        }
    }
    private void HandleTapGInput()
    {
        if(_gHoldInput)
        {
            if(_player.PlayerInventory.rightHandWeapon.oh_hold_G_Action != null)
            {
                _player.UpdateWhichHandCharacterIsUsing(true);
                _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;
                _player.PlayerInventory.rightHandWeapon.oh_hold_G_Action.PerformAction(_player);
            }
            else
            {
                _player.UpdateWhichHandCharacterIsUsing(false);
                _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.leftHandWeapon;
                _player.PlayerInventory.leftHandWeapon.oh_hold_G_Action.PerformAction(_player);
            }
        }
    }  
    private void HandleTapRInput()
    {
        if(_rInput)
        {
            if(_player.PlayerInventory.rightHandWeapon.th_tap_R_Action != null)
            {
                _player.UpdateWhichHandCharacterIsUsing(true);
                _player.PlayerInventory.CurrentItemBeingUsed = _player.PlayerInventory.rightHandWeapon;
                _player.PlayerInventory.rightHandWeapon.th_tap_R_Action.PerformAction(_player);
            }
        }
    }
    private void QuedInput(ref bool quedInput)
    {
        _quedRBInput = false;
        _quedHoldRBInput = false;
        _quedLBInput = false;
        _quedHoldLBInput = false;

        //Se o jogador estiver interagindo ele pode colocar um input na fila, de outra forma não é necessário armazenar dentro
        if(_player.IsInteracting)
        {
            quedInput = true;
            _currentQuedInputTimer = _defaultQuedInputTime;
            _inputHasBeenQued = true;
        }   
    }

    private void HandleQuedInput()
    {
        if(_inputHasBeenQued)
        {
            if(_currentQuedInputTimer > 0)
            {
                _currentQuedInputTimer = _currentQuedInputTimer - Time.deltaTime;
                ProcessQuedInput();
            }
            else
            {
                _inputHasBeenQued = false;
                _currentQuedInputTimer = 0;
            }
        }
    }

    private void ProcessQuedInput()
    {
        if(_quedRBInput)
        {
            _rbInput = true;
        }
        else if(_quedHoldRBInput)
        {
            _rbHoldInput = true;
        }
        else if(_quedLBInput)
        {
            _lbInput = true;
        }
        else if(_quedHoldLBInput)
        {
            _lbHoldInput = true;
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
       _dodgeInput = ctx.ReadValueAsButton();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        _interactInput = ctx.ReadValueAsButton();
    }

    private void OnLightAttack(InputAction.CallbackContext ctx)
    {
        _lbInput = ctx.ReadValueAsButton();
    }

    private void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        _rbInput = ctx.ReadValueAsButton();
    }
    private void OnWeaponArt(InputAction.CallbackContext ctx)
    {
        _zInput = ctx.ReadValueAsButton();
    }
    private void OnAiming(InputAction.CallbackContext ctx)
    {
        _rbHoldInput = ctx.ReadValueAsButton();
    }
    private void OnDrawArrow(InputAction.CallbackContext ctx)
    {
        _rInput = ctx.ReadValueAsButton();
    }
    private void OnCriticalAttack(InputAction.CallbackContext ctx)
    {
        _gHoldInput = ctx.ReadValueAsButton();
    }
    private void OnDefense(InputAction.CallbackContext ctx)
    {
        _blockInput = ctx.ReadValueAsButton();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        _pauseInput = ctx.ReadValueAsButton();
    }
    private void OnTwoHandEquiped(InputAction.CallbackContext ctx)
    {
        _thEquipInput = ctx.ReadValueAsButton();
    }
    private void OnChargeAttack(InputAction.CallbackContext ctx)
    {
        _lbHoldInput = ctx.ReadValueAsButton();
    }

    #endregion
  
}
