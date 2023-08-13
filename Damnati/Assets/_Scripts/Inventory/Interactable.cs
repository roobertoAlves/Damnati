using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private string _interactableText;


    #region GET & SET

    public string InteractableText {get { return _interactableText;} set { _interactableText = value; }}
    #endregion
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("vc interagiu com um objeto");
    }
}
