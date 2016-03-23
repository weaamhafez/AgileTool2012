using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace EngineerWeb.Project
{
    static class Utils
    {
        public static T ToObject<T>(IDictionary<string, object> source)
        where T : class, new()
        {
            T someObject = new T();
            Type someObjectType = someObject.GetType();

            foreach (KeyValuePair<string, object> item in source)
            {
                try
                {
                    var value = Convert.ChangeType(item.Value, Type.GetType(someObjectType.GetProperty(item.Key).PropertyType.FullName));
                    someObjectType.GetProperty(item.Key).SetValue(someObject, value, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return someObject;
        }

        public static string GetUserID(IIdentity identity)
        {
            return IdentityExtensions.GetUserId(identity);
        }

        public static object SerializeObject(object entity)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var sprintDynamic = JsonConvert.SerializeObject(entity, Formatting.Indented, jss);
            return new JavaScriptSerializer().Deserialize(sprintDynamic, typeof(object));
        }

    }
}