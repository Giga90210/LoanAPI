using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public LoanType LoanType { get; set; } 
        public double LoanAmount { get; set; }
        public string Currency {  get; set; }
        public int LoanPeriod { get; set; }
        public LoanStatus Status { get; set; } = LoanStatus.InProcess;
        public int UserId { get; set; }
    }
}
