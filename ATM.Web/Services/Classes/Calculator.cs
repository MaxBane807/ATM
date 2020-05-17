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
        private readonly IData _data;
        public Calculator(IData data)
        {
            _data = data;
        }

        public List<Bill> UpdateBills(List<Bill> BillsWithdrawn)
        {
            var data = _data.GetInitialData();

            if (BillsWithdrawn == null)
            {
                return data;
            }

            return data.Select(x => new Bill
            {
                Amount = x.Amount -= BillsWithdrawn.FirstOrDefault(y => y.Value == x.Value).Amount,
                Value = x.Value
            }).ToList();
        }


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
                CheckOneBillValue(billtype, ref inputValue, ref BillsToWithdraw);
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
        private Bill SubtractValue(ref int amountLeft, Bill concernedBill, ref List<Bill> billsToWithdraw)
        {
            amountLeft -= concernedBill.Value;
            concernedBill.Amount -= 1;
            billsToWithdraw.FirstOrDefault(x => x.Value == concernedBill.Value).Amount += 1;
            return concernedBill;
        }

        private Bill CheckOneBillValue(Bill billTypeInAtm, ref int amountleft, ref List<Bill> billsToWithdraw)
        {
            while (billTypeInAtm.Amount > 0)
            {
                if ((amountleft - billTypeInAtm.Value) >= 0)
                {
                    billTypeInAtm = SubtractValue(ref amountleft, billTypeInAtm, ref billsToWithdraw);
                }
                else
                {
                    break;
                }
            }
            return billTypeInAtm;
        }

        public List<Bill> UpdateWithdrawnBills(List<Bill> billsToWithdraw, List<Bill> billsAlreadyWithdrawn)
        {
            foreach (var bill in billsToWithdraw)
            {
                billsAlreadyWithdrawn.FirstOrDefault(x => x.Value == bill.Value).Amount += bill.Amount;
            }
            return billsAlreadyWithdrawn;
        }
    }
}
