﻿
using System.ComponentModel.DataAnnotations;
namespace WebAPIConsume.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please put the Customer Id for this order")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please put some description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please put the Order Cost as a decimal for this order")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal OrderCost { get; set; }
    }
}
