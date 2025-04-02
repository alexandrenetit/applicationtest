namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Provides way to notify events.
/// </summary>
public interface IEventNotification
{
    /// <summary>
    /// Send event notification.
    /// </summary>
    /// <typeparam name="T">Event type to send</typeparam>
    /// <param name="messageEvent"></param>
    /// <returns></returns>
    Task NotifyAsync<T>(T messageEvent);
}