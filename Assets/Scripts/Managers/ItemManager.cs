using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;

    private Dictionary<string, ItemSO> _itemDictionary = new Dictionary<string, ItemSO>();
    private Dictionary<string, DataManager.Data.Item> _dataItemsDictionary = new Dictionary<string, DataManager.Data.Item>();
    [SerializeField]
    private List<ItemSO> _itemList = new List<ItemSO>();

    public static ItemManager instance
    {get {return _instance;}}

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;

        if (_itemList != null)
        {
            foreach (ItemSO item in _itemList)
            {
                if (!_itemDictionary.ContainsKey(item.itemName))
                {
                    _itemDictionary.Add(item.itemName, item);
                }
            }
        }


        SyncItemsToHere();
    }


    // Update is called once per frame
    void Update()
    {

    }


    public int GetCount(ItemSO item)
    {
         if (_dataItemsDictionary != null)
        {
            if (_dataItemsDictionary.ContainsKey(item.itemName))
            {
                return _dataItemsDictionary[item.itemName].count;
            }
        }

        return 0;
    }


    public bool IsCountValid(ItemSO item, int minCount = 0)
    {
        if (_dataItemsDictionary != null)
        {
            if (_dataItemsDictionary.ContainsKey(item.itemName))
            {
                if (_dataItemsDictionary[item.itemName].count > minCount)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }


    public bool IsCountAboveMax(ItemSO item, int amount = 0)
    {
        if (_dataItemsDictionary != null)
        {
            if (_dataItemsDictionary.ContainsKey(item.itemName))
            {
                if (_dataItemsDictionary[item.itemName].count + amount > item.maxCount)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }

    public bool IsCountEqualMax(ItemSO item, int amount = 0)
    {
        if (_dataItemsDictionary != null)
        {
            if (_dataItemsDictionary.ContainsKey(item.itemName))
            {
                if (_dataItemsDictionary[item.itemName].count + amount == item.maxCount)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }



    public void FixItemCount(ItemSO item)
    {
        if (IsCountAboveMax(item))
        {
            if (_dataItemsDictionary.ContainsKey(item.itemName))
            {
                _dataItemsDictionary[item.itemName].count = item.maxCount;
            }
        }
    }

    public bool ReduceItem(ItemSO item, int amount = 1)
    {
        if (IsCountValid(item, amount))
        {
            if (_dataItemsDictionary.ContainsKey(item.itemName))
            {
                _dataItemsDictionary[item.itemName].count -= amount;

                if(!IsCountValid(_itemDictionary[item.itemName]))
                {
                    _dataItemsDictionary.Remove(item.itemName);
                }

                SyncItemsToData();

                return false;
            }

            return false;
        }

        return false;
    }


    public bool AddItem(ItemSO item, int amount = 1)
    {
        if (!IsCountAboveMax(item, amount))
        {
            if (!_dataItemsDictionary.ContainsKey(item.itemName)) 
            {
                if(_itemDictionary.ContainsKey(item.itemName))
                {
                    _dataItemsDictionary.Add(item.itemName, new DataManager.Data.Item(item.itemName, amount));

                    SyncItemsToData();

                    return true;
                }
            }

            else if(_dataItemsDictionary.ContainsKey(item.itemName)) 
            {
                _dataItemsDictionary[item.itemName].count += amount;

                SyncItemsToData();

                return true;
            }

            return false;
        }

        return false;
    }


    public void UsePowerUp(PowerUpItemSO powerUp)
    {
        if (IsCountValid((ItemSO)powerUp, 1))
        {
            if (powerUp.UsePowerUp())
            {
                ReduceItem(powerUp);

                if (FloatingMessageManager.instance != null)
                {
                    string text = "Used {0}";
                    text = string.Format(text, powerUp.itemName);
                    FloatingMessageManager.instance.SpawnFloatingText(text);

                    return;
                }
            }
        }

        else 
        {
            string text = "You have no {0}";
            text = string.Format(text, powerUp.itemName);

            FloatingMessageManager.instance.SpawnFloatingText(text);

        }
    }

    public void SyncItemsToHere()
    {
        if (DataManager.instance != null)
        {
            if (DataManager.instance.data.items != null)
            {
                foreach (DataManager.Data.Item item in DataManager.instance.data.items)
                {
                    if (_itemDictionary.ContainsKey(item.itemName))
                    {
                        _dataItemsDictionary.Add(item.itemName, item);

                        FixItemCount(_itemDictionary[item.itemName]);
                    }
                }
            }
        }
    }


    public void SyncItemsToData()
    {
        if (DataManager.instance != null)
        {
            DataManager.Data.Item[] items = new DataManager.Data.Item[_dataItemsDictionary.Count];

            int i = 0;
            foreach (DataManager.Data.Item item in _dataItemsDictionary.Values)
            {
                items[i] = item;

                if (_itemDictionary.ContainsKey(item.itemName))
                {
                    FixItemCount(_itemDictionary[item.itemName]);
                }

                i++;
            }

            DataManager.instance.data.items = items;

            DataManager.instance.SaveData();
        }
    }
}
