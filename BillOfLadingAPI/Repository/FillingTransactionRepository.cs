using BillOfLadingAPI.Model;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BillOfLadingAPI.Repository
{
    public class FillingTransactionRepository : IFillingTransactionRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<FillingTransactionRepository> _logger;
        private string detailProductCode;

        public FillingTransactionRepository(ILogger<FillingTransactionRepository> logger, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnectionString");
            _logger = logger;
        }


        public async Task<FillingTransaction> GetBillOfLandingAsync(long orderId)
        {
            try
            {
                _logger.LogInformation($"OrderRepository.GetBillOfLandingAsync: Getting Bill Of Lading: {orderId}.");

                FillingTransaction? transaction = null;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[Usp_Get_BillOfLanding]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pi_OrderId", SqlDbType.BigInt) { Value = orderId });

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                // Read the first result set
                                while (await reader.ReadAsync())
                                {
                                    FillingTransaction fillingTransaction = new FillingTransaction();
                                    fillingTransaction.VehicleId = reader.IsDBNull(reader.GetOrdinal("VehicleId")) ? null : reader.GetString(reader.GetOrdinal("VehicleId"));
                                    fillingTransaction.Customer = reader.IsDBNull(reader.GetOrdinal("Customer")) ? null : reader.GetString(reader.GetOrdinal("Customer"));
                                    fillingTransaction.CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ? null : reader.GetString(reader.GetOrdinal("CustomerId"));
                                    fillingTransaction.LoadNumber = reader.IsDBNull(reader.GetOrdinal("LoadNumber")) ? null : reader.GetString(reader.GetOrdinal("LoadNumber"));
                                    fillingTransaction.TotalRequest = reader.IsDBNull(reader.GetOrdinal("TotalRequest")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotalRequest"));
                                    fillingTransaction.LoadStartTime = reader.IsDBNull(reader.GetOrdinal("LoadStartTime")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LoadStartTime"));
                                    fillingTransaction.LoadEndTime = reader.IsDBNull(reader.GetOrdinal("LoadEndTime")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LoadEndTime"));
                                    fillingTransaction.TotalNoOfCompartments = reader.IsDBNull(reader.GetOrdinal("TotalNoCompartments")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalNoCompartments"));
                                    fillingTransaction.FleetId = reader.IsDBNull(reader.GetOrdinal("FleetId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FleetId"));
                                    orderId = reader.IsDBNull(reader.GetOrdinal("OrderId")) ? 0 : reader.GetInt64(reader.GetOrdinal("OrderId"));
                                    transaction = fillingTransaction;
                                }
                            }

                            // Move to the second result set
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    if (transaction != null)
                                    {
                                        FillingTransactionDetail detail = new FillingTransactionDetail();

                                        detail.CompartmentNo = reader.IsDBNull(reader.GetOrdinal("CompartmentNo")) ? 0 : reader.GetInt32(reader.GetOrdinal("CompartmentNo"));
                                        detail.ProductCode = reader.IsDBNull(reader.GetOrdinal("ProductCode")) ? null : reader.GetString(reader.GetOrdinal("ProductCode"));
                                        detail.ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? null : reader.GetString(reader.GetOrdinal("ProductName"));
                                        detail.CompartmentVolume = reader.IsDBNull(reader.GetOrdinal("TotalVolume")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotalVolume"));
                                        detail.Metric = reader.IsDBNull(reader.GetOrdinal("Metric")) ? 0 : reader.GetInt32(reader.GetOrdinal("Metric"));
                                        detail.ControllerCode = reader.IsDBNull(reader.GetOrdinal("ControllerCode")) ? null : reader.GetString(reader.GetOrdinal("ControllerCode"));


                                        transaction.Details.Add(detail);
                                    }
                                }
                            }
                        }
                    }
                }

                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OrderRepository.GetBillOfLandingAsync: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }


        
        public async Task<List<FillingTransaction>> GetBillOfLandingsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"OrderRepository.GetBillOfLandingAsync: Getting Bill Of Lading: {startDate} and {endDate}.");

                var transactions = new List<FillingTransaction>();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[Usp_Get_BillOfLanding]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pi_LoadStartDate", SqlDbType.DateTime) { Value = startDate });
                        command.Parameters.Add(new SqlParameter("@pi_LoadEndDate", SqlDbType.DateTime) { Value = endDate });

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var transaction = new FillingTransaction
                                {
                                    FillingTransactionId = reader.IsDBNull(reader.GetOrdinal("FillingTransactionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FillingTransactionId")),
                                    VehicleId = reader.IsDBNull(reader.GetOrdinal("VehicleId")) ? null : reader.GetString(reader.GetOrdinal("VehicleId")),
                                    Customer = reader.IsDBNull(reader.GetOrdinal("Customer")) ? null : reader.GetString(reader.GetOrdinal("Customer")),
                                    CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ? null : reader.GetString(reader.GetOrdinal("CustomerId")),
                                    LoadNumber = reader.IsDBNull(reader.GetOrdinal("LoadNumber")) ? null : reader.GetString(reader.GetOrdinal("LoadNumber")),
                                    TotalRequest = reader.IsDBNull(reader.GetOrdinal("TotalRequest")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotalRequest")),
                                    LoadStartTime = reader.IsDBNull(reader.GetOrdinal("LoadStartTime")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LoadStartTime")),
                                    LoadEndTime = reader.IsDBNull(reader.GetOrdinal("LoadEndTime")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("LoadEndTime")),
                                    TotalNoOfCompartments = reader.IsDBNull(reader.GetOrdinal("TotalNoCompartments")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalNoCompartments")),
                                    FleetId = reader.IsDBNull(reader.GetOrdinal("FleetId")) ? 0 : reader.GetInt32(reader.GetOrdinal("FleetId")),
                                    orderId = reader.IsDBNull(reader.GetOrdinal("OrderId")) ? 0 : reader.GetInt64(reader.GetOrdinal("OrderId")),

                                    Details = new List<FillingTransactionDetail>()
                                };

                                transactions.Add(transaction);
                            }

                            // Move to the next result set which contains the details
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var detail = new FillingTransactionDetail
                                    {
                                        CompartmentNo = reader.GetInt32(reader.GetOrdinal("CompartmentNo")),
                                        ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                        CompartmentVolume = reader.GetDecimal(reader.GetOrdinal("TotalVolume")),
                                        Metric = reader.GetInt32(reader.GetOrdinal("Metric")),
                                        ControllerCode = reader.GetString(reader.GetOrdinal("ControllerCode")),
                                        FillingTransactionId = reader.GetInt32(reader.GetOrdinal("FillingTransactionId"))
                                    };

                                    var matchingTransaction = transactions.FirstOrDefault(t => t.FillingTransactionId == detail.FillingTransactionId);
                                    matchingTransaction?.Details.Add(detail);
                                }
                            }
                        }
                    }
                }

                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in OrderRepository.GetBillOfLandingAsync: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }

    }
}


