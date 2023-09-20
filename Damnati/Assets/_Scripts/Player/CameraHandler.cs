using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraHandler : MonoBehaviour
{
    private InputHandler _inputHandler; 
    private PlayerManager _playerManager;

    [Header("Camera Components")]
    [Space(15)]
    private Transform _targetTransform;
    [SerializeField] private Transform _targetTransformWhileAiming;
    [SerializeField] private Transform _cameraTransform;
    private Camera _cameraObject;
    [SerializeField] private Transform _cameraPivotTransform;

    private Vector3 _cameraTransformPosition;
    [SerializeField] private LayerMask _ignoreLayers;
    [SerializeField] private LayerMask _enviromentLayer;
    private Vector3 _cameraFollowVelocity = Vector3.zero;


    [Header("Camera Settings")]
    [Space(15)]

    [SerializeField] private float _horizontalLookSpeed = 25f;
    [SerializeField] private float _verticalLookSpeed = 25f;
    [SerializeField] private float _followSpeed = 1f;
    [SerializeField] private float _horizontalAimingLookSpeed = 5f;
    [SerializeField] private float _verticalAimingLookSpeed = 5f;

    private float _targetPosition;
    private float _defaultPosition;


    [Header("Camera Angle")]
    [Space(15)]
    [SerializeField] private float _minimumLookUpAngle = -35;
    [SerializeField] private float _maximumLookDownAngle = 35;
    private float _horizontalAngle;
    private float _verticalAngle;

    [Header("Camera Collision")]
    [Space(15)]
    [SerializeField] private float _cameraSphereRadius = 0.2f;
    [SerializeField] private float _cameraCollisionOffSet = 0.2f;
    [SerializeField] private float _minimumCollisionOffset = 0.2f;

    [Header("Lock On Settings")]
    [Space(15)]
    private CharacterManager _currentLockOnTarget;
    [SerializeField] private List<CharacterManager> _avaliableTargets = new List<CharacterManager>();
    private CharacterManager _nearestLockOnTarget;
    [SerializeField] private CharacterManager _leftLockOnTarget;
    [SerializeField] private CharacterManager _rightLockOnTarget;
    [Space(15)]
    [SerializeField] private float _maximumLockOnDistance = 30;
    [Space(15)]
    [SerializeField] private float _lockedPivotPosition = 2.25f;
    [SerializeField] private float _unlockedPivotPosition = 1.65f;

    #region GET & SET
    public Transform CameraPivotTransform { get { return _cameraPivotTransform; } set { _cameraPivotTransform = value; }}
    public Transform CameraTransform { get { return _cameraTransform; } set { _cameraTransform = value; }}
    public CharacterManager CurrentLockOnTarget { get { return _currentLockOnTarget; } set { _currentLockOnTarget = value; }}
    public CharacterManager NearestLockOnTarget { get { return _nearestLockOnTarget; } set { _nearestLockOnTarget = value; }}
    public CharacterManager LeftLockOnTarget { get { return _leftLockOnTarget; } set { _leftLockOnTarget = value; }}
    public CharacterManager RightLockOnTarget { get { return _rightLockOnTarget; } set { _rightLockOnTarget = value; }}
    public Camera CameraObject { get { return _cameraObject; }}
    #endregion

    private void Awake()
    {
        _defaultPosition = _cameraTransform.localPosition.z;
        _targetTransform = FindObjectOfType<PlayerManager>().transform;
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _cameraObject = GetComponentInChildren<Camera>();
    }

    private void Start() 
    {
        _enviromentLayer = LayerMask.NameToLayer("Environment");    
    }

    public void FollowTarget()
    {
        if(_playerManager.IsAiming)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, _targetTransformWhileAiming.position, ref _cameraFollowVelocity, Time.deltaTime * _followSpeed);
            transform.position = targetPosition;
        }
        else
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, _targetTransform.position, ref _cameraFollowVelocity, Time.deltaTime * _followSpeed);
            transform.position = targetPosition;
        }

        HandleCameraCollisions();
    }

    public void HandleCameraRotation()
    {
        if(_inputHandler.LockOnFlag && _currentLockOnTarget != null)
        {
            HandleLockedCameraRotation();
        }
        else if(_playerManager.IsAiming)
        {
            HandleAimedCameraRotation();
        }
        else
        {
            HandleStandardCameraRotation();
        }
    }

    public void HandleStandardCameraRotation()
    {
        _horizontalAngle += _inputHandler.HorizontalCameraMovement * _horizontalLookSpeed * Time.deltaTime;
        _verticalAngle -= _inputHandler.VerticalCameraMovement * _verticalLookSpeed * Time.deltaTime;
        _verticalAngle = Mathf.Clamp(_verticalAngle, _minimumLookUpAngle, _maximumLookDownAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = _horizontalAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = _verticalAngle;

        targetRotation = Quaternion.Euler(rotation);
        _cameraPivotTransform.localRotation = targetRotation;
    }
    private void HandleLockedCameraRotation()
    {
        Vector3 dir = _currentLockOnTarget.transform.position - transform.position;
        dir.Normalize();
        dir.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = targetRotation;

        dir = _currentLockOnTarget.transform.position - _cameraPivotTransform.position;
        dir.Normalize();

        targetRotation = Quaternion.LookRotation(dir);
        Vector3 eulerAngle = targetRotation.eulerAngles;
        eulerAngle.y = 0;
        _cameraPivotTransform.localEulerAngles = eulerAngle;
    }
    private void HandleAimedCameraRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _cameraPivotTransform.rotation = Quaternion.Euler(0, 0, 0);

        Quaternion targetRotationX;
        Quaternion targetRotationY;

        Vector3 cameraRotationX = Vector3.zero;
        Vector3 cameraRotationY = Vector3.zero;

        _horizontalAngle += (_inputHandler.HorizontalCameraMovement * _horizontalAimingLookSpeed) * Time.deltaTime;
        _verticalAngle -= (_inputHandler.VerticalCameraMovement * _verticalAimingLookSpeed) * Time.deltaTime;

        cameraRotationY.y = _horizontalAngle;
        targetRotationY = Quaternion.Euler(cameraRotationY);
        targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
        transform.localRotation = targetRotationY;

        cameraRotationX.x = _verticalAngle;
        targetRotationX = Quaternion.Euler(cameraRotationX);
        targetRotationX = Quaternion.Slerp(_cameraTransform.localRotation, targetRotationX, 1);
        _cameraTransform.localRotation = targetRotationX;
    }
    public void ResetAimCameraRotations()
    {
        _cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    private void HandleCameraCollisions()
    {
        // Determine a direção na qual a câmera está olhando
        Vector3 cameraForward = _cameraTransform.forward;

        // Lance um raio na direção da câmera
        if (Physics.SphereCast(_cameraPivotTransform.position, _cameraSphereRadius, cameraForward, out RaycastHit hit, Mathf.Abs(_defaultPosition), _ignoreLayers))
        {
            // Verifique se o objeto atingido não está na camada de ambiente
            if (hit.transform.gameObject.layer == _enviromentLayer)
            {
                float dis = Vector3.Distance(_cameraPivotTransform.position, hit.point);
                _targetPosition = -(dis - _cameraCollisionOffSet);
            }
        }
        else
        {
            _targetPosition = _defaultPosition; // Nenhum objeto atingido, use a posição padrão
        }

        // Limite o valor mínimo de deslocamento da câmera
        if (Mathf.Abs(_targetPosition) < _minimumCollisionOffset)
        {
            _targetPosition = -_minimumCollisionOffset;
        }

        // Interpola a posição da câmera para suavizar os movimentos
        _cameraTransformPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, _targetPosition, Time.deltaTime / 0.2f);
        _cameraTransform.localPosition = _cameraTransformPosition;
    }
    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(_targetTransform.position, 26);
        //Debug.Log("Number of colliders detected: " + colliders.Length);
        //Debug.Log("Passo 1");
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if(character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - _targetTransform.position;
                float distanceFromTarget = Vector3.Distance(_targetTransform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, _cameraTransform.forward);
                //Debug.Log(character.transform.root != _targetTransform.transform.root);
                //Debug.Log("Passo 3");
                //Debug.Log("Character Root: " + character.transform.name);
                //Debug.Log("Target Root: " + _targetTransform.transform.name);
                RaycastHit hit;

                if(character.transform != _targetTransform.transform
                && viewableAngle > -50 && viewableAngle < 50
                && distanceFromTarget <= _maximumLockOnDistance)
                {
                    //Debug.Log("Passo 4");
                    if(Physics.Linecast(_playerManager.LockOnTransform.position, character.LockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(_playerManager.LockOnTransform.position, character.LockOnTransform.position);
                    
                        if(hit.transform.gameObject.layer == _enviromentLayer)
                        {
                            //Cannot lock onto target, object in the way;
                        }
                        else
                        {
                            _avaliableTargets.Add(character);
                        }
                    }
                }
            }
        }
        for (int k = 0; k < _avaliableTargets.Count; k++)
        {
            float distanceFromTarget = Vector3.Distance(_targetTransform.position, _avaliableTargets[k].transform.position);

            if(distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                _nearestLockOnTarget = _avaliableTargets[k];
            }
            if(_inputHandler.LockOnFlag)
            {
                //Vector3 relativeEnemyPosition = _currentLockOnTarget.transform.InverseTransformDirection(_avaliableTargets[k].transform.position);
                //var distanceFromLeftTarget = _currentLockOnTarget.transform.position.x - _avaliableTargets[k].transform.position.x;
                //var distanceFromRightTarget = _currentLockOnTarget.transform.position.x + _avaliableTargets[k].transform.position.x;
                Vector3 relativeEnemyPosition = _inputHandler.transform.InverseTransformDirection(_avaliableTargets[k].transform.position);
                var distanceFromLeftTarget = relativeEnemyPosition.x;
                var distanceFromRightTarget = relativeEnemyPosition.x;

                if(relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget
                    && _avaliableTargets[k] != _currentLockOnTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    _leftLockOnTarget = _avaliableTargets[k];
                }

                if(relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                    && _avaliableTargets[k] != _currentLockOnTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    _rightLockOnTarget = _avaliableTargets[k];
                }
            }
        }
    }

    public void ClearLockOnTargets()
    {
        _avaliableTargets.Clear();
        _nearestLockOnTarget = null;
        _currentLockOnTarget = null;
    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, _lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, _unlockedPivotPosition);

        if(_currentLockOnTarget != null)
        {
            _cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(_cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            _cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(_cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}
