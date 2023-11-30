using ScriptableObjects;
using TMPro;
using UnityEngine;

public class ItemDisplayScript : MonoBehaviour
{
    private ShopManager _shopManager;

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    private Item _item;

    public void Config(ShopManager shopManager, Item item)
    {
        _shopManager = shopManager;
        _item = item;
        _nameText.text = item.name;
        _priceText.text = $"Price: {item.Cost}";

    }

    public void BuyItem()
    {
        _shopManager.BuyItem(_item.Name);
    }
    
}
