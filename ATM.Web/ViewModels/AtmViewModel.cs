using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.Web.Models;

namespace ATM.Web.ViewModels
{
    public class AtmViewModel
    {
        public int ValueRequested { get; set; }

        public List<Bill> BillsGivenOut = new List<Bill>();
    }
}
