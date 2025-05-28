using Kolokwium1.Models.DTOs;

namespace Kolokwium1.Services;

public interface ICarRentalService
{
    Task<ClientRentalsDto> GetClientRentalsAsync(int clientId);
    Task<int> AddClientWithRentalAsync(NewClientRentalDto newClientRental);
}