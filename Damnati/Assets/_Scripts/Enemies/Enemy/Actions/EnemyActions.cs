using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : ScriptableObject
{
    [SerializeField] private string _actionAnimation;

    #region GET & SET
    public string ActionAnimation { get { return _actionAnimation; } set { _actionAnimation = value; }}

    #endregion
}
