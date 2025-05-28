using Kolokwium1.Services;
using Microsoft.AspNetCore.Mvc;
using Kolokwium1.Models.DTOs;
using Kolokwium1.Exceptions;

namespace Kolokwium1.Controllers;

    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ICarRentalService _carRentalService;

        public ClientsController(ICarRentalService carRentalService)
        {
            _carRentalService = carRentalService;
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientRentals(int clientId)
        {
            if (clientId <= 0)
            {
                return BadRequest("Client ID must be a positive number");
            }

            try
            {
                var result = await _carRentalService.GetClientRentalsAsync(clientId);
                return Ok(result);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClientWithRental([FromBody] NewClientRentalDto newClientRental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newClientRental.DateFrom >= newClientRental.DateTo)
            {
                return BadRequest("End date must be after start date");
            }

            try
            {
                var result = await _carRentalService.AddClientWithRentalAsync(newClientRental);
                return CreatedAtAction(nameof(GetClientRentals), new { clientId = result }, null);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }