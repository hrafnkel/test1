using System.Collections.Generic;
using System.Linq;
using WTW.Interface;
using WTW.Model;
using WTW.Repository;
using WTW.Utility;

namespace WTW
{
    public class Program
    {
        private static readonly IMapper Mapper = new Mapper();
        private static readonly ICalculator Calculator = new Calculator();

        static void Main()
        {
            const string filename = @"C:\Workspaces\WTW\WTW\Data\Example.csv";
            //const string filename = @"C:\Workspaces\WTW\WTW\Data\MoreTriangles.csv";

            var paymentRepository = new PaymentRepository(Mapper, filename);

            List<IncrementalPayment> allPayments = paymentRepository.PersistentPayments.ToList();
            int[] originAndDevelopmentYears = Calculator.OriginAndDevelopmentYears(allPayments);

            Dictionary<string, List<IncrementalPayment>> dictionary = paymentRepository.GetPaymentsDictionary();
            var keyList = new List<string>(dictionary.Keys);

            List<string> paymentLines = new List<string>();
            foreach (string key in keyList)
            {
                List<IncrementalPayment> payments;
                dictionary.TryGetValue(key, out payments);
                string line = Calculator.GetTriangleLine(payments, originAndDevelopmentYears[0], originAndDevelopmentYears[1]);
                paymentLines.Add(line);
            }

            paymentRepository.WriteResultToFile(originAndDevelopmentYears, paymentLines);
            paymentRepository.WriteResultToScreen(originAndDevelopmentYears, paymentLines);
        }
    }
}
