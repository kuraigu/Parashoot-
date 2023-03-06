using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    private static ShopManager _instance;

    [SerializeField]
    private List<ItemPurchase> _itemPurchaseList = new List<ItemPurchase>();
    private int _totalCost = 0;

    public static ShopManager instance
    { get { return _instance; } }

    public List<ItemPurchase> itemPurchaseList
    {get {return _itemPurchaseList;}}

    public void Awake()
    {
        _instance = this;

        Reset();
    }

    public ItemPurchase VerifyItem(ItemSO item)
    {
        foreach (ItemPurchase itemPurchase in _itemPurchaseList)
        {
            if (itemPurchase.item == item)
            {
                return itemPurchase;
            }
        }

        return null;
    }

    public void IncreaseItem(ItemSO item)
    {
        ItemPurchase itemPurchase = VerifyItem(item);
        if (itemPurchase == null)
        {
            _itemPurchaseList.Add(new ItemPurchase(item, 0));
            itemPurchase = _itemPurchaseList.Last();
        }

        if (itemPurchase != null)
        {
            if (ItemManager.instance != null)
            {
                if (ItemManager.instance.IsCountAboveMax(itemPurchase.item, itemPurchase.count + 1) || itemPurchase.count >= itemPurchase.item.maxCount)
                {
                    if (FloatingMessageManager.instance != null)
                    {
                        string text = "Max amount for {0} will be reached";
                        text = string.Format(text, itemPurchase.item.itemName);
                        FloatingMessageManager.instance.SpawnFloatingText(text);
                    }
                }

                else
                {
                    itemPurchase.count++;
                    _totalCost += itemPurchase.item.cost;
                }
            }
        }
    }

    public void DecreaseItem(ItemSO item)
    {
        ItemPurchase itemPurchase = VerifyItem(item);

        if (itemPurchase != null)
        {
            if (itemPurchase.count > 0)
                itemPurchase.count--;
        }
    }

    public void Buy()
    {
        if (DataManager.instance != null && ItemManager.instance != null)
        {
            if (DataManager.instance.data.totalScore >= _totalCost)
            {
                string text = "";
                foreach (ItemPurchase itemPurchase in _itemPurchaseList)
                {
                    if (ItemManager.instance.AddItem(itemPurchase.item, itemPurchase.count) && itemPurchase.count > 0)
                    {
                        text += "Bought {0} x{1}| ";
                        text = string.Format(text, itemPurchase.item.itemName, itemPurchase.count);
                    }
                }

        
                FloatingMessageManager.instance.SpawnFloatingText(text);
                Reset();
            }

            else
            {
                if (FloatingMessageManager.instance != null)
                {
                    FloatingMessageManager.instance.SpawnFloatingText("Not enough total score!");
                }
            }
        }
    }

    public void Reset()
    {
        _itemPurchaseList.Clear();
        _totalCost = 0;
    }

    [System.Serializable]
    public class ItemPurchase
    {
        [SerializeField]
        private ItemSO _item;
        [SerializeField]
        private int _count;

        public ItemSO item
        { get { return _item; } }

        public int count
        { get { return _count; } set { _count = value; } }

        public ItemPurchase()
        { }

        public ItemPurchase(ItemSO item, int count)
        {
            _item = item;
            _count = count;
        }
    }
}
