using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private ItemName _itemName;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _maxCount;
        [SerializeField] private int _cost;

        public ItemType Type => _itemType;
        public ItemName Name => _itemName;
        public GameObject Prefab => _prefab;
        public int Cost => _cost;
        public int MaxCount => _maxCount;
    }
}