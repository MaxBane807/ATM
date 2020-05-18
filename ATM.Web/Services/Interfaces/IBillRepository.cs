using ATM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Services.Interfaces
{
    public interface IBillRepository
    {
        List<Bill> GetBillsInAtm();

        List<Bill> GetGivenOutBills();

        void WithdrawBills(List<Bill> resultbills, List<Bill> billsInAtm);
    }
}
