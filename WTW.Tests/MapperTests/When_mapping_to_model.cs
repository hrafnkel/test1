using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WTW.Interface;
using WTW.Model;
using WTW.Utility;

namespace WTW.Tests.MapperTests
{[TestFixture]
    public class When_mapping_to_model
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mapper();
        }


        [Test]
        public void A_well_formed_line_is_parsed()
        {
            const string data = "Non-Comp, 1990, 1990, 45.2";
            var lines = new List<string> {data};
            IEnumerable<IncrementalPayment> results = _mapper.MapLinesToModel(lines);
            IncrementalPayment result = results.FirstOrDefault();
            Assert.That(result.Product, Is.EqualTo("Non-Comp"));
            Assert.That(result.OriginYear, Is.EqualTo(1990));
            Assert.That(result.DevelopmentYear, Is.EqualTo(1990));
            Assert.That(result.IncrementalValue, Is.EqualTo(45.2m));
        }


        [Test]
        public void A_messed_line_is_parsed()
        {
            const string data = "Non-Comp, 1990*1990xxxxxxxxxxxxxxxxxxx45.2yyyyyyyyyyy";
            var lines = new List<string> { data };
            IEnumerable<IncrementalPayment> results = _mapper.MapLinesToModel(lines);
            IncrementalPayment result = results.FirstOrDefault();
            Assert.That(result.Product, Is.EqualTo("Non-Comp"));
            Assert.That(result.OriginYear, Is.EqualTo(1990));
            Assert.That(result.DevelopmentYear, Is.EqualTo(1990));
            Assert.That(result.IncrementalValue, Is.EqualTo(45.2m));
        }

    }
}
