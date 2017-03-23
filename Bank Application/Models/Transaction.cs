using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_Application.Models
{
    
    public class Transaction
    {
        public int Id { get; set; }

        [CustomValidation(typeof(Transaction), "ValidateTransactionDate")]
        public DateTime TransactionDate { get; set; }
        [CustomValidation(typeof(Transaction), "ValidateAmount")]
        public int Amount { get; set; }
        [Required]
        public string Note { get; set; }

        public virtual int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public string Username { get; set; }


        public static ValidationResult ValidateAmount(int Amount, ValidationContext context)
        {
            if (Amount == 0)
            {
                return new ValidationResult("Amount cannot be 0");
            }
            return ValidationResult.Success;
        }
        public static ValidationResult ValidateBalance(int Amount, int Balance, ValidationContext context)
        {

            if (Balance - Amount < 0)
            {
                return new ValidationResult("Amount greater than Balance");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateTransactionDate(DateTime TransactionDate, ValidationContext context)
        {
            if (TransactionDate > DateTime.Now)
            {
                return new ValidationResult("Transaction Date cannot be greater than Today");
            }
            if (TransactionDate.Year < DateTime.Now.Year)
            {
                return new ValidationResult("Transaction Year cannot be less than current Year");
            }
            return ValidationResult.Success;
        }
    }
}