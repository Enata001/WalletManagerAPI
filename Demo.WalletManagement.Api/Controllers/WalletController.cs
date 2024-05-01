using Demo.WalletManagement.Api.CustomActionFilters;
using Demo.WalletManagement.Api.Models.Domain;
using Demo.WalletManagement.Api.Models.DTO;
using Demo.WalletManagement.Api.Repository.Interface;
using Demo.WalletManagement.Api.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WalletManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletRepo _walletRepo;
    private readonly ICacheService _cacheService;
    private readonly ILogger<WalletController> _logger;


    public WalletController(IWalletRepo walletRepo, ILogger<WalletController> logger, ICacheService cacheService)
    {
        _walletRepo = walletRepo;
        _logger = logger;
        _cacheService = cacheService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cacheData = _cacheService.GetData<List<Wallet>>("wallets");
        if (cacheData != null && cacheData.Any())
        {
            return Ok(cacheData);
        }

        var expiryTime = DateTimeOffset.Now.AddMinutes(1);
        cacheData = await _walletRepo.GetAllAsync();
        _cacheService.SetData("drivers", cacheData, expiryTime);
        return Ok(cacheData);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var cacheData = _cacheService.GetData<Wallet>($"wallet{id}");
            if (cacheData is not null)
            {
                return Ok(cacheData);
            }

            var result = await _walletRepo.GetByIdAsync(id);
            if (result is null)
            {
                _logger.LogError("Wallet not found");
                return NotFound();
            }

            var expiryTime = DateTimeOffset.Now.AddMinutes(1);
            _cacheService.SetData($"wallet{result.Id}", result, expiryTime);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateWallet([FromBody] WalletDto walletDto)
    {
        try
        {
            var wallet = new Wallet
            {
                Balance = walletDto.Balance,
                AccountName = walletDto.AccountName,
                AccountNumber = walletDto.AccountNumber
            };
            var result = await _walletRepo.CreateAsync(wallet: wallet);
            if (result is null)
            {
                _logger.LogError("Account Already Exists");
                return BadRequest(new { error = "Account Already exists" });
            }


            var expiryTime = DateTimeOffset.Now.AddMinutes(1);
            _cacheService.SetData($"wallet{result.Id}", result, expiryTime);


            return CreatedAtAction("GetById", new { result.Id }, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateWallet([FromRoute] int id, [FromBody] UpdateWalletDto walletDto)
    {
        try
        {
            var wallet = new Wallet
            {
                Id = id,
                Balance = walletDto.Balance,
                AccountName = walletDto.AccountName,
                AccountNumber = walletDto.AccountNumber
            };

            var result = await _walletRepo.UpdateAsync(id, wallet);
            if (result is null)
            {
                _logger.LogError("Account not found");
                return NotFound();
            }

            var expiryTime = DateTimeOffset.Now.AddMinutes(1);
            _cacheService.SetData($"wallet{result.Id}", result, expiryTime);


            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteWallet([FromRoute] int id)
    {
        try
        {
            var result = await _walletRepo.RemoveAsync(id);
            if (result is null)
            {
                _logger.LogInformation("Wallet not found");
                return NotFound();
            }

            _cacheService.RemoveData($"wallet{result.Id}");
            _logger.LogInformation("Delete successful");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}