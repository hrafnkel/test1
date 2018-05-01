using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WTW.Interface;
using WTW.Model;
using WTW.Repository;
using WTW.Utility;

namespace WTW.Tests.UtilityTests
{
    [TestFixture]
    public class When_calculating
    {
        private const string Filename = @"C:\Workspaces\WTW\WTW.Tests\Data\Example.csv";

        private PaymentRepository _paymentRepository;
        private IMapper _mapper;
        private ICalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mapper();
            _calculator = new Calculator();
        }

        [Test]
        public void the_origin_and_development_years_are_as_expected()
        {
            _paymentRepository = new PaymentRepository(_mapper, Filename);
            int[] result = _calculator.OriginAndDevelopmentYears(_paymentRepository.PersistentPayments);

            Assert.That(result[0], Is.EqualTo(1990));
            Assert.That(result[1], Is.EqualTo(4));
        }

        [TestCase("Comp")]
        [TestCase("Non-Comp")]
        public void the_triangle_first_item_is_the_product_name(string productName)
        {
            _paymentRepository = new PaymentRepository(_mapper, Filename);
            IEnumerable<IncrementalPayment> filteredPayments 
                = _paymentRepository.PersistentPayments.Where(x => x.Product == productName);
            const int startYear = 1990;
            const int developmentYears = 4;

            string result = _calculator.GetTriangleLine(filteredPayments, startYear, developmentYears);

            Assert.That(result.StartsWith(productName));
        }

        [TestCase("Comp","Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200")]
        [TestCase("Non-Comp","Non-Comp, 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100")]
        public void the_triangle_items_are_the_payment_totals(string productName, string expectedLine)
        {
            _paymentRepository = new PaymentRepository(_mapper, Filename);
            IEnumerable<IncrementalPayment> filteredPayments
                = _paymentRepository.PersistentPayments.Where(x => x.Product == productName);
            const int startYear = 1990;
            const int developmentYears = 4;

            string result = _calculator.GetTriangleLine(filteredPayments, startYear, developmentYears);

            Assert.That(result,Is.EqualTo(expectedLine));
        }

        [TestCase("Comp", "Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200")]
        [TestCase("Non-Comp", "Non-Comp, 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100")]
        public void the_triangle_items_are_the_payment_totals_even_from_a_disorganised_source(string productName, string expectedLine)
        {
            const string jumbled = @"C:\Workspaces\WTW\WTW.Tests\Data\JumbledUp.csv";
            var localRepository = new PaymentRepository(_mapper, jumbled);

            IEnumerable<IncrementalPayment> filteredPayments
                = localRepository.PersistentPayments.Where(x => x.Product == productName);
            const int startYear = 1990;
            const int developmentYears = 4;

            string result = _calculator.GetTriangleLine(filteredPayments, startYear, developmentYears);

            Assert.That(result, Is.EqualTo(expectedLine));
        }

        [TestCase("Comp", "Comp, 0, 0, 0, 0, 0, 0, 0, 0, 0, 110, 280, 280, 200, 200, 0")]
        [TestCase("Non-Comp", "Non-Comp, 45.2, 110, 110, 147, 147, 50, 125, 150, 150, 55, 140, 140, 100, 100, 999")]
        public void the_triangle_items_are_the_payment_totals_even_when_many_years(string productName, string expectedLine)
        {
            const string jumbled = @"C:\Workspaces\WTW\WTW.Tests\Data\MoreYears.csv";
            var localRepository = new PaymentRepository(_mapper, jumbled);

            IEnumerable<IncrementalPayment> filteredPayments
                = localRepository.PersistentPayments.Where(x => x.Product == productName);
            const int startYear = 1990;
            const int developmentYears = 5;

            string result = _calculator.GetTriangleLine(filteredPayments, startYear, developmentYears);

            Assert.That(result, Is.EqualTo(expectedLine));
        }

        [TestCase("Comp", "Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200")]
        [TestCase("Non-Comp", "Non-Comp, 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100")]
        [TestCase("Z-Comp", "Z-Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200")]
        public void the_triangle_items_are_the_payment_totals_even_when_many_triangles(string productName, string expectedLine)
        {
            const string moretriangles = @"C:\Workspaces\WTW\WTW.Tests\Data\MoreTriangles.csv";
            var localRepository2 = new PaymentRepository(_mapper, moretriangles);

            IEnumerable<IncrementalPayment> filteredPayments
                = localRepository2.PersistentPayments.Where(x => x.Product == productName);
            const int startYear = 1990;
            const int developmentYears = 4;

            string result = _calculator.GetTriangleLine(filteredPayments, startYear, developmentYears);

            Assert.That(result, Is.EqualTo(expectedLine));
        }
    }
}
