# Configuração (`appsettings.json`)

O JSON não aceita comentários. Este arquivo explica o que cada bloco faz e onde ficam os segredos.

Leia também o **Alerta de segredos** no [README da API](../../README.md#alerta-de-segredos).

## `Catalogo`

Título e subtítulo devolvidos em `GET /api/configuracao` para a vitrine.

## `Jwt`

- `Chave`: assina o token Bearer. **Segredo de estudo** no repositório; em produção use `Jwt__Chave` no host.
- `ExpiraEmMinutos`: validade do token (padrão 120).

## `UsuarioInicial`

Login e senha usados só na **primeira** criação do banco (tabela `usuarios` vazia). **Segredo de estudo**; em produção use variáveis de ambiente.

## `Cors` e `AllowedHosts`

Origens do front e nomes de host em que a API responde. Veja o README da API para deploy.

## Sobrescrever sem editar o arquivo

Variável de ambiente ganha do JSON. No .NET, `Jwt:Chave` vira `Jwt__Chave`. A tabela completa está no README.
