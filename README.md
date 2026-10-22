# Catálogo de Produtos — API

API de estudo para quem está começando. Ela guarda produtos e imagens num arquivo SQLite, explica o token Bearer com um único usuário e documenta tudo no Swagger.

O visual e os slides de apresentação estão no projeto `produtos-ui` (`src/apresentacao/`). Este repositório é só a API.

## Alerta de segredos

Este é um **projeto de aprendizado**: `Jwt:Chave` e `UsuarioInicial` ficam em `src/Api/appsettings.json` e podem ir para o Git com valores de demonstração.

Em um **projeto real**, trate senhas e chaves como segredo: variáveis no host (Fly, Vercel, Azure), User Secrets na máquina de dev, cofre de chaves — **nunca** commite credenciais de produção. O front (`produtos-ui`) expõe `VITE_*` no navegador; lá o `.env` também só deve ter valores de estudo (veja o alerta no README da UI).

O JSON não aceita comentários. Explicação de cada bloco: [`src/Api/appsettings.md`](src/Api/appsettings.md).

## Instalar

1. Instale o [.NET SDK 10](https://dotnet.microsoft.com/download).
2. No terminal: `dotnet --version` (precisa aparecer 10).

## Rodar

Na pasta `produtos-api`, no terminal:

```bash
dotnet run --project src/Api
```

No Visual Studio, abra `Produtos.slnx`. No botão verde, escolha o perfil **Api** e aperte F5.

Abra http://localhost:5080/swagger

Na primeira execução a API cria `src/Api/dados/catalogo.db` com o usuário `admin` e oito produtos de exemplo. O console mostra cada requisição e, em Development, o SQL.

## Ver o README no Visual Studio

Abra `Produtos.slnx`. O README aparece em **Solution Items** e também dentro do projeto **Api**. Com o arquivo aberto, `Ctrl+Shift+V` mostra a pré-visualização.

## Como o código está dividido

| Pasta | O que vive ali |
| --- | --- |
| `src/Dominio` | Entidades, senha e os contratos dos repositórios |
| `src/Aplicacao` | O que a API faz: listar, cadastrar, conferir senha |
| `src/Infraestrutura` | Entity Framework, SQLite e os repositórios |
| `src/Api` | HTTP, Swagger, token, CORS, log e erros |
| `tests/TestesUnitarios` | Testes simples de senha, produtos e login |

Comece por `src/Api/Program.cs`. Ele só liga as peças.

## Tabelas

`produtos`: `id`, `nome`, `descricao`, `data_cadastro`, `data_atualizacao`, `esta_ativo`

`imagens`: `id`, `produto_id`, `url`, `data_cadastro`

`usuarios`: `id`, `login`, `senha_hash`, `sal`, `data_cadastro`

Um produto tem várias imagens. Cada URL é uma linha em `imagens`, com o `produto_id` apontando para o produto. O `id` nasce sozinho. A entidade marca as datas no momento em que o repositório grava. `esta_ativo` começa verdadeiro; produto inativo some da vitrine.

Para olhar as tabelas, abra o arquivo `.db` no [DB Browser for SQLite](https://sqlitebrowser.org/). Se mudar colunas no código, apague o arquivo e rode de novo.

## Token Bearer

A senha `admin` / `admin@123` não fica gravada. No primeiro arranque a API sorteia um sal, mistura com a senha usando PBKDF2 (SHA-256, 100 mil vezes) e guarda só `senha_hash` e `sal`.

O token é um crachá temporário. Sem ele, listar e buscar produtos responde 401, assim como cadastrar, atualizar e apagar. Configuração, imagens, login e saúde continuam abertos. A vitrine (`produtos-ui`) chama `POST /api/autenticacao` no carregamento e usa o token no `GET /api/produtos`.

1. No Swagger, abra `POST /api/autenticacao`.
2. Envie `{ "login": "admin", "senha": "admin@123" }`.
3. Copie o campo `token`.
4. Clique em **Authorize** e cole o token. Não escreva a palavra Bearer: o Swagger acrescenta.
5. Rotas com cadeado passam a aceitar. Sem o token respondem 401.

O crachá vale 120 minutos (`Jwt:ExpiraEmMinutos`).

## Rotas

| Método | Caminho | Token |
| --- | --- | --- |
| GET | `/api/configuracao` | não |
| GET | `/api/produtos` | sim. `?incluirInativos=true` inclui os desativados |
| GET | `/api/produtos/{id}` | sim |
| POST | `/api/produtos` | sim |
| PUT | `/api/produtos/{id}` | sim |
| DELETE | `/api/produtos/{id}` | sim. Apaga o produto e as imagens |
| GET | `/api/imagens?produtoId=1` | não |
| GET | `/api/imagens/{id}` | não |
| POST | `/api/imagens` | sim. Várias URLs de uma vez |
| DELETE | `/api/imagens/{id}` | sim |
| POST | `/api/autenticacao` | não |
| GET | `/api/saude` | não |

Exemplo de cadastro:

```json
{
  "nome": "Colônia Lavanda",
  "descricao": "Fragrância fresca para o dia.",
  "urls": ["https://exemplo.com/foto.jpg"]
}
```

Status: 200 ok, 201 criado, 204 apagado, 400 dados inválidos, 401 sem token ou senha errada, 404 não existe, 500 erro inesperado. O corpo de erro é `{ "mensagem": "...", "status": 400 }`.

## O que dá para mudar

Tudo que muda entre a sua máquina e a nuvem está em `src/Api/appsettings.json` (veja [`appsettings.md`](src/Api/appsettings.md)). Variável de ambiente ganha do arquivo. No .NET, dois-pontos viram dois underlines.

| Arquivo | Variável |
| --- | --- |
| `Catalogo:Titulo` | `Catalogo__Titulo` |
| `Catalogo:Subtitulo` | `Catalogo__Subtitulo` |
| `ConnectionStrings:Catalogo` | `ConnectionStrings__Catalogo` |
| `Cors:OrigensPermitidas:0` | `Cors__OrigensPermitidas__0` |
| `Cors:PermitirProjetosVercel` | `Cors__PermitirProjetosVercel` |
| `Jwt:Chave` | `Jwt__Chave` |
| `Jwt:ExpiraEmMinutos` | `Jwt__ExpiraEmMinutos` |
| `UsuarioInicial:Login` | `UsuarioInicial__Login` |
| `UsuarioInicial:Senha` | `UsuarioInicial__Senha` |
| `Swagger:Habilitado` | `Swagger__Habilitado` |
| `AllowedHosts` | `AllowedHosts` |

O título e o subtítulo saem em `GET /api/configuracao`. O front só exibe.

O usuário inicial só é criado se a tabela `usuarios` estiver vazia. Para trocar a senha na sua máquina, apague o `.db`.

`Cors:PermitirProjetosVercel` vem ligado: qualquer site `https://....vercel.app` pode chamar a API. Para fechar, coloque `false` e liste o endereço exato em `Cors:OrigensPermitidas`.

`AllowedHosts` é o nome do servidor desta API. Em Development o valor é `localhost;127.0.0.1`, então ela responde quando o endereço chamado usa um desses nomes. O endereço exato do front fica em `Cors:OrigensPermitidas`: `http://localhost:5173` e `http://127.0.0.1:5173`. Na nuvem, troque `AllowedHosts` pelo domínio da API, por exemplo `sua-api.fly.dev`.

## Publicar a API na Vercel

A Vercel sobe o container descrito em `Dockerfile.vercel` e `vercel.json`. Ela não tem disco permanente: o SQLite fica em `/tmp` e os produtos de exemplo voltam quando uma instância nova sobe. Cadastro feito no Swagger pode sumir. Na sua máquina o arquivo permanece.

1. Envie este repositório para o GitHub.
2. Na Vercel: **Add New Project** e importe `produtos-api`.
3. Em Environment Variables:
   - `PORT` = `8080`
   - `Jwt__Chave` = uma frase com pelo menos 32 caracteres
   - `Catalogo__Titulo` = `Catálogo de Produtos`
   - `Catalogo__Subtitulo` = `PrograMaria e Grupo Boticário`
4. Deploy.
5. Abra `https://SEU-PROJETO.vercel.app/swagger`.

Se responder 502, confira se `PORT` é `8080`.

## Publicar a API no Fly.io

Para um backend .NET com SQLite que precisa guardar o arquivo, o [Fly.io](https://fly.io) tem cota gratuita e um volume de disco. O mesmo `Dockerfile.vercel` sobe lá. Crie um volume, monte em `/data` e aponte a conexão para ele:

`ConnectionStrings__Catalogo` = `Data Source=/data/catalogo.db`

`AllowedHosts` = o domínio que o Fly mostrar, sem `https://`.

O SQLite da Vercel vive em `/tmp` e some quando a instância reinicia. No Fly o arquivo fica no volume.

## Publicar o front

No projeto `produtos-ui`, a variável `VITE_URL_DA_API` é a URL desta API, sem barra no final. Ela entra no build: mudou a URL, faça o deploy do front de novo.

## Testes

Na pasta `produtos-api`:

```bash
dotnet test
```

Os testes ficam em `tests/TestesUnitarios`. Cada um cobre um comportamento: senha certa, senha errada, nome vazio, produto inexistente, lista só de ativos e login. Eles usam repositórios falsos em memória, sem banco.
