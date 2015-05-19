using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data
{
    public class EntityHelper<T>
    {
        public static void SetValue(T target, Dictionary<string, object> dic)
        {
            PropertyInfo[] proArry = typeof(T).GetProperties();
            Type t = typeof(T); 
            foreach (PropertyInfo p in proArry)
            {
                foreach (var key in dic.Keys)
                {
                    if (key == p.Name)
                    {
                        string name = p.Name;
                        t.GetProperty(name).SetValue(target, dic[key], null);
                        break;
                    }
                }
            }
        }
    }
}
