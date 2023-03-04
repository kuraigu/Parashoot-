using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;

    [SerializeField]
    private Dictionary<string, ItemSO> _itemDictionary = new Dictionary<string, ItemSO>();
    [SerializeField]
    private List<ItemSO> _itemList = new List<ItemSO>();

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;

        if(_itemList != null)
        {
            foreach(ItemSO item in _itemList)
            {
                if(_itemDictionary != null) _itemDictionary.Add(item.itemName, item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UsePowerUp(PowerUpItemSO powerup)
    {
        powerup.UsePowerUp();
    }
}
