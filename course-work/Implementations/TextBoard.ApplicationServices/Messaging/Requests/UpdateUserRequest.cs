namespace TextBoard.ApplicationServices.Messaging.Requests;

public class UpdateUserRequest
{
    public int Id { get; set; }

    public string? Name { get; set; } = null;
    public string? Password { get; set; } = null;

    public UpdateUserRequest(int id, string? name = null, string? pwd = null)
    {
        Id = id;
        Name = name;
        Password = pwd;
    }
}
