using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    [Header("Movements")]
    public CharacterController2D controller;

	bool jump = false;

    bool DontMove = false;
    float whatDirection = 0;

    float horizontal;
    [SerializeField] float speed = 8f;
    [SerializeField] float jumpingPower = 10f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] SpriteRenderer LogImage;


    [Header("Jauges")]
    [SerializeField] JaugeScript HealthJauge;
    [SerializeField] JaugeScript FruitJauge;
    [SerializeField] JaugeScript BiscuitsJauge;
    [SerializeField] JaugeScript BreadJauge;
    [SerializeField] GameObject HealthBar;

    [HideInInspector] public float healthAmount;
    [HideInInspector] public float fruitAmount;
    [HideInInspector] public float biscuitAmount;
    [HideInInspector] public float breadAmount;
    bool Hittable = false;

    public float multiplier;

    [Header("Cooldowns")]
    public float CooldownHealth = 0.2f;
    public float CooldownFruits = 15f;
    public float CooldownBiscuits = 20f;
    public float CooldownBread = 30f;

    [HideInInspector] public bool fruitable=true;
    [HideInInspector] public bool biscuitable=true;
    [HideInInspector] public bool breadable=true;

    float amountOfFood = 0;



	void Start(){
        HealthJauge.logManager = GetComponent<LogManager>();
        HealthJauge.multiplier = multiplier;
        FruitJauge.logManager = GetComponent<LogManager>();
        FruitJauge.multiplier = multiplier;
        BiscuitsJauge.logManager = GetComponent<LogManager>();
        BiscuitsJauge.multiplier = multiplier;
        BreadJauge.logManager = GetComponent<LogManager>();
        BreadJauge.multiplier = multiplier;
        HealthBar.SetActive(false);
        StartCoroutine(Moving());
    }

	void Update(){
        if(jump && IsGrounded()){
            jump=false;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if(jump && rb.velocity.y > 0f){
            jump=false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
	}

	void FixedUpdate (){
        if(!DontMove){
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            if(horizontal>0){
                LogImage.flipX=false;
            }else{
                LogImage.flipX=true;
            }
        }
	}

    bool IsGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    IEnumerator Moving(){
        yield return new WaitForSeconds(Random.Range(3, 12));
        whatDirection = Random.Range(0, 2);
        DontMove = false;
        if(!DontMove){
            if(whatDirection <= 0.5f){
                horizontal = -1;
            }else if(whatDirection > 0.5f){
                horizontal = 1;
            }
            if(Random.Range(2, 4) > 2.5f){
                jump = true;
            }
        }
        yield return new WaitForSeconds(Random.Range(0.3f, 1.3f));
        horizontal = 0;
        StartCoroutine(Moving());
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.GetComponent<FoodScript>() != null){
            if((col.transform.tag == "Berries" || col.transform.tag == "Pear" || col.transform.tag == "Orange") && fruitable){
                fruitable=false;
                StartCoroutine(cooldownFruit(CooldownFruits));
                FruitJauge.UseObject(col.gameObject.GetComponent<FoodScript>().value, col.gameObject, col.transform.tag);
            }else if((col.transform.tag == "Shortbread" || col.transform.tag == "Cookie" || col.transform.tag == "Gingerbread") && biscuitable){
                biscuitable=false;
                StartCoroutine(cooldownBiscuit(CooldownBiscuits));
                BiscuitsJauge.UseObject(col.gameObject.GetComponent<FoodScript>().value, col.gameObject, col.transform.tag);
            }else if((col.transform.tag == "BreadSlice" || col.transform.tag == "RoundBread" || col.transform.tag == "Bun") && breadable){
                breadable=false;
                StartCoroutine(cooldownBread(CooldownBread));
                BreadJauge.UseObject(col.gameObject.GetComponent<FoodScript>().value, col.gameObject, col.transform.tag);
            }
            amountOfFood++;
        }
        if(Hittable && col.gameObject.GetComponent<StickScript>() != null){
            if(col.transform.tag == "Stick"){
                StartCoroutine(Cooldown(CooldownHealth));
                HealthJauge.UseObject(col.gameObject.GetComponent<StickScript>().value, col.gameObject, "stick");
            }
        }
    }
    public void checkUpJauges(){
        if(fruitAmount==100 && biscuitAmount==100 && breadAmount==100){
            healthAmount = 100;
            HealthBar.SetActive(true);
            Hittable = true;
        }
    }
    public void LogHealthCheckUp(){
        if(healthAmount<=0){
            Hittable = false;
            Debug.Log("you got "+Mathf.RoundToInt(amountOfFood*5)+" leafs");
            GameObject.Find("ShopManager").GetComponent<ShopManager>().Leaf += Mathf.RoundToInt((amountOfFood*15)*multiplier);
            GameObject.Find("ShopManager").GetComponent<ShopManager>().actualLogs--;
            Destroy(this.gameObject);
        }
    }
    public IEnumerator Cooldown(float cd){
        if(Hittable){
            Hittable = false;
            yield return new WaitForSeconds(cd);
            Hittable = true;
        }
    }
    IEnumerator cooldownFruit(float cd){
        if(!fruitable){
            yield return new WaitForSeconds(cd);
            fruitable=true;
        }
    }
    IEnumerator cooldownBiscuit(float cd){
        if(!biscuitable){
            yield return new WaitForSeconds(cd);
            biscuitable=true;
        }
    }
    IEnumerator cooldownBread(float cd){
        if(!breadable){
            yield return new WaitForSeconds(cd);
            breadable=true;
        }
    }
}
