using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace POSIntegratorV2
{
    public class SysGlobal
    {
        public static String ConnectionStringConfig()
        {
            String ConnectionString = "";
            String settingspath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"System.json");

            String json;
            using (StreamReader trmRead = new StreamReader(settingspath))
            {
                json = trmRead.ReadToEnd();
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Entities.System s = js.Deserialize<Entities.System>(json);

            ConnectionString = s.LocalConnection;

            return ConnectionString;
        }
    }
}
