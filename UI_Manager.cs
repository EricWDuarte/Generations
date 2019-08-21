using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;


    public GameObject FellInLoveMessage;
    public GameObject HaveBabyMessage;
    public GameObject AdoptBabyMessage;
    public GameObject BecomeChildMessage;
    public GameObject ParentsDiedMessage;



    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FellInLove()
    {
        StartCoroutine(ShowMessage(FellInLoveMessage));
    }
        public void HaveBaby()
    {
        StartCoroutine(ShowMessage(HaveBabyMessage));
    }
        public void BecomeChild()
    {
        StartCoroutine(ShowMessage(BecomeChildMessage));
    }
        public void AdoptBaby()
    {
        StartCoroutine(ShowMessage(AdoptBabyMessage));
    }
        public void ParentsDied()
    {
        StartCoroutine(ShowMessage(ParentsDiedMessage));
    }

    IEnumerator ShowMessage(GameObject Message)
    {
        Message.SetActive(true);
        yield return new WaitForSeconds(3f);
        Message.SetActive(false);
    }
}
