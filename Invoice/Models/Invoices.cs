using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Models
{
    public class Invoices
    {
        public Invoices()
        {
            this.Taxamount = 0;
            this.Totalamount = 0;
            this.Netamount = 0;
        }
        public int Id { get; set; }
        [StringLength(30)]
        [DisplayName("Invoice Number")]
        public string InvoiceNumber { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayName("Invoice Date")]
        public DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [DisplayName("Project name")]
        public string Projectname { get; set; }
        [DisplayName("Payment status")]
        public bool Paymentstatus { get; set; }
        public int Taxamount { get; set; }
        public int Totalamount { get; set; }
        public int Netamount { get; set; }
        


    }
}
