using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LogManager : MonoBehaviour
{
    [Header("Movements")] private bool _jump = false;

    private bool _dontMove = false;
    private float _whatDirection = 0;

    private float _horizontal;
    [FormerlySerializedAs("speed")] [SerializeField]
    private float _speed = 8f;
    [FormerlySerializedAs("jumpingPower")] [SerializeField]
    private float _jumpingPower = 10f;
    [FormerlySerializedAs("rb")] [SerializeField]
    private Rigidbody2D _rb;
    [FormerlySerializedAs("groundCheck")] [SerializeField]
    private Transform _groundCheck;
    [FormerlySerializedAs("groundLayer")] [SerializeField]
    private LayerMask _groundLayer;
    [FormerlySerializedAs("LogImage")] [SerializeField]
    private SpriteRenderer _logImage;


    [FormerlySerializedAs("HealthJauge")]
    [Header("Jauges")]
    [SerializeField]
    private JaugeScript _healthJauge;
    [FormerlySerializedAs("FruitJauge")] [SerializeField]
    private JaugeScript _fruitJauge;
    [FormerlySerializedAs("BiscuitsJauge")] [SerializeField]
    private JaugeScript _biscuitsJauge;
    [FormerlySerializedAs("BreadJauge")] [SerializeField]
    private JaugeScript _breadJauge;
    [FormerlySerializedAs("HealthBar")] [SerializeField]
    private GameObject _healthBar;

    [FormerlySerializedAs("healthAmount")] [HideInInspector] public float _healthAmount;
    [FormerlySerializedAs("fruitAmount")] [HideInInspector] public float _fruitAmount;
    [FormerlySerializedAs("biscuitAmount")] [HideInInspector] public float _biscuitAmount;
    [FormerlySerializedAs("breadAmount")] [HideInInspector] public float _breadAmount;
    private bool _hittable = false;
    
    [FormerlySerializedAs("CooldownHealth")] [Header("Cooldowns")]

    [FormerlySerializedAs("fruitable")] [HideInInspector] public bool _fruitable=true;
    [FormerlySerializedAs("biscuitable")] [HideInInspector] public bool _biscuitable=true;
    [FormerlySerializedAs("breadable")] [HideInInspector] public bool _breadable=true;

    private Item _logItem;
    private bool _leafGeneration;

    public Item GetLogItem() => _logItem;
    
    private ShopManager _shopManager;

    public ShopManager GetShopManager() => _shopManager;

    private float _amountOfFood = 0;
    [SerializeField] private ItemTier _tier;
    private Dictionary<ItemType, JaugeScript> _itemJaugeDic = new Dictionary<ItemType, JaugeScript>(4);

    private void Awake()
    {
        _leafGeneration = true;
        _itemJaugeDic[ItemType.Biscuits] = _biscuitsJauge;
        _itemJaugeDic[ItemType.Bread] = _breadJauge;
        _itemJaugeDic[ItemType.Fruits] = _fruitJauge;
        _fruitable = true;
        _biscuitable = true;
        _breadable = true;
    }

    private void Start() 
    {
        _healthBar.SetActive(false);
        StartCoroutine(Moving());
    }

    public LogManager Config(ShopManager manager, Item logItem)
    {
        _shopManager = manager;
        _logItem = logItem;
        _healthJauge._logManager = this;
        _healthJauge._multiplier = _logItem.LogMultiplier;
        _fruitJauge._logManager = this;
        _fruitJauge._multiplier = _logItem.LogMultiplier;
        _biscuitsJauge._logManager = this;
        _biscuitsJauge._multiplier = _logItem.LogMultiplier;
        _breadJauge._logManager = this;
        _breadJauge._multiplier = _logItem.LogMultiplier;
        return this;
    }

    private void Update(){
        if(_jump && IsGrounded()){
            _jump=false;
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpingPower);
        }
        if(_jump && _rb.velocity.y > 0f){
            _jump=false;
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }
        if (_leafGeneration)
        {
            _shopManager.AddLeaf(1);
            StartCoroutine(CooldownLeaves(_logItem.LogPassiveLeafCooldown));
        }
	}

    private IEnumerator CooldownLeaves(float cooldown)
    {
        _leafGeneration = false;
        yield return new WaitForSeconds(cooldown);
        _leafGeneration = true;
    }

    private void FixedUpdate (){
        if(!_dontMove){
            _rb.velocity = new Vector2(_horizontal * _speed, _rb.velocity.y);
            if(_horizontal>0){
                _logImage.flipX=false;
            }else{
                _logImage.flipX=true;
            }
        }
	}

    private bool IsGrounded(){
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }

    private IEnumerator Moving(){
        yield return new WaitForSeconds(Random.Range(3, 12));
        _whatDirection = Random.Range(0, 2);
        _dontMove = false;
        if(!_dontMove){
            if(_whatDirection <= 0.5f){
                _horizontal = -1;
            }else if(_whatDirection > 0.5f){
                _horizontal = 1;
            }
            if(Random.Range(2, 4) > 2.5f){
                _jump = true;
            }
        }
        yield return new WaitForSeconds(Random.Range(0.3f, 1.3f));
        _horizontal = 0;
        StartCoroutine(Moving());
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        FoodScript foodScript = col.gameObject.GetComponent<FoodScript>();
        if(foodScript != null){

            if ((int)foodScript.GetItem.Tier >= (int)_tier)
            {
                switch (foodScript.GetItem.Type)
                {
                    case ItemType.Biscuits:
                        if (!_biscuitable) return;
                        StartCoroutine(CooldownBiscuit(_logItem.LogBiscuitsCooldown));
                        break;
                    case ItemType.Bread:
                        if(!_breadable) return;
                        StartCoroutine(CooldownBread(_logItem.LogBreadCooldown));
                        break;
                    case ItemType.Fruits:
                        if (!_fruitable) return;
                        StartCoroutine(CooldownFruit(_logItem.LogFruitsCooldown));
                        break;
                    case ItemType.Log:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _itemJaugeDic[foodScript.GetItem.Type].UseObject(foodScript.GetItem, col.gameObject);
            }
            _amountOfFood++;
        }
        
        if(_hittable && col.gameObject.GetComponent<StickScript>() != null){
            if(col.transform.CompareTag("Stick")){
                StartCoroutine(CooldownHit(_logItem.LogHealthCooldown));
                _healthJauge.UseObject(col.gameObject.GetComponent<StickScript>().GetItem, col.gameObject);
            }
        }
    }


    public void CheckUpJauges(){
        if(_fruitAmount>=100 && _biscuitAmount>=100 && _breadAmount>=100){
            _healthAmount = 100;
            _healthBar.SetActive(true);
            _hittable = true;
        }
    }
    public void LogHealthCheckUp(){
        if(_healthAmount<=0){
            _hittable = false;
            Debug.Log("you got "+_logItem.GetRandomLeafDeathAmount+" leafs");
            _shopManager.AddLeaf(_logItem.GetRandomLeafDeathAmount);
            _shopManager._actualLogs--;
            Destroy(gameObject);
        }
    }

    private IEnumerator CooldownHit(float cd){
        if(_hittable){
            _hittable = false;
            yield return new WaitForSeconds(cd);
            _hittable = true;
        }
    }

    private IEnumerator CooldownFruit(float cd)
    {
        _fruitable = false;
        yield return new WaitForSeconds(cd);
        _fruitable=_fruitAmount<100;
    }

    private IEnumerator CooldownBiscuit(float cd)
    {
        _biscuitable = false;
        yield return new WaitForSeconds(cd);
        _biscuitable=_biscuitAmount<100;
    }

    private IEnumerator CooldownBread(float cd)
    {
        _breadable = false;
        yield return new WaitForSeconds(cd);
        _breadable=_breadAmount<100;
    }
}
