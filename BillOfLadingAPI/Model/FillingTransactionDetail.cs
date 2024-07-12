using System.Diagnostics.Contracts;

namespace BillOfLadingAPI.Model
{
    public class FillingTransactionDetail
    {
        public int CompartmentNo { get;set; }

        public string ProductCode { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty; 

        public int Metric { get; set; }   

        public string ControllerCode { get; set; } = string.Empty;
    
        public decimal CompartmentVolume { get; set; }

        public int FillingTransactionId { get; set; }
    }
}
