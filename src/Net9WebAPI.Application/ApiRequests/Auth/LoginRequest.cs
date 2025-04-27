using FluentValidation;
using Net9WebAPI.Application.Abstract;
using FluentValidation.Results;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

public class LoginRequest : IApiRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public class Validator : AbstractValidator<LoginRequest>
    {
        public Validator()
        {
            RuleFor(request => request.Username)
                .NotEmpty()
                .WithMessage("Username is required.");

            RuleFor(request => request.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}

public class LoginRequestHandler : IApiRequestHandler<LoginRequest>
{
    private readonly IConfiguration _configuration;
    private readonly IValidator<LoginRequest> _validator;

    public LoginRequestHandler(IValidator<LoginRequest> validator, IConfiguration configuration)
    {
        _configuration = configuration;
        _validator = validator;
    }

    public async Task<IApiResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationProblemApiResult(validationResult);
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        string secretKey = _configuration["JwtBearerSecretKey"] ?? "default_secret_key";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourIssuer",
            audience: "yourAudience",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        LoginResponseDto dto = new LoginResponseDto()
        {
            Token = tokenString
        };

        return new ApiContentResult<LoginResponseDto>(dto);
    }
}