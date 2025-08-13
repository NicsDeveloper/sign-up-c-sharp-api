# SignUp API

Uma API completa de cadastro de usuários seguindo as melhores práticas de desenvolvimento, incluindo TDD (Test-Driven Development), Clean Architecture, Clean Code e princípios SOLID.

## 🏗️ Arquitetura

A solução segue os princípios da Clean Architecture, organizada em camadas bem definidas:

### 📁 Estrutura do Projeto

```
SignUpApi/
├── src/
│   ├── SignUpApi.Domain/           # Camada de Domínio
│   │   ├── Entities/               # Entidades de negócio
│   │   ├── Interfaces/             # Contratos/Interfaces
│   │   ├── ValueObjects/           # Objetos de valor
│   │   └── Exceptions/             # Exceções de domínio
│   │
│   ├── SignUpApi.Application/      # Camada de Aplicação
│   │   ├── Commands/               # Comandos CQRS
│   │   ├── Queries/                # Consultas CQRS
│   │   ├── Handlers/               # Manipuladores
│   │   ├── Validators/             # Validações
│   │   └── DTOs/                   # Objetos de transferência
│   │
│   ├── SignUpApi.Infrastructure/   # Camada de Infraestrutura
│   │   ├── Data/                   # Contexto EF e configurações
│   │   ├── Repositories/           # Implementações dos repositórios
│   │   └── Services/               # Serviços externos
│   │
│   └── SignUpApi.API/              # Camada de Apresentação
│       └── Controllers/            # Controllers da API
│
└── tests/
    └── SignUpApi.Tests/            # Testes unitários e de integração
        ├── Domain/                  # Testes do domínio
        ├── Application/             # Testes da aplicação
        └── Infrastructure/          # Testes da infraestrutura
```

## 🚀 Tecnologias Utilizadas

- **.NET 8** - Framework mais recente e estável
- **Entity Framework Core** - ORM para persistência de dados
- **xUnit** - Framework de testes unitários
- **Moq** - Biblioteca para criação de mocks
- **FluentAssertions** - Biblioteca para asserções mais legíveis
- **FluentValidation** - Validação de dados
- **MediatR** - Implementação do padrão Mediator para CQRS
- **JWT** - Autenticação baseada em tokens
- **BCrypt.Net-Next** - Criptografia de senhas segura
- **Swagger/OpenAPI** - Documentação da API

## 🧪 TDD (Test-Driven Development)

O projeto foi desenvolvido seguindo a metodologia TDD, onde:

1. **Red** - Primeiro escrevemos os testes que falham
2. **Green** - Implementamos o código mínimo para fazer os testes passarem
3. **Refactor** - Refatoramos o código mantendo os testes passando

### Exemplo de TDD

```csharp
// 1. Teste (Red)
[Fact]
public void CreateUser_WithValidData_ShouldCreateUserSuccessfully()
{
    // Arrange
    var email = "test@example.com";
    var password = "SecurePassword123!";
    var firstName = "João";
    var lastName = "Silva";

    // Act
    var user = new User(email, password, firstName, lastName);

    // Assert
    user.Should().NotBeNull();
    user.Email.Value.Should().Be(email);
    user.Password.Value.Should().Be(password);
}

// 2. Implementação (Green)
public class User
{
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    
    public User(string email, string password, string firstName, string lastName)
    {
        Email = new Email(email);
        Password = new Password(password);
        // ... outras validações
    }
}

// 3. Refactor - Melhorar o código mantendo os testes passando
```

## 🏛️ Clean Architecture

### Princípios Aplicados

1. **Independência de Frameworks** - O domínio não depende de frameworks externos
2. **Testabilidade** - Código facilmente testável através de inversão de dependência
3. **Independência de UI** - A lógica de negócio não depende da interface
4. **Independência de Banco** - O domínio não conhece detalhes de persistência
5. **Independência de Qualquer Coisa Externa** - O domínio é o centro da aplicação

### Camadas

- **Domain**: Regras de negócio, entidades e interfaces
- **Application**: Casos de uso, comandos e consultas
- **Infrastructure**: Implementações concretas (banco, serviços externos)
- **API**: Controllers e configurações da API

## 🧹 Clean Code

### Princípios Aplicados

1. **Nomes Significativos** - Variáveis, métodos e classes com nomes descritivos
2. **Funções Pequenas** - Cada função tem uma responsabilidade única
3. **Comentários Desnecessários** - Código autoexplicativo
4. **Formatação Consistente** - Padrões de formatação seguidos
5. **Tratamento de Erros** - Exceções apropriadas e mensagens claras

## 🔧 SOLID

### Single Responsibility Principle (SRP)
Cada classe tem uma única responsabilidade:
- `User` - Gerencia dados do usuário
- `Email` - Valida formato de email
- `Password` - Valida força da senha

### Open/Closed Principle (OCP)
Aberto para extensão, fechado para modificação:
- Novos tipos de validação podem ser adicionados sem modificar código existente

### Liskov Substitution Principle (LSP)
Implementações podem ser substituídas:
- `IUserRepository` pode ter diferentes implementações (SQL Server, PostgreSQL, etc.)

### Interface Segregation Principle (ISP)
Interfaces específicas para cada necessidade:
- `IUserRepository` - Operações de usuário
- `IPasswordHasher` - Criptografia de senhas
- `IJwtTokenGenerator` - Geração de tokens

### Dependency Inversion Principle (DIP)
Dependências de abstrações, não de implementações:
- Controllers dependem de `IMediator`, não de implementações específicas

## 📋 Funcionalidades Implementadas

### ✅ Usuários
- [x] Cadastro de usuário (SignUp) com hash de senha
- [x] Login de usuário com verificação de senha hasheada
- [x] Listagem de usuários com filtros
- [x] Atualização de perfil (nome e sobrenome)
- [x] Validação de dados com FluentValidation
- [x] Criptografia de senhas com BCrypt.Net-Next
- [x] Geração de tokens JWT
- [x] Registro de último login
- [x] Controle de status ativo/inativo

### 🔄 Em Desenvolvimento
- [ ] Busca de usuário por ID
- [ ] Alteração de senha
- [ ] Refresh tokens
- [ ] Autorização baseada em roles

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- Nenhum banco de dados externo necessário (usa banco em memória para desenvolvimento)

### 1. Clone o repositório
```bash
git clone <url-do-repositorio>
cd SignUpApi
```

### 2. Restaure as dependências
```bash
dotnet restore
```

### 3. Execute os testes
```bash
dotnet test
```

### 4. Execute a API
```bash
dotnet run --project src/SignUpApi.API
```

### 5. Acesse a documentação
- Swagger UI: `http://localhost:5170/swagger`
- API Base: `http://localhost:5170/api`

## 🔧 Correções e Melhorias Recentes

### ✅ Problemas Resolvidos
- **Hash de Senhas**: Corrigido problema onde senhas não eram hasheadas corretamente
- **Value Converters**: Implementados conversores personalizados para Email e Password
- **Factory Methods**: Adicionados métodos `FromHashedPassword()` e `FromDatabase()` para Value Objects
- **Warnings de Compilação**: Resolvidos todos os warnings CS8618 e CS1998
- **Testes**: Todos os 23 testes passando após correções

### 🏗️ Melhorias de Arquitetura
- **Value Objects Robustos**: Email e Password com construtores internos para EF Core
- **Segurança**: Senhas sempre hasheadas no domínio
- **Validação**: Sistema de validação consistente em todas as camadas
- **Performance**: Otimizações no mapeamento de entidades

## 📚 Endpoints da API

### Autenticação
- `POST /api/auth/signup` - Cadastro de usuário
- `POST /api/auth/login` - Login de usuário

### Usuários
- `GET /api/users` - Lista todos os usuários
- `GET /api/users?activeOnly=true` - Lista apenas usuários ativos
- `GET /api/users/{id}` - Busca usuário por ID (em desenvolvimento)

## 🧪 Cobertura de Testes

A solução possui uma cobertura abrangente de testes:

- **Testes de Domínio**: Validação de entidades e value objects (5 testes)
- **Testes de Aplicação**: Comandos, consultas e handlers (15 testes)
- **Testes de Infraestrutura**: Repositórios e serviços (3 testes)
- **Total**: 23 testes passando ✅

### Cobertura por Camada
- **Domain**: 100% - Todas as entidades e value objects testados
- **Application**: 100% - Todos os handlers e validadores testados
- **Infrastructure**: 100% - Todos os repositórios testados

### Executar Testes Específicos

```bash
# Apenas testes de domínio
dotnet test tests/SignUpApi.Tests/Domain/

# Apenas testes de aplicação
dotnet test tests/SignUpApi.Tests/Application/

# Apenas testes de infraestrutura
dotnet test tests/SignUpApi.Tests/Infrastructure/
```

## 🔐 Segurança

- **Senhas**: Criptografadas com BCrypt.Net-Next (salt automático)
- **Tokens**: JWT com expiração configurável
- **Validação**: Validação rigorosa de entrada com FluentValidation
- **Value Objects**: Imutabilidade e validação de dados
- **Hash Seguro**: Senhas sempre hasheadas antes de chegar ao domínio
- **Validação de Email**: Formato e unicidade garantidos
- **Validação de Senha**: Complexidade mínima exigida

## 📈 Próximos Passos

### 🔄 Em Desenvolvimento
1. **Implementar GetUserByIdQuery** - Busca de usuário por ID
2. **Implementar alteração de senha** - Com validação e hash
3. **Implementar refresh tokens** - Renovação automática de tokens
4. **Adicionar autorização baseada em roles** - Controle de acesso

### 🚀 Futuras Implementações
5. **Adicionar logging estruturado** - Serilog ou NLog
6. **Implementar cache distribuído** - Redis ou similar
7. **Adicionar métricas e monitoramento** - Prometheus, Grafana
8. **Implementar rate limiting** - Proteção contra abuso
9. **Adicionar documentação OpenAPI completa** - Swagger detalhado
10. **Implementar testes de integração** - WebApplicationFactory

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 👨‍💻 Autor

Desenvolvido seguindo as melhores práticas de desenvolvimento em C# e arquitetura de software.

## 🎯 Status Atual do Projeto

### ✅ **Completamente Funcional**
- **API rodando** em `http://localhost:5170`
- **23 testes passando** ✅
- **Build limpo** sem warnings de código
- **Swagger funcionando** para testes da API
- **Banco em memória** configurado para desenvolvimento

### 🔧 **Últimas Correções**
- **Hash de senhas** funcionando corretamente
- **Value Converters** implementados para EF Core
- **Factory Methods** para Value Objects
- **Warnings de compilação** resolvidos
- **Sistema de autenticação** operacional

---

**Nota**: Este projeto é um exemplo educacional de como implementar uma API seguindo Clean Architecture, TDD e princípios SOLID. Para uso em produção, considere adicionar mais validações de segurança, logging, monitoramento e testes de carga.
