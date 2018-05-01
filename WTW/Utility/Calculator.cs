using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WTW.Interface;
using WTW.Model;

namespace WTW.Utility
{
    public class Calculator : ICalculator
    {
        public int[] OriginAndDevelopmentYears(IEnumerable<IncrementalPayment> payments)
        {
            int minOriginYear = payments.Select(x => x.OriginYear).Min(x => x);
            int maxOriginYear = payments.Select(x => x.OriginYear).Max(x => x);

            int[] data = new[] { minOriginYear, maxOriginYear - minOriginYear + 1 };

            return data;
        }

        public string GetTriangleLine(IEnumerable<IncrementalPayment> filteredPayments,
            int startYear,
            int developmentYears)
        {
            string name = GetProductNameFromPayments(filteredPayments);
            string paymentTotals = GetPaymentTotalsFromPayments(filteredPayments, startYear, developmentYears);
            return $"{name}, {paymentTotals}";
        }

        private string GetProductNameFromPayments(IEnumerable<IncrementalPayment> filteredPayments)
        {
            IncrementalPayment firstPayment = filteredPayments.First();

            return firstPayment.Product;
        }

        private string GetPaymentTotalsFromPayments(IEnumerable<IncrementalPayment> filteredPayments, int startYear, int developmentYears)
        {
            string line = string.Empty;

            for (int originYear = startYear; originYear < startYear + developmentYears; originYear++)
            {
                int year = originYear;
                List<IncrementalPayment> thisYearsPayments = filteredPayments.Where(x => x.OriginYear == year).ToList();
                int range = developmentYears - (originYear - startYear);
                
                for (int i = 0; i < range; i++)
                {
                    int devYear = originYear + i;
                    string sum = thisYearsPayments.Any() ? SumUp(thisYearsPayments, devYear) : "0, ";
                    line = line + sum; 
                    }
            }

            char[] trimChars = {',' , ' '};
            return line.Trim(trimChars);
        }

        private string SumUp(List<IncrementalPayment> thisYearsPayments,  int devYear)
        {
            return 
                thisYearsPayments.Where(x => x.DevelopmentYear <= devYear).Sum(x => x.IncrementalValue).ToString("0.##",CultureInfo.InvariantCulture) + ", "; 

        }
    }
}
