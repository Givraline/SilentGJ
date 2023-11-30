using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [FormerlySerializedAs("ShopObject")] [SerializeField]
    private GameObject _shopObject;

    private bool _activated=false;

    [FormerlySerializedAs("Leaf")]
    [Header("Money")]
    [HideInInspector] public int _leaf;
    [FormerlySerializedAs("leafAmount")] [SerializeField]
    private TextMeshProUGUI _leafAmount;

    private float _timer = 0f;
    [FormerlySerializedAs("delayAmount")] [SerializeField]
    private float _delayAmount = 2;

    [FormerlySerializedAs("LogPrb")]
    [Header("Logs")]
    [SerializeField]
    private GameObject _logPrb;
    [FormerlySerializedAs("LogPrice")] [SerializeField]
    private TextMeshProUGUI _logPrice;
    [FormerlySerializedAs("LogsPriceNumber")] [SerializeField]
    private int _logsPriceNumber;
    [FormerlySerializedAs("LogBtn")] [SerializeField]
    private Button _logBtn;
    [FormerlySerializedAs("maxLogs")] [SerializeField]
    private int _maxLogs;
    [FormerlySerializedAs("actualLogs")] [HideInInspector] public int _actualLogs = 0;

    [FormerlySerializedAs("RareLogPrb")] [SerializeField]
    private GameObject _rareLogPrb;
    [FormerlySerializedAs("RareLogPrice")] [SerializeField]
    private TextMeshProUGUI _rareLogPrice;
    [FormerlySerializedAs("RareLogsPriceNumber")] [SerializeField]
    private int _rareLogsPriceNumber;
    [FormerlySerializedAs("RareLogBtn")] [SerializeField]
    private Button _rareLogBtn;

    [FormerlySerializedAs("EpicLogPrb")] [SerializeField]
    private GameObject _epicLogPrb;
    [FormerlySerializedAs("EpicLogPrice")] [SerializeField]
    private TextMeshProUGUI _epicLogPrice;
    [FormerlySerializedAs("EpicLogsPriceNumber")] [SerializeField]
    private int _epicLogsPriceNumber;
    [FormerlySerializedAs("EpicLogBtn")] [SerializeField]
    private Button _epicLogBtn;

    [SerializeField] private List<Item> _items;
    private Dictionary<ItemName, DataHolder> _dataHolders = new();

    [SerializeField] private GameObject _itemDisplayPrefab;
    [SerializeField] private List<GameObject> _pagesContent;


    private bool _cd = false;

    private void Awake()
    {
        _items.Sort((item, item1) => item.Cost.CompareTo(item1.Cost));
        foreach (Item item in _items)
        {
            _dataHolders[item.Name] = new DataHolder(item);
            ItemDisplayScript itemDisplayScript = Instantiate(_itemDisplayPrefab, _pagesContent[(int)item.Type].transform).GetComponent<ItemDisplayScript>();
            itemDisplayScript.Config(this, item);
        }
    }

    private void Start(){
        _shopObject.SetActive(false);
        _activated = false;
        _leafAmount.SetText(_leaf.ToString());
    }

    public DataHolder GetDataHolderOf(ItemName itemName)
    {
        return _dataHolders[itemName];
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(!_activated){
                _shopObject.SetActive(true);
                _activated=true;
            }else{
                _shopObject.SetActive(false);
                _activated=false;
            }   
        }
        _timer += Time.deltaTime;
        if(_timer>= _delayAmount){
            _timer=0f;
            //_leaf++;
            _leafAmount.SetText(_leaf.ToString());
        }        
    }

    public void AddLeaf(int value)
    {
        _leaf+= value;
        _leafAmount.SetText(_leaf.ToString());
    }

    public void OpenShop(){
        if(!_activated){
            _shopObject.SetActive(true);
            _activated=true;
        }else{
            _shopObject.SetActive(false);
            _activated=false;
        }  
    }
    
    public void BuyItem(ItemName itemName)
    {
        DataHolder dataHolder = GetDataHolderOf(itemName);
        if (_leaf >= dataHolder.Cost && !_cd && dataHolder.CanHaveMore())
        {
            dataHolder.AddAmount(1);
            StartCoroutine(Cooldown());
            WriteLeaf(dataHolder.Cost);

            switch (dataHolder.Type)
            {
                case ItemType.Log:
                    LogManager logScript = Instantiate(dataHolder.Prefab).GetComponent<LogManager>();
                    logScript.Config(this, dataHolder.Item);
                    break;
                case ItemType.Biscuits:
                case ItemType.Bread:
                case ItemType.Fruits:
                    FoodScript foodScript = Instantiate(dataHolder.Prefab).GetComponent<FoodScript>();
                    foodScript.SetItem(dataHolder.Item);
                    break;
                case ItemType.Stick:
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }

    private void WriteLeaf(int substractLeaf){
        _leaf-=substractLeaf;
        _leafAmount.SetText(_leaf.ToString());
    }

    private IEnumerator Cooldown(){
        _cd = true;
        yield return new WaitForSeconds(0.2f);
        _cd = false;
    }

        
}

public class DataHolder{
    
    public ItemName GetName { get; private set; }
    public int Count { get; private set; }
    public Item Item { get; private set; }
    public int MaxCount => Item.MaxCount;
    public int Cost => Item.Cost;
    public ItemType Type => Item.Type;
    public GameObject Prefab => Item.Prefab;
    
    public DataHolder(Item item)
    {
        Item = item;
    }

    public void AddAmount(int amount)
    {
        Count += amount;
    }

    public bool CanHaveMore()
    {
        return Count < MaxCount;
    }
}

