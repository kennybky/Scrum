using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace Scrum.Data
{
    public class ScrumRole : IdentityRole<int>
    {
        public  ScrumRole(string role) : base(role)
        {
            // Here we define role descriptions
        }

        public ScrumRole() : base(Roles.Developer)
        {
            // define role for developer
        }

 
       
        

        public string Description { get; set; }

    }


    public static class Roles
    {
        public const string Admin = "ADMIN";
        public const string Scrum_Master = "SCRUM_MASTER";
        public const string Product_Owner = "PRODUCT_OWNER";
        public const string Developer = "DEVELOPER";


        public static ICollection<string> getRoles()
        {
            Type type = typeof(Roles);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var fields = type.GetFields(flags).Where(f => f.IsLiteral && f.FieldType == typeof(string)).Select(fi => (string)fi.GetRawConstantValue()).ToList();
            return fields;
        }
    }

   
}
