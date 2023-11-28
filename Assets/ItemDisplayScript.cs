using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDisplayScript : MonoBehaviour
{
    private ShopManager _shopManager;

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;

    void Config(ShopManager shopManager)
    {
        _shopManager = shopManager;
        // _item = item;
    }

    public void BuyItem()
    {
        // _shopManager.BuyItem(this.itemType);
    }

    private void SetPriceText()
    {
        int amount = 0;
        _priceText.text = $"Price: {amount}";
    }
}
