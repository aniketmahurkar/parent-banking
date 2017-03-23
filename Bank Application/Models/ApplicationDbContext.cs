using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank_Application.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Bank_Application.Models.Transaction> Transactions { get; set; }

        public System.Data.Entity.DbSet<Bank_Application.Models.WishList> WishLists { get; set; }

        public System.Data.Entity.DbSet<Bank_Application.Models.Account> Accounts { get; set; }
    }
}