using System.Globalization;
using Scar.Common.Localization;

namespace Scar.Common.WebApi.Startup
{
    class ConsoleCultureManager : ICultureManager
    {
        public void ChangeCulture(CultureInfo cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }
}
