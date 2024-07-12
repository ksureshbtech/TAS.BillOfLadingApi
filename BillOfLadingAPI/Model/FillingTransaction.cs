using System.Diagnostics.Contracts;

namespace BillOfLadingAPI.Model
{
    public class FillingTransaction
    {
        public int FillingTransactionId { get; set; }
        public string VehicleId { get;set; }=string.Empty;

        public string Customer { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public string OrderNumber { get; set; } = string.Empty;


       public string LoadNumber { get; set; }

        public decimal TotalRequest { get; set; }

        public DateTime LoadStartTime { get; set; }

        public DateTime LoadEndTime { get; set; } 

        public int TotalNoOfCompartments { get; set; }

        public int FleetId { get;set; }

        public long orderId { get;set; }

        public List<FillingTransactionDetail> Details { get; set; } = new List<FillingTransactionDetail>();


    }

    public class ReportTransactionDetail
    {
        public List<FillingTransaction>? FillingTransactions { get; set; }
    }
}
