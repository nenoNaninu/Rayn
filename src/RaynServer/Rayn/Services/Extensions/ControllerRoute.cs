using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Rayn.Services.Extensions;

public static class ControllerRoute
{
    private static class Cache<T>
    {
        private const string ControllerTemplateString = "[controller]";

        public static readonly string ControllerRouteValue;

        private static string RemoveControllerStringFromTypeName(Type type)
        {
            var name = type.Name;
            var index = name.IndexOf("Controller", StringComparison.Ordinal);
            return name[..index];
        }

        static Cache()
        {
            var type = typeof(T);

            var routeAttribute = type
                .GetCustomAttributes(true)
                .Select(x => x as RouteAttribute)
                .FirstOrDefault(x => x is not null);

            if (routeAttribute is not null)
            {
                var template = routeAttribute.Template;

                if (template is null)
                {
                    ControllerRouteValue = RemoveControllerStringFromTypeName(type);
                    return;
                }

                var index = template.IndexOf(ControllerTemplateString, StringComparison.Ordinal);

                if (index < 0)
                {
                    ControllerRouteValue = template;
                    return;
                }

                var controllerName = RemoveControllerStringFromTypeName(type);

                var prefix = template[..index];
                var suffix = template[(index + ControllerTemplateString.Length)..];

                ControllerRouteValue = $"{prefix}{controllerName}{suffix}";
            }

            ControllerRouteValue = RemoveControllerStringFromTypeName(type);
        }
    }

    public static string Value<T>() where T : ControllerBase
    {
        return Cache<T>.ControllerRouteValue;
    }
}
