using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.Models
{
    public class eVoucher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Image { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentMethodPerDiscount { get; set; }
        public Int32 Qty { get; set; }
        public string Type { get; set; }
        public string MO_Name { get; set; }
        public string MO_PhNo { get; set; }
        public Int32 MO_MaxBuyLimit { get; set; }
        public string GTO_Name { get; set; }
        public string GTO_PhNo { get; set; }
        public Int32 GTO_GTOLimit { get; set; }
        public Int32 GTO_MaxBuyLimit { get; set; }
        public string PromoCode { get; set; }
        public string QRCode { get; set; }
        public string Email { get; set; }
    }
}
