using TextBoard.ApplicationServices.Messaging.Requests;
using TextBoard.ApplicationServices.Messaging.Responses;

namespace TextBoard.ApplicationServices.Interfaces;

public interface IBoardsService
{
    // List boards
    public Task<ListBoardsResponse> ListBoardsAsync(ListBoardsRequest request);
    public Task<ListUserBoardsResponse> ListUserBoardsAsync(ListUserBoardsRequest request); // list boards where User participated

    // Get boards
    public Task<GetBoardResponse> GetBoardByNameAsync(GetBoardByNameRequest request);
    public Task<GetBoardResponse> GetBoardByIdAsync(GetBoardByIdRequest request);

    // Create board
    public Task<CreateBoardResponse> CreateBoardAsync(CreateBoardRequest request);

    // Modify board
    public Task<UpdateBoardResponse> UpdateBoardAsync(UpdateBoardRequest request);
    public Task<ArchiveBoardResponse> ArchiveBoardAsync(ArchiveBoardRequest request);

    // Delete board
    public Task<DeleteBoardResponse> DeleteBoardAsync(DeleteBoardRequest request);
}

