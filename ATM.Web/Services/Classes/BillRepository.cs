using ATM.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Web.Models;
using ATM.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace ATM.Web.Services.Classes
{
    public class BillRepository : IBillRepository
    {
        private readonly AtmContext _context;
        public BillRepository(AtmContext context)
        {
            _context = context;
        }
        public List<Bill> GetBillsInAtm()
        {
            var bills = _context.Bills.AsNoTracking().ToList();
            return bills;
        }

        public List<Bill> GetGivenOutBills()
        {
            var initialBills = new List<Bill>()
            {
                new Bill {Value = 1000, Amount = 2},
                new Bill {Value = 500, Amount = 3},
                new Bill {Value = 100, Amount = 5}
            };
            
            return initialBills.Select(x => new Bill
            {
                Amount = (x.Amount -= _context.Bills.FirstOrDefault(y => y.Value == x.Value).Amount),
                Value = x.Value
            }).ToList();
        }

        public void WithdrawBills(List<Bill> resultbills, List<Bill> billsInAtm)
        {
            var correctBillValues = billsInAtm.Select(x => new Bill
            {
                Amount = x.Amount -= resultbills.FirstOrDefault(y => x.Value == y.Value).Amount,
                Value = x.Value
            });

            var allbills = _context.Bills;
            
            _context.Bills.RemoveRange(allbills);

            _context.Bills.AddRange(correctBillValues);

            _context.SaveChanges();
        }
    }
}
