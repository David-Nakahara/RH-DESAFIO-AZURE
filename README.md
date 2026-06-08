# RH Azure Desafio - Sistema de Gestão de Funcionários

Web API em .NET 8 com deploy no Microsoft Azure.

## Tecnologias
- **ASP.NET Core 8** - Web API
- **Entity Framework Core** - ORM para SQL Database
- **Azure SQL Database** - Banco de dados relacional
- **Azure Table Storage** - Armazenamento de logs
- **Azure App Service** - Hospedagem da API
- **Swagger** - Documentação dos endpoints

## Endpoints

| Verbo  | Endpoint              | Descrição                  |
|--------|-----------------------|----------------------------|
| GET    | /Funcionario/{id}     | Obtém funcionário por ID   |
| POST   | /Funcionario          | Cria novo funcionário      |
| PUT    | /Funcionario/{id}     | Atualiza funcionário       |
| DELETE | /Funcionario/{id}     | Deleta funcionário         |

## Como rodar localmente

### 1. Pré-requisitos
- .NET 8 SDK
- SQL Server ou Azure SQL
- Azure Storage Account (ou Azurite para emulação local)

### 2. Configurar connection strings no appsettings.json
```json
{
  "ConnectionStrings": {
    "SqlConnection": "sua connection string do SQL",
    "TableStorageConnection": "sua connection string do Table Storage"
  }
}
```

### 3. Gerar e aplicar a Migration
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Rodar a aplicação
```bash
dotnet run
```
Acesse o Swagger em: `https://localhost:5001/swagger`

## Deploy no Azure

### 1. Criar recursos no Azure
- **Resource Group**: crie um grupo para organizar os recursos
- **Azure SQL Server + Database**: para persistência dos funcionários
- **Storage Account + Table**: para armazenar os logs (a tabela `FuncionarioLog` é criada automaticamente)
- **App Service**: para hospedar a API (plano Free é suficiente para testes)

### 2. Configurar Connection Strings no App Service
No portal Azure:
`App Service → Configurações → Configuração → Connection Strings`

Adicione:
- `SqlConnection` → sua string do Azure SQL
- `TableStorageConnection` → sua string do Storage Account

### 3. Publicar via VS Code
```bash
dotnet publish -c Release
# ou use a extensão Azure App Service no VS Code
```

## Estrutura do Projeto
```
RH-Azure/
├── Controllers/
│   └── FuncionarioController.cs   # Endpoints CRUD
├── Models/
│   ├── Funcionario.cs             # Entidade principal
│   ├── FuncionarioLog.cs          # Entidade de log (Table Storage)
│   └── TipoAcao.cs                # Enum: Criou, Atualizou, Deletou
├── Context/
│   └── RHContext.cs               # DbContext do EF Core
├── Repository/
│   └── FuncionarioRepository.cs   # Acesso a dados (SQL + Table Storage)
├── Program.cs                     # Configuração e startup
└── appsettings.json               # Connection strings
```

## Fluxo de Log
Toda ação (criar, atualizar, deletar) registra automaticamente um log na Azure Table:
- **PartitionKey**: Departamento do funcionário
- **RowKey**: GUID único
- **TipoAcao**: Criou (1), Atualizou (2), Deletou (3)
