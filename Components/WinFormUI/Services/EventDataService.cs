using Slipstream.Components.WinFormUI.Models;
using Slipstream.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.Components.WinFormUI.Services
{
    public static class EventDataService
    {
        public static async Task<IEnumerable<EventInfoModel>> FindEvents(Func<EventInfoModel, bool> filter)
        {
            return await Task.Run(() =>
            {
                var type = typeof(IEvent);
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                    .Select(t => BuildEventInfo(t))
                    .Where(p => filter(p))
                    .OrderBy(p => p.Name)
                    .ToList();
            });
        }

        private static EventInfoModel BuildEventInfo(Type t)
        {
            return new EventInfoModel
            {
                Name = t.Name,
                Properties = BuildEventProperties(t),
                EventType = t
            };
        }

        private static string GetDescriptionFromAttribute(PropertyInfo p)
        {
            return Attribute.IsDefined(p, typeof(DescriptionAttribute)) ?
                    (Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)) as DescriptionAttribute).Description : null;
        }

        private static IList<EventPropertyInfoModel> BuildEventProperties(Type t)
        {
            var exclusionList = new List<string>
            {
               "EventType",
               "ExcludeFromTxrx"
            };

            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return props
                .Where(p => !exclusionList.Any(exc => exc == p.Name))
                .Select(p => new EventPropertyInfoModel 
            { 
                Name = p.Name,
                IsComplex = p.PropertyType.Namespace.StartsWith("Slipstream"),
                Type = p.PropertyType,
                Description = GetDescriptionFromAttribute(p)
            }).ToList();
        }
    }
}