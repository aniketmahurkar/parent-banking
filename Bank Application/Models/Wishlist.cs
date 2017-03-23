using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_Application.Models
{
    public class WishList
    {
        public int Id { get; set; }
       
        public DateTime DateAdded { get; set; }
        [Required]
        public long Cost { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Purchased { get; set; }
        [Url]
        [Required]
        public string url { get; set; }
        public string Username { get; set; }
        public int purchasable { get; set; }

        public virtual int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}