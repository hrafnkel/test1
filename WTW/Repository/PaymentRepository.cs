using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WTW.Interface;
using WTW.Model;

namespace WTW.Repository
{
    public class PaymentRepository
    {
        private readonly IMapper _mapper;
        private readonly StringComparison _comparison;
        public IEnumerable<IncrementalPayment> PersistentPayments { get;  }

        public PaymentRepository(IMapper mapper, string filename)
        {
            _mapper = mapper;
            _comparison = StringComparison.InvariantCultureIgnoreCase;
            PersistentPayments = GetAllFromFileExceptHeader(filename);
        }

        public Dictionary<string, List<IncrementalPayment>> GetPaymentsDictionary()
        {
            List<string> products = PersistentPayments.Select(x => x.Product).Distinct().ToList();
            var dictionary = new Dictionary<string, List<IncrementalPayment>>();

            foreach (string product in products)
            {
                List<IncrementalPayment> productPayments = PersistentPayments.Where(x => x.Product == product).ToList();
                dictionary.Add(product, productPayments);
            }

            return dictionary;
        }

        public void WriteResultToFile(int[] metadata, List<string> paymentLines)
        {
            using (var file = new StreamWriter(@"C:\Temp\TriangleOutput.txt"))
            {
                file.WriteLine($"{metadata[0]}, {metadata[1]}");

                foreach (string line in paymentLines)
                {
                    file.WriteLine(line);
                }
            }
        }

        public void WriteResultToScreen(int[] metadata, List<string> paymentLines)
        {
            
                Console.WriteLine($"{metadata[0]}, {metadata[1]}");

                foreach (string line in paymentLines)
                {
                    Console.WriteLine(line);
                }

            Console.WriteLine(string.Empty);
            Console.Write("Press any key to continue : ");
            Console.ReadKey();
        }

        private IEnumerable<IncrementalPayment> GetAllFromFileExceptHeader(string filename)
        {
            List<string> lines = File.ReadLines(filename).Where(x => !x.Any(y=>x.StartsWith("Product", _comparison))).ToList();
            return _mapper.MapLinesToModel(lines);
        }
    }
}
