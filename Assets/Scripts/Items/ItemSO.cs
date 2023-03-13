using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    [SerializeField]
    private string _itemName;

    [SerializeField]
    private Sprite _sprite;

    [SerializeField]
    private int _cost;

    [SerializeField]
    private int _maxCount;

    [SerializeField]
    private string _description = "N/A";

    public string itemName 
    {get {return _itemName;}}

    public Sprite sprite
    {get {return _sprite;}}

    public int cost
    {get {return _cost;}}

    public int maxCount
    {get {return _maxCount;}}

    public string description
    {get {return _description;}}

    public virtual bool UseItem()
    {
        return false;
    }
}
