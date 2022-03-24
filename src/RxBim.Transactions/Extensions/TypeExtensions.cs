namespace RxBim.Transactions.Extensions
{
    using System;
    using System.Linq;
    using Attributes;
    using Castle.Core.Internal;
    using Castle.DynamicProxy.Internal;

    public static class TypeExtensions
    {
        public static void CheckType(this Type type)
        {
            var methods = type.GetMethods().Where(x => x.GetAttribute<TransactionalAttribute>() != null).ToList();
            if (methods.Any())
            {
                var allInterfaces = type.GetAllInterfaces();
                if (allInterfaces.Any())
                {
                    // allInterfaces
                    //     .SelectMany(x => x.GetMethods())
                    //     .Select(x => )
                }
                else
                {
                    if (methods.Any(x => !x.IsVirtual))
                    {
                        throw new Exception();
                    }
                }
            }
        }
    }
}