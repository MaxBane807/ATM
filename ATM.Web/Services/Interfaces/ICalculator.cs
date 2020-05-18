using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Web.Models;

namespace ATM.Web.Services.Interfaces
{
    public interface ICalculator
    {
        public bool GetIfValueExists(List<Bill> Bills, int inputValue);
        public bool GetIfAtmISEmpty(List<Bill> Bills);
        public List<Bill> CalculateBillsToWithdraw(List<Bill> Bills, int inputValue);
    }
}
