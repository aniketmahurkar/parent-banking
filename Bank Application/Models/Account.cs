using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_Application.Models
{
    public class Account
    {
        public Account()
        {
            Transaction = new List<Transaction>();
            Wishlist = new List<WishList>();
            Date = DateTime.Now;
        }
        public int Id { get; set; }
        [DisplayName("Owner Email")]
        [ReadOnly(true)]
        [EmailAddress]
        public string OwnerEmail { get; set; }
        [DisplayName ("Recipient Email")]
        [EmailAddress]
        public string RecipientEmail { get; set; }
        public float Interest { get; set; }
        public string AccountName { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }
       [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Range(10, 100)]
        public long InterestRate { get; set; }
        public float Balance { get; set; }
        public virtual List<Transaction> Transaction { get; set; }
        public virtual List<WishList> Wishlist { get; set; }
        public string Username { get; set; }
        
        public float interestearned { get; set; }

        public float PrincipalPercentage()
        {
            float Amount = Balance + interestearned;
            float Princpercent = (Balance / Amount) * 100;
            return Princpercent;
        }

        public float InterestPercentage()
        {
            float FinInterest = Balance + interestearned;
            float Intersetperct = (interestearned / FinInterest) * 100;
            return Intersetperct;
        }

        public DateTime AccountDate()
        {
            return DateTime.Now;
        }
    }
}