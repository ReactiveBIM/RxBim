using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public abstract record Enumeration(string Name, string Version)
{
    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
}