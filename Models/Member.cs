namespace hackadisc_dotnet_api.Models;

public class Member
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required int Semester { get; set; }
    public required string Career { get; set; }
}
