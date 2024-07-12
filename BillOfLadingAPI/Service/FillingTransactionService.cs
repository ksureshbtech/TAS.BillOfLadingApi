using BillOfLadingAPI.Model;
using BillOfLadingAPI.Repository;

namespace BillOfLadingAPI.Service
{
    public class FillingTransactionService : IFillingTransactionService
    {
        public IFillingTransactionRepository _fillingTransactionRepository { get; set; }
        public FillingTransactionService(IFillingTransactionRepository fillingTransactionRepository) {

            _fillingTransactionRepository=fillingTransactionRepository;
        }
        public async Task<FillingTransaction> GetBillOfLandingAsync(long orderId)
        {
            return await _fillingTransactionRepository.GetBillOfLandingAsync(orderId);
        }

        public async Task<List<FillingTransaction>> GetBillOfLandingsAsync(DateTime startDate, DateTime endDate)
        {
            return await _fillingTransactionRepository.GetBillOfLandingsAsync(startDate, endDate);
        }
    }
}
