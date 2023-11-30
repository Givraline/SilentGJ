using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayScript : MonoBehaviour
{
    private ShopManager _shopManager;

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _image;
    private Item _item;

    public void Config(ShopManager shopManager, Item item)
    {
        _shopManager = shopManager;
        _item = item;
        SpriteRenderer[] sprites = _item.Prefab.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite.gameObject.name == "Image")
            {
                _image.sprite = sprite.sprite;
                break;
            };
        }
        _nameText.text = item.name;
        _priceText.text = $"Price: {item.Cost}";

    }

    public void BuyItem()
    {
        _shopManager.BuyItem(_item.Name);
    }
    
}
