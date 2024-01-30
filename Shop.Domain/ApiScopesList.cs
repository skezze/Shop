using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain
{
    public static class ApiScopesList
    {
        public static string ShopApi { get; } = "Shop.Api";
        public static string ShopConsoleClient { get; } = "Shop.ConsoleClient";
    }
}
