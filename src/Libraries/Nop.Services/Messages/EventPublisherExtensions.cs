using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Core.Events;

namespace Nop.Services.Messages;

/// <summary>
/// Event publisher extensions
/// </summary>
public static class EventPublisherExtensions
{
    //NEWSLETTER FEATURE REMOVED
    //Removed: PublishNewsLetterSubscribeAsync, PublishNewsLetterUnsubscribeAsync

    /// <summary>
    /// Entity tokens added
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="eventPublisher">Event publisher</param>
    /// <param name="entity">Entity</param>
    /// <param name="tokens">Tokens</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public static async Task EntityTokensAddedAsync<T>(this IEventPublisher eventPublisher, T entity, IList<Token> tokens) where T : BaseEntity
    {
        await eventPublisher.PublishAsync(new EntityTokensAddedEvent<T>(entity, tokens));
    }

    /// <summary>
    /// Message token added
    /// </summary>
    /// <param name="eventPublisher">Event publisher</param>
    /// <param name="message">Message</param>
    /// <param name="tokens">Tokens</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public static async Task MessageTokensAddedAsync(this IEventPublisher eventPublisher, MessageTemplate message, IList<Token> tokens)
    {
        await eventPublisher.PublishAsync(new MessageTokensAddedEvent(message, tokens));
    }
}