using BillOfLadingAPI.Model;

namespace BillOfLadingAPI.Service
{
    public interface IFillingTransactionService
    {
        Task<FillingTransaction> GetBillOfLandingAsync(long orderId);

        Task<List<FillingTransaction>> GetBillOfLandingsAsync(DateTime startDate, DateTime endDate);




    }
}
