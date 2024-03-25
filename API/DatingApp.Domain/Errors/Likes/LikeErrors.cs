using DatingApp.Domain.Common.Errors;

namespace DatingApp.Domain.Errors.Likes
{
    public static class LikeErrors
    {
        public static Error TargetUserNotFound(string username) 
            => new Error("Like.TargetUserNotFound", $"The target user with username {username} not found.");

        public static Error TargetLikeUserIsSelf 
            = new("Like.TargetUserIsSelf", "You cannot like yourself.");

        public static Error TargetUserAlreadyLiked(string username)
            => new("Like.TargetUserAlreadyLiked", $"You already like the user {username}.");

        public static Error LikeFailed(string username) 
            => new("Like.LikeFailed", $"Failed to like user with username {username}.");

        public static Error TargetDislikeUserIsSelf 
            = new("Like.TargetDislikeUserIsSelf", "You cannot dislike yourself.");

        public static Error TargetUserHasNotBeenLikedBeforeDislike 
            = new("Like.TargetUserHasNotBeenLikedBeforeDislike", "You cannot dislike someone you haven't liked before.");

        public static Error DislikeFailed(string username)
            => new("Like.DislikeFailed", $"Failed to dislike user with username {username}.");
    }
}
