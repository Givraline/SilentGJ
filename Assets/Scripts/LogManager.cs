using System;
using System.Collections;
using Enums;
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

    [FormerlySerializedAs("multiplier")] public float _multiplier;

    [FormerlySerializedAs("CooldownHealth")] [Header("Cooldowns")]
    public float _cooldownHealth = 0.2f;
    [FormerlySerializedAs("CooldownFruits")] public float _cooldownFruits = 15f;
    [FormerlySerializedAs("CooldownBiscuits")] public float _cooldownBiscuits = 20f;
    [FormerlySerializedAs("CooldownBread")] public float _cooldownBread = 30f;

    [FormerlySerializedAs("fruitable")] [HideInInspector] public bool _fruitable=true;
    [FormerlySerializedAs("biscuitable")] [HideInInspector] public bool _biscuitable=true;
    [FormerlySerializedAs("breadable")] [HideInInspector] public bool _breadable=true;

    private float _amountOfFood = 0;
    [SerializeField] private ItemTier _tier;


    private void Start(){
        _healthJauge._logManager = GetComponent<LogManager>();
        _healthJauge._multiplier = _multiplier;
        _fruitJauge._logManager = GetComponent<LogManager>();
        _fruitJauge._multiplier = _multiplier;
        _biscuitsJauge._logManager = GetComponent<LogManager>();
        _biscuitsJauge._multiplier = _multiplier;
        _breadJauge._logManager = GetComponent<LogManager>();
        _breadJauge._multiplier = _multiplier;
        _healthBar.SetActive(false);
        StartCoroutine(Moving());
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

            if ((int)foodScript.GetItem.Tier <= (int)_tier)
            {
                switch (foodScript.GetItem.Type)
                {
                    case ItemType.Null:
                        break;
                    case ItemType.Biscuits:
                        StartCoroutine(CooldownBiscuit(_cooldownBiscuits));
                        break;
                    case ItemType.Bread:
                        StartCoroutine(CooldownBread(_cooldownBread));
                        break;
                    case ItemType.Fruits:
                        StartCoroutine(CooldownFruit(_cooldownFruits));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _fruitJauge.UseObject(foodScript.GetItem, col.gameObject);
            }
            
            if((col.transform.CompareTag("Berries") || col.transform.CompareTag("Pear") || col.transform.CompareTag("Orange")) && _fruitable){
                _fruitable=false;
                _fruitJauge.UseObject(foodScript._value, col.gameObject, col.transform.tag);
            }else if((col.transform.CompareTag("Shortbread") || col.transform.CompareTag("Cookie") || col.transform.CompareTag("Gingerbread")) && _biscuitable){
                _biscuitable=false;
                _biscuitsJauge.UseObject(foodScript._value, col.gameObject, col.transform.tag);
            }else if((col.transform.CompareTag("BreadSlice") || col.transform.CompareTag("RoundBread") || col.transform.CompareTag("Bun")) && _breadable){
                _breadable=false;
                StartCoroutine(CooldownBread(_cooldownBread));
                _breadJauge.UseObject(foodScript._value, col.gameObject, col.transform.tag);
            }
            _amountOfFood++;
        }
        
        if(_hittable && col.gameObject.GetComponent<StickScript>() != null){
            if(col.transform.CompareTag("Stick")){
                StartCoroutine(CooldownHit(_cooldownHealth));
                _healthJauge.UseObject(col.gameObject.GetComponent<StickScript>()._value, col.gameObject, "stick");
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
            Debug.Log("you got "+Mathf.RoundToInt(_amountOfFood*5)+" leafs");
            GameObject.Find("ShopManager").GetComponent<ShopManager>()._leaf += Mathf.RoundToInt((_amountOfFood*15)*_multiplier);
            GameObject.Find("ShopManager").GetComponent<ShopManager>()._actualLogs--;
            Destroy(this.gameObject);
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
        _fruitable=true;
    }

    private IEnumerator CooldownBiscuit(float cd)
    {
        _biscuitable = false;
        yield return new WaitForSeconds(cd);
        _biscuitable=true;
    }

    private IEnumerator CooldownBread(float cd)
    {
        _breadable = false;
        yield return new WaitForSeconds(cd);
        _breadable=true;
    }
}
