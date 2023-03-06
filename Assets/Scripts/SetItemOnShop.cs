using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetItemOnShop : MonoBehaviour
{
    [SerializeField]
    private ItemSO _itemSO;
    [SerializeField]
    private Image _image;

    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private TextMeshProUGUI _remainingText;
    private string _remainingTextFormat;

    [SerializeField]
    private TextMeshProUGUI _inventoryText;
    private string _inventoryTextFormat;

    [SerializeField]
    private TextMeshProUGUI _costText;

    [SerializeField]
    private Button _buyButton;

    private void Start()
    {
        if (_remainingText != null)
        {
            _remainingTextFormat = _remainingText.text;
        }

        if (_inventoryText != null)
        {
            _inventoryTextFormat = _inventoryText.text;
        }

        if (_buyButton != null)
        {
            if (ShopManager.instance != null)
            {
                _buyButton.onClick.AddListener(() => Buy());
            }
        }

        UpdateValues();
    }

    private void OnDestroy()
    {
        if (_buyButton != null) _buyButton.onClick.RemoveAllListeners();
    }

    private void Buy()
    {
        ShopManager.instance.IncreaseItem(_itemSO);
        ShopManager.instance.Buy();
        //ShopManager.instance.Reset();
        UpdateValues();
    }

    public void UpdateValues()
    {
        if (_itemSO != null && ItemManager.instance != null)
        {
            if (_image != null) _image.sprite = _itemSO.sprite;

            if (_nameText != null)
            {
                _nameText.text = _itemSO.itemName;
            }


            if (_inventoryText != null)
            {
                _inventoryText.text = string.Format(_inventoryTextFormat, ItemManager.instance.GetCount(_itemSO));
            }


            if (ShopManager.instance != null)
            {
                if (_remainingText != null)
                {
                    ShopManager.ItemPurchase itemPurchase = ShopManager.instance.VerifyItem(_itemSO);
                    int count = _itemSO.maxCount - ItemManager.instance.GetCount(_itemSO);
                    if (itemPurchase != null) count = itemPurchase.count;
                    _remainingText.text = string.Format(_remainingTextFormat, count);
                }

                if (_costText != null)
                {
                    _costText.text = _itemSO.cost.ToString();
                }
            }

        }

        return;
    }
}
