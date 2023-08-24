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
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _cameraPivotTransform;
    private Transform _myTransform;
    private Vector3 _cameraTransformPosition;
    [SerializeField] private LayerMask _ignoreLayers;
    private LayerMask _enviromentLayer;
    private Vector3 _cameraFollowVelocity = Vector3.zero;


    [Header("Camera Settings")]
    [Space(15)]
    [SerializeField] private float _lookSpeed = 0.1f;
    [SerializeField] private float _followSpeed = 0.1f;
    [SerializeField] private float _pivotSpeed = 0.03f;

    private float _targetPosition;
    private float _defaultPosition;
    private float _lookAngle;
    private float _pivotAngle;

    [Header("Camera Angle")]
    [Space(15)]
    [SerializeField] private float _minimumPivot = -35;
    [SerializeField] private float _maximumPivot = 35;

    [Header("Camera Collision")]
    [Space(15)]
    private float _cameraSphereRadius = 0.2f;
    private float _cameraCollisionOffSet = 0.2f;
    private float _minimumCollisionOffset = 0.2f;

    [Header("Lock On Settings")]
    [Space(15)]
    private Transform _currentLockOnTarget;
    [SerializeField] private List<CharacterManager> _avaliableTargets = new List<CharacterManager>();
    private Transform _nearestLockOnTarget;
    [SerializeField] private Transform _leftLockOnTarget;
    [SerializeField] private Transform _rightLockOnTarget;
    [Space(15)]
    [SerializeField] private float _maximumLockOnDistance = 30;
    [Space(15)]
    [SerializeField] private float _lockedPivotPosition = 2.25f;
    [SerializeField] private float _unlockedPivotPosition = 1.65f;

    #region GET & SET
    public Transform CameraTransform { get { return _cameraTransform; } set { _cameraTransform = value; }}
    public Transform CurrentLockOnTarget { get { return _currentLockOnTarget; } set { _currentLockOnTarget = value; }}
    public Transform NearestLockOnTarget { get { return _nearestLockOnTarget; } set { _nearestLockOnTarget = value; }}
    public Transform LeftLockOnTarget { get { return _leftLockOnTarget; } set { _leftLockOnTarget = value; }}
    public Transform RightLockOnTarget { get { return _rightLockOnTarget; } set { _rightLockOnTarget = value; }}
    #endregion

    private void Awake()
    {
        _myTransform = transform;
        _defaultPosition = _cameraTransform.localPosition.z;
        _targetTransform = FindObjectOfType<PlayerManager>().transform;
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start() 
    {
        _enviromentLayer = LayerMask.NameToLayer("Environment");    
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(_myTransform.position, _targetTransform.position, ref _cameraFollowVelocity, delta / _followSpeed);
        _myTransform.position = targetPosition;

        HandleCameraCollisions(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if(_inputHandler.LockOnFlag == false && _currentLockOnTarget == null)
        {
            _lookAngle += (mouseXInput * _lookSpeed * Time.deltaTime) / delta;
            _pivotAngle -= (mouseYInput * _pivotSpeed * Time.deltaTime) / delta;
            _pivotAngle = Mathf.Clamp(_pivotAngle, _minimumPivot, _maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = _lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            _myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = _pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            _cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            Vector3 dir = _currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = _currentLockOnTarget.transform.position - _cameraPivotTransform.transform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngles = targetRotation.eulerAngles;
            eulerAngles.y = 0;
            _cameraPivotTransform.localEulerAngles = eulerAngles;
        }
    }

    private void HandleCameraCollisions(float delta)
    {
        _targetPosition = _defaultPosition;
        RaycastHit hit;
        Vector3 direction = _cameraTransform.position - _cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(_cameraPivotTransform.position, _cameraSphereRadius, direction, out hit, Mathf.Abs(_targetPosition), _ignoreLayers))
        {
            float dis = Vector3.Distance(_cameraPivotTransform.position, hit.point);
            _targetPosition = -(dis - _cameraCollisionOffSet);
        }

        if (Mathf.Abs(_targetPosition) < _minimumCollisionOffset)
        {
            _targetPosition = -_minimumCollisionOffset;
        }

        _cameraTransformPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, _targetPosition, delta / 0.2f);
        _cameraTransform.localPosition = _cameraTransformPosition;
    }
    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(_targetTransform.position, 26);
        Debug.Log("Number of colliders detected: " + colliders.Length);
        Debug.Log("Passo 1");
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();
            if (character != null)
            {
                Debug.Log("CharacterManager found!");
                Debug.Log("Passo 2");
            }

            if(character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - _targetTransform.position;
                float distanceFromTarget = Vector3.Distance(_targetTransform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, _cameraTransform.forward);
                Debug.Log(character.transform.root != _targetTransform.transform.root);
                Debug.Log("Passo 3");
                Debug.Log("Character Root: " + character.transform.name);
                Debug.Log("Target Root: " + _targetTransform.transform.name);
                RaycastHit hit;

                if(character.transform != _targetTransform.transform)
                {
                    Debug.Log("Diferentes");
                }

                if(character.transform != _targetTransform.transform
                && viewableAngle > -50 && viewableAngle < 50
                && distanceFromTarget <= _maximumLockOnDistance)
                {
                    Debug.Log("Passo 4");
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
                _nearestLockOnTarget = _avaliableTargets[k].LockOnTransform;
            }
            if(_inputHandler.LockOnFlag)
            {
                Vector3 _relativeEnemyPosition = _currentLockOnTarget.InverseTransformDirection(_avaliableTargets[k].transform.position);
                var distanceFromLeftTarget = _currentLockOnTarget.transform.position.x - _avaliableTargets[k].transform.position.x;
                var distanceFromRightTarget = _currentLockOnTarget.transform.position.x + _avaliableTargets[k].transform.position.x;
            
                if(_relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    _leftLockOnTarget = _avaliableTargets[k].LockOnTransform;
                }

                if(_relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    _rightLockOnTarget = _avaliableTargets[k].LockOnTransform;
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
