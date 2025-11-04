using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.Administration.Core
{
    public interface IEmployeeService
    {
        Task<bool> IncrementAppliedPromocodesAsync(Guid employeeId, CancellationToken cancellationToken = default);
    }
}
