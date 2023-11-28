using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject ShopObject;
    [SerializeField] Button ShopBtn;
    bool activated=false;

    [Header("Money")]
    [HideInInspector] public int Leaf;
    [SerializeField] TextMeshProUGUI leafAmount;
    float timer = 0f;
    [SerializeField] float delayAmount = 2;

    [Header("Logs")]
    [SerializeField] GameObject LogPrb;
    [SerializeField] TextMeshProUGUI LogPrice;
    [SerializeField] int LogsPriceNumber;
    [SerializeField] Button LogBtn;
    [SerializeField] int maxLogs;
    [HideInInspector] public int actualLogs = 0;

    [SerializeField] GameObject RareLogPrb;
    [SerializeField] TextMeshProUGUI RareLogPrice;
    [SerializeField] int RareLogsPriceNumber;
    [SerializeField] Button RareLogBtn;

    [SerializeField] GameObject EpicLogPrb;
    [SerializeField] TextMeshProUGUI EpicLogPrice;
    [SerializeField] int EpicLogsPriceNumber;
    [SerializeField] Button EpicLogBtn;

    [SerializeField] private List<Item> _items;
    private Dictionary<ItemName, DataHolder> _dataHolders = new();

    [SerializeField] private GameObject _itemDisplayPrefab;
    [SerializeField] private List<GameObject> _pagesContent;
    

    bool cd = false;

    private void Awake()
    {
        foreach (Item item in _items)
        {
            _dataHolders[item.Name] = new DataHolder(item);
            ItemDisplayScript itemDisplayScript = Instantiate(_itemDisplayPrefab, _pagesContent[(int)item.Type].transform).GetComponent<ItemDisplayScript>();
            itemDisplayScript.Config(this, item);
        }
    }

    void Start(){
        ShopObject.SetActive(false);
        leafAmount.SetText(Leaf.ToString());
    }

    public DataHolder GetDataHolderOf(ItemName itemName)
    {
        return _dataHolders[itemName];
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.B)&&!cd)
        {
            StartCoroutine(cooldown());
            if(activated){
                ShopObject.SetActive(true);
                activated=false;
            }else{
                ShopObject.SetActive(false);
                activated=true;
            }   
        }
        timer += Time.deltaTime;
        if(timer>= delayAmount){
            timer=0f;
            Leaf++;
            leafAmount.SetText(Leaf.ToString());
        }        
    }

    public void OpenShop(){
        if(!cd){
            StartCoroutine(cooldown());
            if(activated){
                ShopObject.SetActive(true);
                activated=false;
            }else{
                ShopObject.SetActive(false);
                activated=true;
            }  
        }
    }

    public void BuyItem(ItemName itemName)
    {
        DataHolder dataHolder = GetDataHolderOf(itemName);
        if (Leaf >= dataHolder.Cost && !cd && dataHolder.CanHaveMore())
        {
            dataHolder.AddAmount(1);
            StartCoroutine(cooldown());
            WriteLeaf(dataHolder.Cost);
            Instantiate(dataHolder.Prefab);
        }
    }

    void WriteLeaf(int substractLeaf){
        Leaf-=substractLeaf;
        leafAmount.SetText(Leaf.ToString());
    }
    IEnumerator cooldown(){
        cd=true;
        yield return new WaitForSeconds(0.2f);
        cd = false;
    }

        
}

public class DataHolder{
    
    public ItemName GetName { get; private set; }
    public int Count { get; private set; }
    public Item Item { get; private set; }
    public int MaxCount => Item.MaxCount;
    public int Cost => Item.Cost;
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

