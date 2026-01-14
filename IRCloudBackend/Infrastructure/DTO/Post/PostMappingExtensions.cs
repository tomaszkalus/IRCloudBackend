using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.Identity;

using Microsoft.CodeAnalysis.CSharp.Syntax;

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

    //public static PostReviewDTO ToPostReviewDto(this Review review) =>
    //    new PostReviewDTO
    //    {
    //          AuthorUsername = review.Author
    //    }

    //public static List<PostCategoryDTO> ToPostCategoryDto(this Domain.Models.Category category)
    //{
    //    while(category.)
    //}
}
