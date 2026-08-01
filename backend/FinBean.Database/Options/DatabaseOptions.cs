using System.ComponentModel.DataAnnotations;

namespace FinBean.Database.Options;

public class DatabaseOptions
{

    [Required]
    public required string Host {get; init;}
    [Required]
    public required int Port {get; init;}
    [Required]
    public required string Database {get; init;}
    [Required]
    public required string Username {get; init;}
    [Required]
    public required string Password {get; init;}

    public string GetConnectionString()
    {
        return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
    }
}