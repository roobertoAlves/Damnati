using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{
   private GameControls _gameControls;


    private float _horizontalMovement;
    private float _verticalMovement;
    private float _moveAmount;
   
   #region Input Flags

    private bool _runInput;
    private bool _sbInput;
    private bool _rbAttackInput;
    private bool _lbAttackInput;
    private bool _thEquipInput;
    private bool _comboFlag;
    private bool _interactInput;
    private bool _pauseInput;

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
    
    #endregion

    private void Awake() 
    {
        _gameControls = new GameControls();

        _gameControls.PlayerMovement.View.performed += CameraView;
        _gameControls.PlayerMovement.View.canceled += CameraView;

        _gameControls.PlayerMovement.Walk.performed += OnWalk;
        _gameControls.PlayerMovement.Walk.canceled += OnWalk;

        _gameControls.PlayerActions.Run.performed += OnRun;
        _gameControls.PlayerActions.Run.canceled += OnRun;

        _gameControls.PlayerActions.LB.performed += OnLightAttack;
        _gameControls.PlayerActions.LB.canceled += OnLightAttack;

        _gameControls.PlayerActions.RB.performed += OnHeavyAttack;
        _gameControls.PlayerActions.RB.canceled += OnHeavyAttack;

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
    }

    private void MoveInput(float delta)
    {
        _horizontalMovement = _walkMoveInput.x;
        _verticalMovement = _walkMoveInput.y;
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalMovement) + Mathf.Abs(_verticalMovement));

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
