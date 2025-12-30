using IRCloudBackend.Infrastructure.Identity;

namespace IRCloudBackend.Infrastructure.DTO.Post;

public static class PostMappingExtensions
{
    public static SavedPostDTO ToSavedPostDTO(this Domain.Models.Post post, ) =>
        new SavedPostDTO
        {
            Id = post.Id,
            AuthorId = post.AuthorId,
            CreatedAt = post.CreatedAt,
            Description = post.Description,
            Title = post.Title
        };

    public static PostDTO ToDto(this Domain.Models.Post post, ApplicationUser applicationUser) =>
        new PostDTO
        {
            CreatedAt = post.CreatedAt,
            AuthorUsername = applicationUser.UserName,
            Categories = post.
        };

    //public static List<PostCategoryDTO> ToPostCategoryDto(this Domain.Models.Category category)
    //{
    //    while(category.)
    //}
}
