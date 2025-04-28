using FluentValidation;
using Net9WebAPI.Application.Abstract;
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

    public LoginRequestHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IApiResult> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await Task.Delay(10);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        string secretKey = _configuration["JwtBearerSecretKey"] ?? "default_secret_key";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "https://localhost:8001",
            audience: "Net9WebApi",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        LoginResultDto dto = new LoginResultDto()
        {
            Token = tokenString
        };

        return new ApiContentResult<LoginResultDto>(dto);
    }
}