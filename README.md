# 🏧 Digital Wallet Challenge

API desenvolvida com .NET 9 para gerenciamento de carteiras digitais, permitindo transações financeiras entre usuários com autenticação, validações e persistência de dados.

## ✔ Tecnologias

- .NET 9
- Swagger
- BCrypt.Net
- AutoMapper
- FluentValidation
- MediatR
- JWT
- EntityFramework
- MSTest
- Moq

## 📝 Funcionalidades

* Autenticação
* Criar um usuário
* Consultar saldo da carteira de um usuário
* Adicionar saldo à carteira
* Criar uma transferência entre usuários (carteiras)
* Listar transferências realizadas por um usuário, com filtro opcional por período de data

## 🚀 Execução do projeto

### 📍 Local
Para rodar a aplicação localmente, siga os passos abaixo:

1. Clone este repositório:

   ```bash
   git clone https://github.com/Matheus7k/Wallet.git
   ```

2. Tenha uma instância do PostgresSQL rodando localmente ou configure o PostgreSQL com Docker seguindo os passos:
    - Baixe a imagem do PostgreSQL 16:
        ```bash
        docker pull postgres:16
        ```
    - Inicie um container do PostgreSQL:
        ```bash
        docker run -p 5432:5432 -e POSTGRES_PASSWORD=postgres postgres:16
        ```

3. Acesse o banco de dados e execute os scripts localizados em em `deployment/scripts`

4. Abra o projeto na sua IDE preferida e execute o projeto.

5. Você terá acesso ao Swagger da aplicação através do link: `http://localhost:5065/swagger/index.html`

### 🐳 Docker
Se preferir executar a aplicação usando Docker, siga os passos abaixo:

1. Na raiz do projeto, execute:
    ```
    docker-compose up -d
    ```

2. Os scripts de criação e população do banco serão executados automaticamente na primeira inicialização.

3. Acesse o Swagger: `http://localhost:5065/swagger/index.html`

4. Para parar e limpar tudo:
     ```
    docker-compose down -v
    ```

## ✅ Testes Unitários

Utiliza MSTest com Moq para testes de unidade.

Execute com:

```
dotnet test
```

## 👤 Usuários de exemplo para autenticação
  ```
  {
    "email": "maria@email.com",
    "password": "SenhaForte321#"
  }
  ```
  ```
  {
    "email": "joao@email.com",
    "password": "SenhaForte123#"
  }
  ```