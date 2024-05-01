namespace Demo.WalletManagement.Api.Models.Domain;

public class Wallet
{
   
    public int Id { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public double Balance { get; set; }
}