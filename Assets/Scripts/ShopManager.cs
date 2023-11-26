using System.Collections;
using System.Collections.Generic;
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


    [Header("Fruits")]
    [SerializeField] GameObject BerriesPrb;
    [SerializeField] TextMeshProUGUI BerriesPrice;
    [SerializeField] int BerriesPriceNumber;
    [SerializeField] Button BerriesBtn;
    [SerializeField] int maxBerries;
    [HideInInspector] public int actualBerries = 0;
    
    [SerializeField] GameObject PearPrb;
    [SerializeField] TextMeshProUGUI PearPrice;
    [SerializeField] int PearPriceNumber;
    [SerializeField] Button PearBtn;
    [SerializeField] int maxPear;
    [HideInInspector] public int actualPear = 0;

    [SerializeField] GameObject OrangePrb;
    [SerializeField] TextMeshProUGUI OrangePrice;
    [SerializeField] int OrangePriceNumber;
    [SerializeField] Button OrangeBtn;
    [SerializeField] int maxOrange;
    [HideInInspector] public int actualOrange = 0;

    [Header("Bread")]
    [SerializeField] GameObject SliceOfBreadPrb;
    [SerializeField] TextMeshProUGUI SliceOfBreadPrice;
    [SerializeField] int SliceOfBreadPriceNumber;
    [SerializeField] Button SliceOfBreadBtn;
    [SerializeField] int maxSliceOfBread;
    [HideInInspector] public int actualSliceOfBread = 0;
    
    [SerializeField] GameObject RoundBreadPrb;
    [SerializeField] TextMeshProUGUI RoundBreadPrice;
    [SerializeField] int RoundBreadPriceNumber;
    [SerializeField] Button RoundBreadBtn;
    [SerializeField] int maxRoundBread;
    [HideInInspector] public int actualRoundBread = 0;

    [SerializeField] GameObject BunPrb;
    [SerializeField] TextMeshProUGUI BunPrice;
    [SerializeField] int BunPriceNumber;
    [SerializeField] Button BunBtn;
    [SerializeField] int maxBun;
    [HideInInspector] public int actualBun = 0;

    [Header("Biscuits")]
    [SerializeField] GameObject ShortBreadPrb;
    [SerializeField] TextMeshProUGUI ShortBreadPrice;
    [SerializeField] int ShortBreadPriceNumber;
    [SerializeField] Button ShortBreadBtn;
    [SerializeField] int maxShortBread;
    [HideInInspector] public int actualShortBread = 0;
    
    [SerializeField] GameObject CookiePrb;
    [SerializeField] TextMeshProUGUI CookiePrice;
    [SerializeField] int CookiePriceNumber;
    [SerializeField] Button CookieBtn;
    [SerializeField] int maxCookie;
    [HideInInspector] public int actualCookie = 0;

    [SerializeField] GameObject GingerBreadPrb;
    [SerializeField] TextMeshProUGUI GingerBreadPrice;
    [SerializeField] int GingerBreadPriceNumber;
    [SerializeField] Button GingerBreadBtn;
    [SerializeField] int maxGingerBread;
    [HideInInspector] public int actualGingerBread = 0;

    bool cd = false;

    void Start(){
        ShopBtn.onClick.AddListener(OpenShop);
        LogBtn.onClick.AddListener(BuyLog);
        RareLogBtn.onClick.AddListener(BuyRareLog);
        EpicLogBtn.onClick.AddListener(BuyEpicLog);
        BerriesBtn.onClick.AddListener(BuyBerries);
        PearBtn.onClick.AddListener(BuyPear);
        OrangeBtn.onClick.AddListener(BuyOrange);
        SliceOfBreadBtn.onClick.AddListener(BuySliceOfBread);
        RoundBreadBtn.onClick.AddListener(BuyRoundBread);
        BunBtn.onClick.AddListener(BuyBun);
        ShortBreadBtn.onClick.AddListener(BuyShortBread);
        CookieBtn.onClick.AddListener(BuyCookie);
        GingerBreadBtn.onClick.AddListener(BuyGingerBread);


        LogPrice.SetText(LogsPriceNumber.ToString());
        RareLogPrice.SetText(RareLogsPriceNumber.ToString());
        EpicLogPrice.SetText(EpicLogsPriceNumber.ToString());
        BerriesPrice.SetText(BerriesPriceNumber.ToString());
        PearPrice.SetText(PearPriceNumber.ToString());
        OrangePrice.SetText(OrangePriceNumber.ToString());
        SliceOfBreadPrice.SetText(SliceOfBreadPriceNumber.ToString());
        RoundBreadPrice.SetText(RoundBreadPriceNumber.ToString());
        BunPrice.SetText(BunPriceNumber.ToString());
        ShortBreadPrice.SetText(ShortBreadPriceNumber.ToString());
        CookiePrice.SetText(CookiePriceNumber.ToString());
        GingerBreadPrice.SetText(GingerBreadPriceNumber.ToString());

        ShopObject.SetActive(false);
        leafAmount.SetText(Leaf.ToString());
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

    void OpenShop(){
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

    void BuyLog(){
        if(Leaf >= LogsPriceNumber && !cd && actualLogs<maxLogs){
            actualLogs++;
            StartCoroutine(cooldown());
            WriteLeaf(LogsPriceNumber);
            Instantiate(LogPrb);
        }
    }
    void BuyRareLog(){
        if(Leaf >= RareLogsPriceNumber && !cd && actualLogs<maxLogs){
            actualLogs++;
            StartCoroutine(cooldown());
            WriteLeaf(RareLogsPriceNumber);
            Instantiate(RareLogPrb);
        }
    }
    void BuyEpicLog(){
        if(Leaf >= EpicLogsPriceNumber && !cd && actualLogs<maxLogs){
            actualLogs++;
            StartCoroutine(cooldown());
            WriteLeaf(EpicLogsPriceNumber);
            Instantiate(EpicLogPrb);
        }
    }

    void BuyBerries(){
        if(Leaf >= BerriesPriceNumber && !cd && actualBerries<maxBerries){
            actualBerries++;
            StartCoroutine(cooldown());
            WriteLeaf(BerriesPriceNumber);
            Instantiate(BerriesPrb);
        }
    }
    void BuyPear(){
        if(Leaf >= PearPriceNumber && !cd && actualPear<maxPear){
            actualPear++;
            StartCoroutine(cooldown());
            WriteLeaf(PearPriceNumber);
            Instantiate(PearPrb);
        }
    }
    void BuyOrange(){
        if(Leaf >= OrangePriceNumber && !cd && actualOrange<maxOrange){
            actualOrange++;
            StartCoroutine(cooldown());
            WriteLeaf(OrangePriceNumber);
            Instantiate(OrangePrb);
        }
    }

    void BuySliceOfBread(){
        if(Leaf >= SliceOfBreadPriceNumber && !cd && actualSliceOfBread<maxSliceOfBread){
            actualSliceOfBread++;
            StartCoroutine(cooldown());
            WriteLeaf(SliceOfBreadPriceNumber);
            Instantiate(SliceOfBreadPrb);
        }
    }
    void BuyRoundBread(){
        if(Leaf >= RoundBreadPriceNumber && !cd && actualRoundBread<maxRoundBread){
            actualRoundBread++;
            StartCoroutine(cooldown());
            WriteLeaf(RoundBreadPriceNumber);
            Instantiate(RoundBreadPrb);
        }
    }
    void BuyBun(){
        if(Leaf >= BunPriceNumber && !cd && actualBun<maxBun){
            actualBun++;
            StartCoroutine(cooldown());
            WriteLeaf(BunPriceNumber);
            Instantiate(BunPrb);
        }
    }

    void BuyShortBread(){
        if(Leaf >= ShortBreadPriceNumber && !cd && actualShortBread<maxShortBread){
            actualShortBread++;
            StartCoroutine(cooldown());
            WriteLeaf(ShortBreadPriceNumber);
            Instantiate(ShortBreadPrb);
        }
    }
    void BuyCookie(){
        if(Leaf >= CookiePriceNumber && !cd && actualCookie<maxCookie){
            actualCookie++;
            StartCoroutine(cooldown());
            WriteLeaf(CookiePriceNumber);
            Instantiate(CookiePrb);
        }
    }
    void BuyGingerBread(){
        if(Leaf >= GingerBreadPriceNumber && !cd && actualGingerBread<maxGingerBread){
            actualGingerBread++;
            StartCoroutine(cooldown());
            WriteLeaf(GingerBreadPriceNumber);
            Instantiate(GingerBreadPrb);
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
