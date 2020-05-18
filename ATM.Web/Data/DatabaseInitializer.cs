using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Data
{
    public class DatabaseInitializer
    {
        public static void Initialize(AtmContext context)
        {
            context.Database.Migrate();
            SeedData(context);
        }
        private static void SeedData(AtmContext context)
        {

            var allbills = context.Bills;
            if (allbills.Count() != 0)
            {
                context.Bills.RemoveRange(allbills);
                context.SaveChanges();
            }
            
            if (!context.Bills.Any(x => x.Value == 1000))
            {
                
                context.Bills.Add(new Models.Bill
                {
                    Value = 1000,
                    Amount = 2
                });
                
            }
            if (!context.Bills.Any(x => x.Value == 500))
            {
                context.Bills.Add(new Models.Bill
                {
                    Value = 500,
                    Amount = 3
                });
               
            }
            if (!context.Bills.Any(x => x.Value == 100))
            {
                context.Bills.Add(new Models.Bill
                {
                    Value = 100,
                    Amount = 5
                });
                
            }
            context.SaveChanges();
        }
    }
}
