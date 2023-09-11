using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : ScriptableObject
{
    [SerializeField] private string _actionAnimation;
    [SerializeField] private bool _isRightHandedAction = true;

    #region GET & SET
    public string ActionAnimation { get { return _actionAnimation; } set { _actionAnimation = value; }}
    public bool IsRightHandedAction { get { return _isRightHandedAction; } set { _isRightHandedAction = value; }}
    #endregion
}
