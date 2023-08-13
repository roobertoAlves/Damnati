using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractableUI : MonoBehaviour
{

    [SerializeField] private TMP_Text _interactableText;
    [SerializeField] private TMP_Text _itemText;
    [SerializeField] private RawImage _itemIcon;


    public TMP_Text InteractableText {get { return _interactableText; } set { _interactableText = value; }}
    public TMP_Text ItemText {get { return _itemText; } set { _itemText = value; }}
    public RawImage ItemIcon {get { return _itemIcon; } set { _itemIcon = value; }}
}
