﻿using System.ComponentModel.DataAnnotations;

namespace Demo.WalletManagement.Api.Models.DTO;

public class UpdateWalletDto
{
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 8)]
    public string AccountName { get; set; } = string.Empty;

    [Required]
    [StringLength(maximumLength:15, MinimumLength = 10)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required] 
    public double Balance { get; set; }
}