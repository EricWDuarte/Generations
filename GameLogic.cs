using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    // STARTING VAR //

    private int STARTING_AGE = 192;
    private int BASE_PAYMENT = 100;
    private string SINGLE_STRING = "Single";
    private string MARIED_STRING = "Maried";
    private string PARENT_STRING = "Parent";

    // CHANCES //

    private float BABY_CHANCE = 0.031f;

    // VARIANTIONS // 

    public float moneyVar = 0.3f;
    public float IQVar = 0.1f;

    // CONSTANTS //

    private int singleCost = 500;
    private int mariedCost = 1000;
    private int parentCost = 2500;
    
    // TEXTS //
    
    public Text age_TEXT;
    public Text money_TEXT;
    public Text iq_TEXT;
    public Text status_TEXT;

    public Text workPayment_TEXT;
    public Text studyCost_TEXT;

    // PLAYER CHART //

    public int generation = 0;
    public int age;
    public int money = 200;
    public int iq = 90;
    private int extraIQ = 0;
    public string status;

    // PARTNER CHART //

    public int partnerMoney;
    public int partnerIQ;

    // BABY //

    public int babyAge = 0;
    public int babyIQ;
    private int babyExtraIQ = 0;

    // PARENT CHART //

    public int parentAge;
    public int parentMoney;

    // COSTS //

    public int livingCost;
    public int workPayment;
    public int[] studyCosts = {1000, 2500, 4500, 7000, 11000, 20000, 50000, 100000, 700000, 1500000};
    public int studyCostIndex = 1;

    // ACHIEVEMENTS // 

    public bool entrepenour = false;
    public bool bankrupt = false;
    public bool genius = false;
    public bool dumb = false;
    public bool millionaire = false;

    private int entrepenourGen;
    private int bankruptGen;
    private int geniusGen;
    private int dumbGen;
    private int millionaireGen;

    public GameObject Bankrupt;
    public GameObject Entrepenour;
    public GameObject Genius;
    public GameObject Dumb;
    public GameObject Bankruptttt;
    public GameObject Millionaire;

    // MISC //

    private int turnsWorking = 0;
    private int speed = 1;
    private bool parentsDed = false;

    private UI_Manager UI;

    void Start()
    {
        livingCost = singleCost;
        age = STARTING_AGE;
        workPayment = BASE_PAYMENT;
        status = SINGLE_STRING;

        UI = gameObject.GetComponent<UI_Manager>();

        UpdateText();
    }


    public void Work()
    {
        money += Formulas.TruePayment(BASE_PAYMENT, iq + extraIQ);

        age += 3 * speed;

        if (status == PARENT_STRING)
        {
            babyAge += 3 * speed;
        }

        if (generation > 0)
        {
            parentAge += 3 * speed;
        }

        if (entrepenour)
        {
            if (Random.value < 0.4f)
            {
                iq += 1;
            }
        }
        else if (turnsWorking > 3)
        {
            iq -= 1;
            if (iq < 80)
            {
                iq = 80;
            }
        }
        


        turnsWorking += 1;
        EndTurn();
    }

    public void Study()
    {
        if (money >= studyCosts[studyCostIndex])
        {
            iq += dumb ? 2 : 4;
            money -= studyCosts[studyCostIndex];

            age += 6 * speed;

            if (status == PARENT_STRING)
            {
                babyAge += 6 * speed;
            }

            if (generation > 0)
            {
                parentAge += 6 * speed;
            }

            if (genius)
            {
                money += Formulas.TruePayment(BASE_PAYMENT, iq + extraIQ);
            }

            turnsWorking = 0;
            EndTurn();
        }
    }




    public void EndTurn()
    {
        money -= livingCost + (bankrupt ? 200 : 0);

        if (age > 276 && status == SINGLE_STRING) {

            SearchLove();

        }
        else if (age > 540 && status == MARIED_STRING)
        {

            AdoptBaby();
        }
        else if (status == MARIED_STRING) {

            if (Random.value < BABY_CHANCE * speed)
            {
                RollForBaby();
            }
        } 
        else if (status == PARENT_STRING) {
            if (babyAge >= 192)
            {
                BecomeBaby();
            }
        }

        if (generation > 0 && parentsDed == false)
        {
            CheckParentsDied();
        }

        studyCostIndex = (iq - 80) / 10;
        speed = Mathf.FloorToInt((age - 192) / 120f) + 1;

        CheckAchievements();
        UpdateText();
    }

    public void SearchLove()
    {
        if (Random.value < (age - 275f)/85f)
        {
            status = MARIED_STRING;
            livingCost = mariedCost;
            UI.FellInLove();

            partnerIQ = Mathf.RoundToInt(Random.Range(1f - IQVar, 1f + IQVar) * iq);
            partnerMoney = Mathf.RoundToInt(Random.Range(1f - moneyVar, 1f + moneyVar) * money);
        }
    }


    public void RollForBaby()
    {
        status = PARENT_STRING;
        livingCost = parentCost;
        UI.HaveBaby();

        babyAge = 0;
        babyIQ = 90;
        babyExtraIQ = Mathf.RoundToInt((iq - 90) * Random.Range(0.1f, 0.25f));
    }

    public void AdoptBaby()
    {
        status = PARENT_STRING;
        livingCost = parentCost;
        UI.AdoptBaby();

        babyAge = 0;
        babyExtraIQ = 0;
        babyIQ = 90;
    }

    public void BecomeBaby ()
    {

        if (parentsDed == false)
        {
            parentMoney += money;
        } else
        {
            parentMoney = money;
        }

        parentsDed = false;

        parentAge = age;

        age = STARTING_AGE;
        iq = babyIQ;
        if (parentMoney > 0)
        {
            money = Mathf.RoundToInt(parentMoney * 0.05f);
            parentMoney -= money;
        }
        else
        {
            money = 0;
        }

        status = SINGLE_STRING;
        livingCost = singleCost;
        UI.BecomeChild();

        generation += 1;
        UpdateText();
    }

    public void CheckParentsDied()
    {
        if (Random.value < (parentAge - 600f) / 600f)
        {

            ParentsDied();
        }
    }

    public void ParentsDied()
    {

        UI.ParentsDied();
        if (parentMoney > 0)
        {
            money += parentMoney / 2;
        }
        parentsDed = true;

    }

    public void CheckAchievements()
    {

        if (money < 0)
        {
            bankrupt = true;
            bankruptGen = generation;
        }

        if (bankruptGen < generation && status == MARIED_STRING)
        {
            bankrupt = false;
        }

        if (turnsWorking > 30)
        {
            entrepenour = true;
            entrepenourGen = generation;
        }

        if (entrepenourGen < generation && status == PARENT_STRING)
        {
            entrepenour = false;
        }

        if (iq < 80)
        {
            dumb = true;
            dumbGen = generation;
        }

        if (dumbGen < generation)
        {
            dumb = false;
        }

        if (iq + extraIQ > 130) {
            genius = true;
            geniusGen = generation;
        }

        if (geniusGen + 1 < generation)
        {
            genius = false;
        }

        if (money > 200000)
        {
            millionaire = true;
        }



        Bankrupt.SetActive(bankrupt);
        Entrepenour.SetActive(entrepenour);
        Genius.SetActive(genius);
        Dumb.SetActive(dumb);
        Millionaire.SetActive(millionaire);
    }

    public void UpdateText()
    {
        age_TEXT.text = Mathf.Floor(age/12f) + "y " + age%12 + "m";
        money_TEXT.text = money.ToString() + "   - " + (livingCost + (bankrupt ? 200 : 0));
        iq_TEXT.text = (iq + extraIQ).ToString();
        status_TEXT.text = status;

        workPayment_TEXT.text = "+ " + Formulas.TruePayment(BASE_PAYMENT, iq + extraIQ).ToString();
        studyCost_TEXT.text = "- " + studyCosts[studyCostIndex].ToString() +  (genius ? " + " + Formulas.TruePayment(BASE_PAYMENT, iq + extraIQ).ToString() : "");
    }

}
