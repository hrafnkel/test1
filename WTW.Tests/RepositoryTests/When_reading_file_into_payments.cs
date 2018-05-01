using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WTW.Interface;
using WTW.Model;
using WTW.Repository;
using WTW.Utility;

namespace WTW.Tests.RepositoryTests
{
    [TestFixture]
    public class When_reading_file_into_payments
    {
        private const string Filename = @"C:\Workspaces\WTW\WTW.Tests\Data\Example.csv";

        private PaymentRepository _paymentRepository;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mapper();
            _paymentRepository = new PaymentRepository(_mapper, Filename);
        }

        [Test]
        public void it_is_read_into_domain_object()
        {
            IEnumerable<IncrementalPayment> payments = _paymentRepository.PersistentPayments;
            int count = payments.Count();
            const int expectedCount = 12;
            Assert.That(count, Is.EqualTo(expectedCount));
        }

        [TestCase("Comp", 3)]
        [TestCase("Non-Comp", 9)]
        [TestCase("Bad-Value", 0)]
        public void it_can_be_filtered_by_product(string filter, int expectedCount)
        {
            IEnumerable<IncrementalPayment> payments = _paymentRepository.PersistentPayments.Where(x=>x.Product == filter);
            int count = payments.Count();

            Assert.That(count, Is.EqualTo(expectedCount));
        }

        [Test]
        public void the_dictionary_is_returned()
        {
            Dictionary<string, List<IncrementalPayment>> dictionary = _paymentRepository.GetPaymentsDictionary();

            Assert.That(dictionary.Count, Is.GreaterThan(0));
        }
    }
}
