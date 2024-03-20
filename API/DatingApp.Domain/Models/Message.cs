namespace DatingApp.Domain.Models;

public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
    public AppUser Sender { get; set; }
    public int RecipientId { get; set; }
    public string RecipientUsername { get; set; }
    public AppUser Recipient { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }

    public static Message Create(AppUser sender, AppUser recipient, string messageContent)
    {
        return new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = messageContent
        };
    }

    public void SetMessageAsDeleted(string currentUsername)
    {
        if (SenderUsername == currentUsername) SenderDeleted = true;

        if (RecipientUsername == currentUsername) RecipientDeleted = true;
    }

    public bool CanMessageBeFullyDeleted() => SenderDeleted && RecipientDeleted;

    public bool BelongsToUser(string currentUsername) => SenderUsername == currentUsername || RecipientUsername == currentUsername;

}
