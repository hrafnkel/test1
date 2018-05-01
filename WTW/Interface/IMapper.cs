using System.Collections.Generic;
using WTW.Model;

namespace WTW.Interface
{
    public interface IMapper
    {
        IEnumerable<IncrementalPayment> MapLinesToModel(List<string> lines);
        List<decimal> GetNumbers(string input);
    }
}
