using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scar.Common.WPF.Controls
{
    public interface IAutoCompleteDataProvider
    {
        Task<IEnumerable<object>> GetItemsAsync(string textPattern, CancellationToken cancellationToken);
    }
}
