namespace UIM.Core.Controllers;

[JwtAuthorize]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService, IJwtService jwtService)
    {
        _authService = authService;
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpGet("info")]
    public async Task<IActionResult> GetMeData()
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized,
                                    ErrorResponseMessages.Unauthorized);

        var user = await _userService.FindByIdAsync(userId);
        return Ok(new GenericResponse(user));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var response = await _authService.LoginAsync(request);
        return Ok(new GenericResponse(response));
    }

    [AllowAnonymous]
    [HttpPost("login-ex")]
    public async Task<IActionResult> LoginExternal([FromBody] ExternalAuthRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var response = await _authService.ExternalLoginAsync(request);
        return Ok(new GenericResponse(response));
    }

    [HttpPut("token/revoke")]
    public IActionResult Revoke(string refreshToken)
    {
        _authService.RevokeRefreshToken(refreshToken);
        return Ok(new GenericResponse(SuccessResponseMessages.TokenRevoked));
    }

    [HttpPut("token/rotate")]
    public async Task<IActionResult> Rotate([FromBody] RotateTokenRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var response = await _authService.RotateTokensAsync(request);
        return Ok(new GenericResponse(response));
    }
}