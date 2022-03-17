using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPackServer.Constants
{
    public static class PermissionModules
    {
        #region Module Names
        //public const string [Module] = "[Module]";
        public const string Users = "Users";
        public const string Roles = "Roles";
        public const string RoleClaims = "RoleClaims";
        public const string Clients = "Clients";
        public const string Contacts = "Contacts";
        public const string Providers = "Providers";
        public const string Products = "Products";
        public const string Articles = "Articles";
        public const string ArticleFamilies = "ArticleFamilies";
        public const string ArticleGroups = "ArticleGroups";
        public const string Quotes = "Quotes";
        public const string Projects = "Projects";
        public const string PurchaseOrders = "PurchaseOrders";
        public const string Requisitions = "Requisitions";
        #endregion
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static List<string> GeneratePermissionsForMenu(string module)
        {
            return new List<string>()
            {
                $"Permissions.Menu.{module}"
            };
        }

        public static List<string> GetAllPermissionsModules()
        {
            return new List<string>()
            {
                Users
                ,Roles
                ,RoleClaims
                ,Clients
                ,Contacts
                ,Providers
                ,Products
                ,Articles
                ,ArticleFamilies
                ,ArticleGroups
                ,Quotes
                ,Projects
                ,PurchaseOrders
                ,Requisitions
            };
        }

        public static List<string> GetAllPermissionsMenus()
        {
            return new List<string>()
            {
                Users
                ,Roles
                ,Clients
                ,Providers
                ,Products
                ,Articles
                ,Projects
                ,PurchaseOrders
                ,Requisitions
            };
        }

        public static List<string> GetSpecialPermissions()
        {
            return new List<string>()
            {
                Permissions.ProjectSpecial.ClientChange
                ,Permissions.Reports.View
                ,Permissions.Reports.Create
            };
        }
    }
}
