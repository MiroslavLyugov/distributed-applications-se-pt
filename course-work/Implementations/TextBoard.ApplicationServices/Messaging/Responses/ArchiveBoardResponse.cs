namespace TextBoard.ApplicationServices.Messaging.Responses;

public class ArchiveBoardResponse : ResponseBase
{
    public int Id { get; set; }

    public ArchiveBoardResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }

    public ArchiveBoardResponse(int id)
        : base(BusinessStatusCodeEnum.Success, "Board archived")
    {
        Id = id;
    }
}
