using ATM.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Web.Models;

namespace ATM.Web.Services.Classes
{
    public class Calculator : ICalculator
    {
        public bool GetIfValueExists(List<Bill> Bills, int inputValue)
        {
            int sum = Bills.Sum(x => x.Value * x.Amount);
            if (inputValue > sum)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool GetIfAtmISEmpty(List<Bill> Bills)
        {
            if (Bills.Sum(x => x.Amount) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Bill> CalculateBillsToWithdraw(List<Bill> Bills, int inputValue)
        {
            var BillsToWithdraw = new List<Bill>() {
                new Bill { Value = 1000, Amount = 0},
                new Bill { Value = 500, Amount = 0},
                new Bill { Value = 100, Amount = 0}
            };

            foreach (var billtype in Bills)
            {
                CheckOneBillValue(billtype, ref inputValue, BillsToWithdraw);
            }

            if (inputValue > 0)
            {
                return null;
            }
            else
            {
                return BillsToWithdraw;
            }
        }
        private int SubtractValue(int amountLeft, Bill concernedBill, List<Bill> billsToWithdraw)
        {
            amountLeft -= concernedBill.Value;
            concernedBill.Amount -= 1;
            billsToWithdraw.FirstOrDefault(x => x.Value == concernedBill.Value).Amount += 1;
            return amountLeft;
        }

        private Bill CheckOneBillValue(Bill billTypeInAtm, ref int amountleft, List<Bill> billsToWithdraw)
        {
            while (billTypeInAtm.Amount > 0)
            {
                if ((amountleft - billTypeInAtm.Value) >= 0)
                {
                    amountleft = SubtractValue(amountleft, billTypeInAtm, billsToWithdraw);
                }
                else
                {
                    break;
                }
            }
            return billTypeInAtm;
        }
    }
}
