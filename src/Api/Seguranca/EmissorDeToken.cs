using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Api.Seguranca;

public class EmissorDeToken
{
    private readonly OpcoesDeToken _opcoes;

    public EmissorDeToken(IOptions<OpcoesDeToken> opcoes)
    {
        _opcoes = opcoes.Value;
    }

    public TokenResposta Emitir(string login)
    {
        var minutos = _opcoes.ExpiraEmMinutos < 1 ? 1 : _opcoes.ExpiraEmMinutos;
        var expiraEm = DateTime.UtcNow.AddMinutes(minutos);
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opcoes.Chave));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _opcoes.Emissor,
            audience: _opcoes.Publico,
            claims: [new Claim(ClaimTypes.Name, login)],
            expires: expiraEm,
            signingCredentials: credenciais);

        return new TokenResposta
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Tipo = "Bearer",
            ExpiraEm = expiraEm
        };
    }
}
