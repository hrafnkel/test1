using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WTW.Interface;
using WTW.Model;

namespace WTW.Utility
{
    public class Mapper : IMapper
    {
        public IEnumerable<IncrementalPayment> MapLinesToModel(List<string> lines)
        {
            List<string> productNames = GetProductNames(lines);

            var payments = new List<IncrementalPayment>();
            foreach (string line in lines)
            {
                string product = line.Split(',').FirstOrDefault();
                if (productNames.Contains(product))
                {
                    List<decimal> values = GetNumbers(line);
                    var payment = new IncrementalPayment
                    {
                        Product = product,
                        OriginYear = (int)values[0],
                        DevelopmentYear = (int)values[1],
                        IncrementalValue = values[2]
                    };
                    payments.Add(payment);
                }
            }
            return payments;
        }

        public List<decimal> GetNumbers(string input)
        {
            const string pattern = @"[^0-9\.]+";

            List<decimal> values = Regex.Split(input, pattern)
                .Where(c => c != "." && c.Trim() != "")
                .Select(decimal.Parse)
                .ToList();

            if (values.Count == 2)
            {
                values.Add(0);
            }

            return values;
        }

        public List<string> GetProductNames(IEnumerable<string> lines)
        {
            var zzz = lines.Select(x => x.Split(',').First()).Distinct();

            return zzz.ToList();
        }
    }
}
