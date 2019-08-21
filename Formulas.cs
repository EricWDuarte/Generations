using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formulas 
{
    public static int TruePayment (int basePayment, int IQ)
    {
        int payment = basePayment * IQ * IQ / 540;
        if (payment > 1300)
        {
            return payment;
        }
        else
        {
            return 1300;
        }
    }
}
