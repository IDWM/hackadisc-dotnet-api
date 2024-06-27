using hackadisc_dotnet_api.DTOs;
using hackadisc_dotnet_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowLocalhost",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});
var app = builder.Build();
app.UseCors("AllowLocalhost");

var api = app.MapGroup("/api/member");

api.MapPost("", CreateMemberAsync);
api.MapGet("", GetMembersAsync);
api.MapPut("/{id}", UpdateMemberAsync);
api.MapGet("/{id}", GetMemberAsync);

app.Run();

static async Task<IResult> CreateMemberAsync(
    [FromBody] CreateMemberDto createMemberDto,
    DataContext dataContext
)
{
    ErrorMessageDto errorMessageDto;

    if (await dataContext.Members.AnyAsync(m => m.Email == createMemberDto.Email))
    {
        errorMessageDto = new()
        {
            Error = $"El correo electrónico {createMemberDto.Email} ya está registrado."
        };
        return TypedResults.BadRequest(errorMessageDto);
    }

    var member = new Member
    {
        Name = createMemberDto.Name,
        Email = createMemberDto.Email,
        Semester = createMemberDto.Semester,
        Career = createMemberDto.Career
    };

    await dataContext.Members.AddAsync(member);

    if (0 < await dataContext.SaveChangesAsync())
    {
        return TypedResults.Created($"/api/member/{member.Id}", member);
    }

    errorMessageDto = new() { Error = "Error en el servidor." };
    return TypedResults.BadRequest(errorMessageDto);
}

static async Task<IResult> GetMembersAsync(DataContext dataContext)
{
    var members = await dataContext.Members.ToListAsync();

    return TypedResults.Ok(members);
}

static async Task<IResult> GetMemberAsync(DataContext dataContext, [FromRoute] int id)
{
    var member = await dataContext.Members.FindAsync(id);

    if (member == null)
    {
        ErrorMessageDto errorMessageDto = new() { Error = $"Integrante no encontrado." };
        return Results.NotFound(errorMessageDto);
    }

    return TypedResults.Ok(member);
}

static async Task<IResult> UpdateMemberAsync(
    [FromRoute] int id,
    [FromBody] UpdateMemberDto updateMemberDto,
    DataContext dataContext
)
{
    ErrorMessageDto errorMessageDto;
    var member = await dataContext.Members.FindAsync(id);

    if (member == null)
    {
        errorMessageDto = new() { Error = $"Integrante no encontrado." };
        return Results.NotFound(errorMessageDto);
    }
    else if (
        await dataContext.Members.AnyAsync(m => m.Email == updateMemberDto.Email && m.Id != id)
    )
    {
        errorMessageDto = new()
        {
            Error = $"El correo electrónico {updateMemberDto.Email} ya está registrado."
        };
        return TypedResults.BadRequest(errorMessageDto);
    }

    member.Name = updateMemberDto.Name;
    member.Email = updateMemberDto.Email;
    member.Semester = updateMemberDto.Semester;
    member.Career = updateMemberDto.Career;

    if (0 < await dataContext.SaveChangesAsync())
    {
        return TypedResults.Ok();
    }

    errorMessageDto = new() { Error = "Error en el servidor." };
    return TypedResults.BadRequest(errorMessageDto);
}
