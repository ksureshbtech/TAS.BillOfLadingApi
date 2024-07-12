using BillOfLadingAPI.Model;

namespace BillOfLadingAPI.Repository
{
    public interface IFillingTransactionRepository
    {
        Task<FillingTransaction> GetBillOfLandingAsync(long orderId);

        Task<List<FillingTransaction>> GetBillOfLandingsAsync(DateTime startDate, DateTime endDate);
    }
}
