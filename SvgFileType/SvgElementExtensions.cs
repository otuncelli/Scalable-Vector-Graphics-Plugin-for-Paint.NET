using Svg;
using System.Reflection;

namespace SvgFileTypePlugin
{
    internal static class SvgElementExtensions
    {
        private static readonly MethodInfo ElementNameGetter =
            typeof(SvgElement).GetProperty("ElementName",
                BindingFlags.NonPublic | BindingFlags.Instance)?.GetGetMethod(true);

        public static string GetName(this SvgElement element)
        {
            return element.GetType().GetCustomAttribute<SvgElementAttribute>()?.ElementName ??
                ElementNameGetter?.Invoke(element, null) as string ??
                element.GetType().Name;
        }
    }
}
