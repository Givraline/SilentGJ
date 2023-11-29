using System;
using Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class JaugeScript : MonoBehaviour
{
    [Header("Jauge bars")] private Vector3 _localScale;
    private float _maxBarAmount;
    [FormerlySerializedAs("barAmount")] [HideInInspector] public float _barAmount;
    [FormerlySerializedAs("Health")] [SerializeField]
    private bool _health;
    [FormerlySerializedAs("logManager")] [HideInInspector] public LogManager _logManager;
    private ShopManager _shopManager;
    [FormerlySerializedAs("multiplier")] [HideInInspector] public float _multiplier;

    private void Start(){
        _maxBarAmount = transform.GetChild(0).transform.localScale.x;
        _barAmount = 0;
        _localScale = transform.GetChild(0).transform.localScale;
        if(!_health){
            ChangeJauge();
        }else{
            _barAmount = _maxBarAmount;
        }
        _shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
    }

    public void ChangeJauge(){
        _localScale.x = _barAmount;
        transform.GetChild(0).transform.localScale = _localScale;
    }

    public void UseObject(int howMuchToAdd, GameObject whatObject, string whatType){
        if(!_health){
            if(_barAmount<_maxBarAmount){
                _barAmount+= _maxBarAmount*((howMuchToAdd/_multiplier)*0.01f);
                // if(whatType=="fruit" && logManager.fruitable){
                //     logManager.fruitable=false;
                //     StartCoroutine(logManager.Cooldown(logManager.CooldownFruits));
                // }else if(whatType=="biscuit" && logManager.biscuitable){
                //     logManager.biscuitable=false;
                //     StartCoroutine(logManager.Cooldown(logManager.CooldownBiscuits));
                // }if(whatType=="bread" && logManager.breadable){
                //     logManager.breadable=false;
                //     StartCoroutine(logManager.Cooldown(logManager.CooldownBread));
                // }
                if(_barAmount>=_maxBarAmount){
                    _barAmount = _maxBarAmount;
                    if(whatType=="Berries" || whatType=="Pear" || whatType=="Orange"){
                        _logManager._fruitAmount = 100;
                    }else if(whatType=="Shortbread" || whatType=="Cookie" || whatType=="Gingerbread"){
                        _logManager._biscuitAmount = 100;
                    }else if(whatType=="BreadSlice" || whatType=="RoundBread" || whatType=="Bun"){
                        _logManager._breadAmount = 100;
                    }
                    _logManager.CheckUpJauges();
                }
                ChangeJauge();
                if(whatType=="Berries"){
                    _shopManager.GetDataHolderOf(ItemName.Berries).AddAmount(-1);
                }else if(whatType=="Pear"){
                    _shopManager.GetDataHolderOf(ItemName.Pear).AddAmount(-1);
                }else if(whatType=="Orange"){
                    _shopManager.GetDataHolderOf(ItemName.Orange).AddAmount(-1);
                }else if(whatType=="Shortbread"){
                    _shopManager.GetDataHolderOf(ItemName.Shortbread).AddAmount(-1);
                }else if(whatType=="Cookie"){
                    _shopManager.GetDataHolderOf(ItemName.Cookie).AddAmount(-1);
                }else if(whatType=="GingerBread"){
                    _shopManager.GetDataHolderOf(ItemName.Gingerbread).AddAmount(-1);
                }else if(whatType=="BreadSlice"){
                    _shopManager.GetDataHolderOf(ItemName.BreadSlice).AddAmount(-1);
                }else if(whatType=="RoundBread"){
                    _shopManager.GetDataHolderOf(ItemName.RoundBread).AddAmount(-1);
                }else if(whatType=="Bun"){
                    _shopManager.GetDataHolderOf(ItemName.Bun).AddAmount(-1);
                }
                Destroy(whatObject);
            }
        }else{
            _barAmount-=_maxBarAmount*(howMuchToAdd*0.01f);
            _logManager._healthAmount = _barAmount;
            if(_barAmount<=0){
                _barAmount=0;
            }
            ChangeJauge();
            _logManager.LogHealthCheckUp();
        }
    }

    public void UseObject(Item item, GameObject whatObject)
    {
        if (_health) return;
        if (!(_barAmount < _maxBarAmount)) return;
        
        _barAmount+= _maxBarAmount*((item.FoodValue/_multiplier)*0.01f);

        if(_barAmount>=_maxBarAmount){
            _barAmount = _maxBarAmount;

            switch (item.Type)
            {
                /*case ItemType.Null:
                    Debug.LogWarning("Null ItemType passed to UseObject");
                    break;*/
                case ItemType.Biscuits:
                    _logManager._biscuitAmount = 100;
                    break;
                case ItemType.Bread:
                    _logManager._breadAmount = 100;
                    break;
                case ItemType.Fruits:
                    _logManager._fruitAmount = 100;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item.Type), item.Type, null);
            }
                    
            _logManager.CheckUpJauges();
        }
        ChangeJauge();
                
        _shopManager.GetDataHolderOf(item.Name).AddAmount(-1);
                
        Destroy(whatObject);
    }
}
