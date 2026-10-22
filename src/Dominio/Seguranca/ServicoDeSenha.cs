using System.Security.Cryptography;

namespace Dominio.Seguranca;

public static class ServicoDeSenha
{
    public const int Iteracoes = 100_000;
    public const string HashFalso = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
    public const string SalFalso = "AAAAAAAAAAAAAAAAAAAAAA==";

    public static (string Hash, string Sal) Gerar(string senha)
    {
        var sal = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(senha, sal, Iteracoes, HashAlgorithmName.SHA256, 32);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(sal));
    }

    public static bool Confere(string senha, string hashArmazenado, string salArmazenado)
    {
        var sal = Convert.FromBase64String(salArmazenado);
        var esperado = Convert.FromBase64String(hashArmazenado);
        var calculado = Rfc2898DeriveBytes.Pbkdf2(senha, sal, Iteracoes, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(esperado, calculado);
    }
}
