using TextBoard.ApplicationServices.Messaging.Requests;
using TextBoard.ApplicationServices.Messaging.Responses;

namespace TextBoard.ApplicationServices.Interfaces;

public interface IMessagesService
{
    // List messages
    public Task<ListBoardMessagesResponse> ListBoardMessagesAsync(ListBoardMessagesRequest request);

    // Get messages
    public Task<GetMessageResponse> GetMessageByIdAsync(GetMessageByIdRequest request);

    // Create message
    public Task<CreateMessageResponse> CreateMessageAsync(CreateMessageRequest request);

    // Modify message
    public Task<ReplaceMessageResponse> ReplaceMessageAsync(ReplaceMessageRequest request);
    // messages follow a blockchain approach to editing - a message may replace another one.

    // Delete message
    public Task<DeleteMessageResponse> DeleteMessageAsync(DeleteMessageRequest request);
}

