namespace UIM.Core.Controllers.Shared;

[Route("api/[controller]")]
public class IdeaController : SharedController<IIdeaService>
{
    private readonly IJwtService _jwtService;

    public IdeaController(IIdeaService ideaService, IJwtService jwtService) : base(ideaService)
    {
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIdeaRequest request)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.UserId = userId;
        request.SubmissionId = EncryptHelpers.DecodeBase64Url(request.SubmissionId);

        var response = await _service.CreateAsync(request);
        return ResponseResult(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        var entityId = EncryptHelpers.DecodeBase64Url(id);

        await _service.RemoveAsync(userId, entityId);
        return ResponseResult();
    }

    [HttpGet("table/list")]
    public async Task<IActionResult> Read([FromQuery] SieveModel request, string? submissionId)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var decodedSubmissionId = EncryptHelpers.DecodeBase64Url(submissionId);
        var result = await _service.FindAsync(decodedSubmissionId, request);
        return ResponseResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Read(string id)
    {
        var entityId = EncryptHelpers.DecodeBase64Url(id);
        var result = await _service.FindByIdAsync(entityId);
        return ResponseResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateIdeaRequest request, string id)
    {
        if (request == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?
            .Split(" ")
            .Last();

        var userId = _jwtService.Validate(token);
        if (userId == null)
            throw new HttpException(HttpStatusCode.Unauthorized);

        request.Id = EncryptHelpers.DecodeBase64Url(id);
        request.SubmissionId = EncryptHelpers.DecodeBase64Url(request.SubmissionId);
        request.UserId = userId;

        await _service.EditAsync(request);
        return ResponseResult();
    }
}
