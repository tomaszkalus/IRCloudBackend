using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.Identity;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IRCloudBackend.Application.DTO.Post;

public static class PostMappingExtensions
{
    public static SavedPostDTO ToSavedPostDTO(this Domain.Models.Post post) =>
        new SavedPostDTO
        {
            Id = post.Id,
            AuthorName = post.Author.Username,
            AuthorId = post.AuthorId,
            CreatedAt = post.CreatedAt,
            Description = post.Description,
            Title = post.Title
        };
}
