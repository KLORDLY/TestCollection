using System;
using System.Collections.Generic;
using System.Text;

namespace ComToolOld
{
    class GetResources
    {
        public class iResource
        {
            static public string GetString(string name)
            {
                System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;
                return Properties.Resources.ResourceManager.GetString(name, cultureInfo);
            }
            static public string GetString(string name, System.Globalization.CultureInfo culture)
            {
                return Properties.Resources.ResourceManager.GetString(name, culture);
            }
        }
    }
}
