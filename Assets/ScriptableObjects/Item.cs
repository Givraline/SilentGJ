using System;
using Enums;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.InspectedAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private ItemName _itemName;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private ItemTier _itemTier;
        [SerializeField] private int _cost;
        [SerializeField] private int _maxCount;
        [FormerlySerializedAs("_foodValue")] [SerializeField, HideIf("_itemType", ItemType.Log)] private int _value;
        
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private int _logHealth;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private int _logMultiplier;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private float _logPassiveLeafCooldown;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private float _logHealthCooldown;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private float _logFruitsCooldown;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private float _logBiscuitsCooldown;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private float _logBreadCooldown;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private int _logMinLeafDeathAmount;
        [SerializeField, ShowIf("_itemType", ItemType.Log)] private int _logMaxLeafDeathAmount;

        public ItemType Type => _itemType;
        public ItemName Name => _itemName;
        public GameObject Prefab => _prefab;
        public int Cost => _cost;
        public int MaxCount => _maxCount;
        public ItemTier Tier => _itemTier;
        public int Value => _value;

        public int LogHealth => _logHealth;
        public int LogMultiplier => _logMultiplier;
        public float LogHealthCooldown => _logHealthCooldown;
        public float LogFruitsCooldown => _logFruitsCooldown;
        public float LogBiscuitsCooldown => _logBiscuitsCooldown;
        public float LogBreadCooldown => _logBreadCooldown;
        public float LogPassiveLeafCooldown => _logPassiveLeafCooldown;
        public int GetRandomLeafDeathAmount => Random.Range(_logMinLeafDeathAmount, _logMaxLeafDeathAmount);
    }
}