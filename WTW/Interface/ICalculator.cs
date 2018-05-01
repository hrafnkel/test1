using System.Collections.Generic;
using WTW.Model;

namespace WTW.Interface
{
    public interface ICalculator
    {
        int[] OriginAndDevelopmentYears(IEnumerable<IncrementalPayment> payments);
        string GetTriangleLine(IEnumerable<IncrementalPayment> filteredPayments,int startYear, int developmentYears);

    }
}
