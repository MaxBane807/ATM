using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Web.Models;

namespace ATM.Web.Services.Interfaces
{
    interface ICalculator
    {
        public List<Bill> UpdateBills(List<Bill> BillsWithdrawn);
        public bool GetIfValueExists(List<Bill> Bills, int inputValue);
        public bool GetIfAtmISEmpty(List<Bill> Bills);
        public List<Bill> CalculateBillsToWithdraw(List<Bill> Bills, int inputValue);

        public List<Bill> UpdateWithdrawnBills(List<Bill> billsToWithdraw, List<Bill> billsAlreadyWithdrawn);
    }
}
