using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{
    private GameControls _gameControls;
    private CameraHandler _cameraHandler;

    private float _horizontalMovement;
    private float _verticalMovement;
    private float _moveAmount;
   
   #region Input Flags

    private bool _runInput;
    private bool _sbInput;
    private bool _rbAttackInput;
    private bool _lbAttackInput;
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
    public bool LockOnInput { get { return _lockOnInput; } set { _lockOnInput = value; }}
    public bool LockOnFlag { get { return _lockOnFlag; } set { _lockOnFlag = value; }}
    public bool RightStickInput { get { return _rStickInput; } set { _rStickInput = value; }}
    public bool LeftStickInput { get { return _lStickInput; } set { _lStickInput = value; }}
    public bool CriticalAttackInput { get { return _caInput; } set { _caInput = value; }}
    #endregion

    private void Awake() 
    {
        _gameControls = new GameControls();

        _cameraHandler = FindObjectOfType<CameraHandler>();

        _gameControls.PlayerMovement.View.performed += CameraView;
        _gameControls.PlayerMovement.View.canceled += CameraView;

        _gameControls.PlayerActions.CameraLockOn.performed += ctx => _lockOnInput = true;
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

    }
    
    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleLockOnInput();
    }

    private void MoveInput(float delta)
    {
        _horizontalMovement = _walkMoveInput.x;
        _verticalMovement = _walkMoveInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalMovement) + Mathf.Abs(_verticalMovement));

    }
    private void HandleLockOnInput()
    {
        if(_lockOnInput && _lockOnFlag == false)
        {
            _lockOnInput = false;
            _cameraHandler.HandleLockOn();
            
            if(_cameraHandler.NearestLockOnTarget != null)
            {
                _cameraHandler.CurrentLockOnTarget = _cameraHandler.NearestLockOnTarget;
                _lockOnFlag = true; 
            }
        }
        else if(_lockOnInput && _lockOnFlag)
        {
            _lockOnInput = false;
            _lockOnFlag = false;
            _cameraHandler.ClearLockOnTargets();
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


    #region Input Methods
    public void CameraView(InputAction.CallbackContext ctx)
    {
        _cameraMoveInput = ctx.ReadValue<Vector2>();

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

    #region Input Management

    public GameControls GameControls
    {
        get { return _gameControls; }
    }

    private void OnEnable() 
    {
        _gameControls.Enable();    
    }
    private void OnDisable() 
    {
        _gameControls.Disable();   
    }

    #endregion    
}
