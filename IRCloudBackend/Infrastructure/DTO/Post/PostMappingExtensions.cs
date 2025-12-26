namespace IRCloudBackend.Infrastructure.DTO.Post;

public static class PostMappingExtensions
{
    public static SavedPostDTO ToSavedPostDTO(this Domain.Models.Post post) =>
        new SavedPostDTO
        {
            Id = post.Id,
            AuthorId = post.AuthorId,
            CreatedAt = post.CreatedAt,
            Description = post.Description,
            Title = post.Title
        };
}
