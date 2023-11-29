using Enums;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.InspectedAttributes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private ItemName _itemName;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private ItemTier _itemTier;
        [SerializeField, HideIf("_itemType", ItemType.Log)] private int _maxCount;
        [SerializeField, HideIf("_itemType", ItemType.Log)] private int _cost;
        [SerializeField, HideIf("_itemType", ItemType.Log)] private int _foodValue;
        
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private int _logHealth;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private int _logMultiplier;

        public ItemType Type => _itemType;
        public ItemName Name => _itemName;
        public GameObject Prefab => _prefab;
        public int Cost => _cost;
        public int MaxCount => _maxCount; 
        public ItemTier Tier => _itemTier;
        public int FoodValue => _foodValue;
    }
}