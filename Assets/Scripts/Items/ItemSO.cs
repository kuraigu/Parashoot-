using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    [SerializeField]
    private string _itemName;

    [SerializeField]
    private Sprite _sprite;

    public string itemName 
    {get {return _itemName;}}

    public Sprite sprite
    {get {return _sprite;}}
}
