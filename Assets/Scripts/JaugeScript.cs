using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaugeScript : MonoBehaviour
{
    [Header("Jauge bars")]
    Vector3 localScale;
    float maxBarAmount;
    [HideInInspector] public float barAmount;
    [SerializeField] bool Health;
    [HideInInspector] public LogManager logManager;
    ShopManager shopManager;
    [HideInInspector] public float multiplier;

    void Start(){
        maxBarAmount = transform.GetChild(0).transform.localScale.x;
        barAmount = 0;
        localScale = transform.GetChild(0).transform.localScale;
        if(!Health){
            ChangeJauge();
        }else{
            barAmount = maxBarAmount;
        }
        shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
    }

    public void ChangeJauge(){
        localScale.x = barAmount;
        transform.GetChild(0).transform.localScale = localScale;
    }

    public void UseObject(int howMuchToAdd, GameObject whatObject, string whatType){
        if(!Health){
            if(barAmount<maxBarAmount){
                barAmount+= maxBarAmount*((howMuchToAdd/multiplier)*0.01f);
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
                if(barAmount>=maxBarAmount){
                    barAmount = maxBarAmount;
                    if(whatType=="Berries" || whatType=="Pear" || whatType=="Orange"){
                        logManager.fruitAmount = 100;
                    }else if(whatType=="Shortbread" || whatType=="Cookie" || whatType=="Gingerbread"){
                        logManager.biscuitAmount = 100;
                    }else if(whatType=="BreadSlice" || whatType=="RoundBread" || whatType=="Bun"){
                        logManager.breadAmount = 100;
                    }
                    logManager.checkUpJauges();
                }
                ChangeJauge();
                if(whatType=="Berries"){
                    shopManager.actualBerries--;
                }else if(whatType=="Pear"){
                    shopManager.actualPear--;
                }else if(whatType=="Orange"){
                    shopManager.actualOrange--;
                }else if(whatType=="Shortbread"){
                    shopManager.actualShortBread--;
                }else if(whatType=="Cookie"){
                    shopManager.actualCookie--;
                }else if(whatType=="GingerBread"){
                    shopManager.actualGingerBread--;
                }else if(whatType=="BreadSlice"){
                    shopManager.actualSliceOfBread--;
                }else if(whatType=="RoundBread"){
                    shopManager.actualRoundBread--;
                }else if(whatType=="Bun"){
                    shopManager.actualBun--;
                }
                Destroy(whatObject);
            }
        }else{
            barAmount-=maxBarAmount*(howMuchToAdd*0.01f);
            logManager.healthAmount = barAmount;
            if(barAmount<=0){
                barAmount=0;
            }
            ChangeJauge();
            logManager.LogHealthCheckUp();
        }
    }
}
