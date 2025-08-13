# 🚀 Aprendendo TDD, Clean Architecture e Clean Code com C#

## 📚 Introdução

Este documento é um guia completo e didático para aprender **TDD (Test-Driven Development)**, **Clean Architecture** e **Clean Code** através de uma API real de cadastro de usuários em C#.

## 🎯 O que você vai aprender

- ✅ **TDD**: Como escrever testes antes do código
- ✅ **Clean Architecture**: Como organizar seu código em camadas
- ✅ **Clean Code**: Como escrever código legível e manutenível
- ✅ **SOLID**: Como aplicar os princípios de design
- ✅ **CQRS**: Como separar comandos e consultas
- ✅ **Value Objects**: Como criar objetos imutáveis e seguros

---

## 🧪 TDD - Test-Driven Development

### 🤔 O que é TDD?

TDD é uma metodologia de desenvolvimento onde você **escreve os testes ANTES de escrever o código**. Parece estranho, mas é muito poderoso!

### 🔄 Ciclo TDD (Red-Green-Refactor)

#### 1️⃣ **RED** - Teste Falha
Primeiro, escrevemos um teste que falha (porque o código ainda não existe):

```csharp
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
```

**Resultado**: ❌ Falha! A classe `User` não existe ainda.

#### 2️⃣ **GREEN** - Código Mínimo
Agora escrevemos o código mínimo para fazer o teste passar:

```csharp
public class User
{
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public User(string email, string password, string firstName, string lastName)
    {
        Email = new Email(email);
        Password = new Password(password);
        FirstName = firstName;
        LastName = lastName;
    }
}
```

**Resultado**: ✅ Passa! O teste agora funciona.

#### 3️⃣ **REFACTOR** - Melhorar o Código
Finalmente, melhoramos o código mantendo os testes passando:

```csharp
public class User
{
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public User(string email, string password, string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Nome não pode ser vazio", nameof(firstName));
            
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Sobrenome não pode ser vazio", nameof(lastName));

        Email = new Email(email);
        Password = new Password(password);
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }
}
```

**Resultado**: ✅ Testes continuam passando, mas o código está melhor!

### 💡 Por que TDD é poderoso?

1. **Força você a pensar na interface primeiro**
2. **Garante que seu código é testável**
3. **Documenta o comportamento esperado**
4. **Reduz bugs desde o início**
5. **Facilita refatoração**

---

## 🏛️ Clean Architecture

### 🎯 O que é Clean Architecture?

Clean Architecture é uma forma de organizar seu código em **camadas bem definidas**, onde cada camada tem uma responsabilidade específica e as dependências apontam sempre para o centro (domínio).

### 🏗️ Camadas da Arquitetura

```
┌─────────────────────────────────────────────────────────────┐
│                    API (Controllers)                        │
├─────────────────────────────────────────────────────────────┤
│                  Application (Use Cases)                    │
├─────────────────────────────────────────────────────────────┤
│                    Domain (Business Rules)                  │
├─────────────────────────────────────────────────────────────┤
│                Infrastructure (External)                    │
└─────────────────────────────────────────────────────────────┘
```

### 📁 Estrutura do Projeto

```
SignUpApi/
├── src/
│   ├── SignUpApi.Domain/           # 🎯 Regras de Negócio
│   │   ├── Entities/               # Entidades (User)
│   │   ├── Interfaces/             # Contratos (IUserRepository)
│   │   ├── ValueObjects/           # Objetos de Valor (Email, Password)
│   │   └── Exceptions/             # Exceções de Domínio
│   │
│   ├── SignUpApi.Application/      # 🔧 Casos de Uso
│   │   ├── Commands/               # Comandos (SignUpCommand)
│   │   ├── Queries/                # Consultas (GetUsersQuery)
│   │   ├── Handlers/               # Manipuladores
│   │   ├── Validators/             # Validações
│   │   └── DTOs/                   # Objetos de Transferência
│   │
│   ├── SignUpApi.Infrastructure/   # 🗄️ Implementações
│   │   ├── Data/                   # Entity Framework
│   │   ├── Repositories/           # Repositórios
│   │   └── Services/               # Serviços Externos
│   │
│   └── SignUpApi.API/              # 🌐 Controllers
│       └── Controllers/            # Endpoints da API
│
└── tests/                          # 🧪 Testes
    └── SignUpApi.Tests/
        ├── Domain/                  # Testes do Domínio
        ├── Application/             # Testes da Aplicação
        └── Infrastructure/          # Testes da Infraestrutura
```

### 🔄 Fluxo de Dependências

```
API → Application → Domain ← Infrastructure
```

**Regra**: As dependências sempre apontam para o **DOMÍNIO** (centro).

---

## 🧹 Clean Code

### 🎯 O que é Clean Code?

Clean Code é código que é **fácil de ler, entender e manter**. É como escrever um livro - deve ser claro para qualquer desenvolvedor.

### 📝 Princípios do Clean Code

#### 1️⃣ **Nomes Significativos**

❌ **Ruim**:
```csharp
public class Usr { }
public void Proc() { }
public string str { get; set; }
```

✅ **Bom**:
```csharp
public class User { }
public void ProcessUser() { }
public string Email { get; set; }
```

#### 2️⃣ **Funções Pequenas e Focadas**

❌ **Ruim**:
```csharp
public async Task<SignUpResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
{
    // 50 linhas de código fazendo várias coisas
    var user = await _userRepository.GetByEmailAsync(request.Email);
    if (user != null) return SignUpResult.Failure("Email já existe");
    
    var hashedPassword = _passwordHasher.HashPassword(request.Password);
    var newUser = new User(request.Email, hashedPassword, request.FirstName, request.LastName);
    
    await _userRepository.AddAsync(newUser);
    var token = _jwtTokenGenerator.GenerateToken(newUser);
    
    return SignUpResult.Success(new SignUpData { Token = token, User = MapToDto(newUser) });
}
```

✅ **Bom**:
```csharp
public async Task<SignUpResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
{
    if (await UserExistsAsync(request.Email))
        return SignUpResult.Failure("Email já existe");

    var user = await CreateUserAsync(request);
    var token = GenerateTokenAsync(user);
    
    return SignUpResult.Success(CreateSignUpData(user, token));
}

private async Task<bool> UserExistsAsync(string email)
{
    var user = await _userRepository.GetByEmailAsync(email);
    return user != null;
}

private async Task<User> CreateUserAsync(SignUpCommand request)
{
    var hashedPassword = _passwordHasher.HashPassword(request.Password);
    var user = new User(request.Email, hashedPassword, request.FirstName, request.LastName);
    return await _userRepository.AddAsync(user);
}
```

#### 3️⃣ **Comentários Desnecessários**

❌ **Ruim**:
```csharp
// Verifica se o usuário existe
if (user != null)
{
    // Retorna erro se existir
    return SignUpResult.Failure("Email já existe");
}
```

✅ **Bom**:
```csharp
if (user != null)
{
    return SignUpResult.Failure("Email já existe");
}
```

#### 4️⃣ **Tratamento de Erros Apropriado**

❌ **Ruim**:
```csharp
public User(string email, string password, string firstName, string lastName)
{
    Email = email; // Pode ser inválido!
    Password = password; // Pode ser fraco!
    FirstName = firstName;
    LastName = lastName;
}
```

✅ **Bom**:
```csharp
public User(string email, string password, string firstName, string lastName)
{
    if (string.IsNullOrWhiteSpace(firstName))
        throw new ArgumentException("Nome não pode ser vazio", nameof(firstName));
        
    if (string.IsNullOrWhiteSpace(lastName))
        throw new ArgumentException("Sobrenome não pode ser vazio", nameof(lastName));

    Email = new Email(email); // Valida o email
    Password = new Password(password); // Valida a senha
    FirstName = firstName.Trim();
    LastName = lastName.Trim();
}
```

---

## 🔧 Princípios SOLID

### 🎯 O que são os princípios SOLID?

SOLID são 5 princípios de design que ajudam a criar código **manutenível, extensível e testável**.

### 1️⃣ **S - Single Responsibility Principle (SRP)**

> "Uma classe deve ter apenas uma razão para mudar"

❌ **Ruim**:
```csharp
public class UserService
{
    public void CreateUser() { }      // Cria usuário
    public void SendEmail() { }       // Envia email
    public void SaveToDatabase() { }  // Salva no banco
    public void ValidateData() { }    // Valida dados
}
```

✅ **Bom**:
```csharp
public class UserService
{
    public void CreateUser() { }      // Apenas cria usuário
}

public class EmailService
{
    public void SendEmail() { }       // Apenas envia emails
}

public class UserRepository
{
    public void Save() { }            // Apenas salva dados
}

public class UserValidator
{
    public void Validate() { }        // Apenas valida dados
}
```

### 2️⃣ **O - Open/Closed Principle (OCP)**

> "Aberto para extensão, fechado para modificação"

❌ **Ruim**:
```csharp
public class PasswordValidator
{
    public bool Validate(string password)
    {
        if (password.Length < 8) return false;
        if (!password.Any(char.IsUpper)) return false;
        if (!password.Any(char.IsLower)) return false;
        if (!password.Any(char.IsDigit)) return false;
        return true;
    }
}
```

✅ **Bom**:
```csharp
public interface IPasswordRule
{
    bool IsValid(string password);
}

public class MinimumLengthRule : IPasswordRule
{
    public bool IsValid(string password) => password.Length >= 8;
}

public class UppercaseRule : IPasswordRule
{
    public bool IsValid(string password) => password.Any(char.IsUpper);
}

public class PasswordValidator
{
    private readonly List<IPasswordRule> _rules;

    public PasswordValidator()
    {
        _rules = new List<IPasswordRule>
        {
            new MinimumLengthRule(),
            new UppercaseRule(),
            new LowercaseRule(),
            new DigitRule()
        };
    }

    public bool Validate(string password)
    {
        return _rules.All(rule => rule.IsValid(password));
    }
}
```

### 3️⃣ **L - Liskov Substitution Principle (LSP)**

> "Implementações devem ser substituíveis por suas abstrações"

✅ **Bom**:
```csharp
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
}

public class SqlServerUserRepository : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id) { /* SQL Server */ }
    public async Task<User> AddAsync(User user) { /* SQL Server */ }
}

public class InMemoryUserRepository : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id) { /* Memória */ }
    public async Task<User> AddAsync(User user) { /* Memória */ }
}

// Ambos podem ser usados da mesma forma:
IUserRepository repository = new SqlServerUserRepository(); // ou InMemoryUserRepository
var user = await repository.GetByIdAsync(id);
```

### 4️⃣ **I - Interface Segregation Principle (ISP)**

> "Interfaces específicas são melhores que interfaces genéricas"

❌ **Ruim**:
```csharp
public interface IUserService
{
    void CreateUser();
    void UpdateUser();
    void DeleteUser();
    void SendEmail();
    void GenerateReport();
    void BackupData();
}
```

✅ **Bom**:
```csharp
public interface IUserRepository
{
    void CreateUser();
    void UpdateUser();
    void DeleteUser();
}

public interface IEmailService
{
    void SendEmail();
}

public interface IReportService
{
    void GenerateReport();
}

public interface IBackupService
{
    void BackupData();
}
```

### 5️⃣ **D - Dependency Inversion Principle (DIP)**

> "Dependa de abstrações, não de implementações concretas"

❌ **Ruim**:
```csharp
public class UserService
{
    private readonly SqlServerUserRepository _repository; // Depende de implementação específica
    
    public UserService()
    {
        _repository = new SqlServerUserRepository(); // Criação hardcoded
    }
}
```

✅ **Bom**:
```csharp
public class UserService
{
    private readonly IUserRepository _repository; // Depende de abstração
    
    public UserService(IUserRepository repository) // Injeção de dependência
    {
        _repository = repository;
    }
}

// Configuração no Program.cs
builder.Services.AddScoped<IUserRepository, SqlServerUserRepository>();
// ou
builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();
```

---

## 🚀 CQRS - Command Query Responsibility Segregation

### 🎯 O que é CQRS?

CQRS separa **comandos** (que modificam dados) de **consultas** (que apenas leem dados).

### 📝 Comandos (Commands)

Comandos **modificam** o estado da aplicação:

```csharp
public class SignUpCommand : IRequest<SignUpResult>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResult>
{
    public async Task<SignUpResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        // Lógica para criar usuário
        var user = new User(request.Email, request.Password, request.FirstName, request.LastName);
        await _repository.AddAsync(user);
        
        return SignUpResult.Success(user);
    }
}
```

### 🔍 Consultas (Queries)

Consultas **apenas leem** dados:

```csharp
public class GetUsersQuery : IRequest<List<UserDto>>
{
    public bool ActiveOnly { get; set; } = false;
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync();
        
        if (request.ActiveOnly)
            users = users.Where(u => u.IsActive).ToList();
            
        return users.Select(u => new UserDto { /* mapeamento */ }).ToList();
    }
}
```

### 💡 Vantagens do CQRS

1. **Separação clara** de responsabilidades
2. **Otimizações diferentes** para leitura e escrita
3. **Escalabilidade** independente
4. **Testabilidade** melhorada

---

## 🎯 Value Objects

### 🎯 O que são Value Objects?

Value Objects são objetos que representam **conceitos do domínio** e são **imutáveis**. Eles encapsulam validações e regras de negócio.

### 📧 Exemplo: Email

```csharp
public class Email
{
    public string Value { get; }
    
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email não pode ser vazio", nameof(value));
            
        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("Formato de email inválido", nameof(value));
            
        Value = value.Trim().ToLowerInvariant();
    }

    public static implicit operator string(Email email) => email.Value;
    
    public override bool Equals(object? obj)
    {
        if (obj is Email other)
            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        return false;
    }
    
    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
}
```

### 🔐 Exemplo: Password

```csharp
public class Password
{
    public string Value { get; }
    
    private static readonly Regex PasswordRegex = new(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        RegexOptions.Compiled);

    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Senha não pode ser vazia", nameof(value));
            
        if (value.Length < 8)
            throw new ArgumentException("Senha deve ter pelo menos 8 caracteres", nameof(value));
            
        if (!PasswordRegex.IsMatch(value))
            throw new ArgumentException("Senha deve conter pelo menos uma letra maiúscula, uma minúscula, um número e um caractere especial", nameof(value));
            
        Value = value;
    }
}
```

### 💡 Vantagens dos Value Objects

1. **Validação centralizada** - Regras em um só lugar
2. **Imutabilidade** - Dados não podem ser alterados incorretamente
3. **Reutilização** - Podem ser usados em várias entidades
4. **Testabilidade** - Fácil de testar isoladamente

---

## 🧪 Testes na Prática

### 📝 Testando Value Objects

```csharp
[Fact]
public void CreateEmail_WithValidEmail_ShouldCreateEmailSuccessfully()
{
    // Arrange
    var validEmail = "test@example.com";

    // Act
    var email = new Email(validEmail);

    // Assert
    email.Value.Should().Be(validEmail.ToLowerInvariant());
}

[Fact]
public void CreateEmail_WithInvalidEmail_ShouldThrowException()
{
    // Arrange
    var invalidEmail = "invalid-email";

    // Act & Assert
    var action = () => new Email(invalidEmail);
    action.Should().Throw<ArgumentException>()
        .WithMessage("*Formato de email inválido*");
}
```

### 📝 Testando Entidades

```csharp
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
    user.FirstName.Should().Be(firstName);
    user.LastName.Should().Be(lastName);
    user.IsActive.Should().BeTrue();
    user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
}
```

### 📝 Testando Handlers

```csharp
[Fact]
public async Task Handle_WithValidSignUpCommand_ShouldReturnSuccessResult()
{
    // Arrange
    var command = new SignUpCommand
    {
        Email = "test@example.com",
        Password = "SecurePassword123!",
        FirstName = "João",
        LastName = "Silva"
    };

    var user = new User(command.Email, command.Password, command.FirstName, command.LastName);
    var token = "jwt.token.here";

    _mockUserRepository
        .Setup(x => x.ExistsByEmailAsync(command.Email))
        .ReturnsAsync(false);

    _mockPasswordHasher
        .Setup(x => x.HashPassword(command.Password))
        .Returns("hashedPassword");

    _mockJwtTokenGenerator
        .Setup(x => x.GenerateToken(It.IsAny<User>()))
        .Returns(token);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();
    result.Data.Should().NotBeNull();
    result.Data!.Token.Should().Be(token);
}
```

---

## 🚀 Como Executar o Projeto

### 1️⃣ **Clone e Restaure**
```bash
git clone <url-do-repositorio>
cd SignUpApi
dotnet restore
```

### 2️⃣ **Execute os Testes**
```bash
dotnet test
```

### 3️⃣ **Execute a API**
```bash
dotnet run --project src/SignUpApi.API
```

### 4️⃣ **Acesse a Documentação**
- Swagger UI: `https://localhost:7001/swagger`
- API Base: `https://localhost:7001/api`

---

## 📚 Endpoints da API

### 🔐 Autenticação
- `POST /api/auth/signup` - Cadastro de usuário
- `POST /api/auth/login` - Login de usuário

### 👥 Usuários
- `GET /api/users` - Lista todos os usuários
- `GET /api/users?activeOnly=true` - Lista apenas usuários ativos

---

## 🎯 Próximos Passos

1. **Implementar autenticação JWT completa**
2. **Adicionar autorização baseada em roles**
3. **Implementar refresh tokens**
4. **Adicionar logging estruturado**
5. **Implementar cache distribuído**
6. **Adicionar métricas e monitoramento**

---

## 💡 Dicas para Aplicar na Prática

### 🧪 **TDD**
- Comece sempre com um teste simples
- Escreva o teste que você gostaria de ter
- Implemente o mínimo para passar
- Refatore com confiança

### 🏗️ **Clean Architecture**
- Pense nas dependências
- O domínio nunca deve depender de frameworks
- Use interfaces para abstrações
- Mantenha as camadas separadas

### 🧹 **Clean Code**
- Nomes descritivos
- Funções pequenas e focadas
- Evite comentários óbvios
- Trate erros apropriadamente

### 🔧 **SOLID**
- Uma classe, uma responsabilidade
- Abra para extensão, feche para modificação
- Substitua implementações
- Interfaces específicas
- Dependa de abstrações

---

## 🎉 Conclusão

Parabéns! Você agora tem uma base sólida em:

- ✅ **TDD** - Desenvolvimento guiado por testes
- ✅ **Clean Architecture** - Organização de código em camadas
- ✅ **Clean Code** - Código legível e manutenível
- ✅ **SOLID** - Princípios de design de software
- ✅ **CQRS** - Separação de comandos e consultas
- ✅ **Value Objects** - Objetos imutáveis e seguros

Continue praticando e aplicando esses conceitos em seus projetos. Lembre-se: **a prática leva à perfeição**! 🚀

---

**💡 Dica**: Este projeto é um exemplo educacional. Para uso em produção, considere adicionar mais validações de segurança, logging, monitoramento e testes de carga.
