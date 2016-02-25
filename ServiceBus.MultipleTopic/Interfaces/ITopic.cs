//-----------------------------------------------------------------------
// <copyright file="ITopic.cs" company="MultipleTopic">
//     Copyright (c) MultipleTopic All rights reserved.
// </copyright>
// <author>Oleh Pashchenko</author>
//-----------------------------------------------------------------------

namespace ServiceBus.MultipleTopic.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// This interface describes the methods, properties, indexer and fields for working with topic
    /// </summary>
    public interface ITopic
    {
        /// <summary>
        /// Gets Topic client
        /// </summary>
        TopicClient TopicClient { get; }

        /// <summary>
        /// Gets SubscriptionClient from dictionary
        /// </summary>
        /// <param name="key">SubscriptionClient key</param>
        /// <returns>Subscription client</returns>
        SubscriptionClient this[string key] { get; }

        /// <summary>
        /// Create new subscription
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        void CreateSubscription(string subscriptionName);

        /// <summary>
        /// Create new SubscriptionClient
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <returns>Subscription Client</returns>
        SubscriptionClient CreateSubscriptionClient(string subscriptionName);

        /// <summary>
        /// Create new subscriptions
        /// </summary>
        /// <param name="subscriptionName">Subscriptions name</param>
        /// <param name="filter">Filter for subscription</param>
        void CreateSubscription(string subscriptionName, string filter);

        /// <summary>
        /// Delete subscription
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <returns>True if successfully delete</returns>
        bool DeleteSubscription(string subscriptionName);

        /// <summary>
        /// Close all subscriptions of this topic
        /// </summary>
        void CloseSubscriptions();

        /// <summary>
        /// Asynchronously close all subscriptions of the topic
        /// </summary>
        /// <returns>Async void</returns>
        Task CloseSubscriptionsAsync();

        /// <summary>
        /// Close  subscription of this topic
        /// </summary>
        /// <param name="subscriptionName">Subscriptions name</param>
        void CloseSubscription(string subscriptionName);

        /// <summary>
        /// Asynchronously close subscription of the topic
        /// </summary>
        /// <param name="subscriptionName">Subscriptions name</param>
        /// <returns>Async void</returns>
        Task CloseSubscriptionAsync(string subscriptionName);

        /// <summary>
        /// Asynchronously create new subscriptions
        /// </summary>
        /// <param name="subscriptionName">Subscriptions name</param>
        /// <returns>Return async void</returns>
        Task CreateSubscriptionAsync(string subscriptionName);

        /// <summary>
        /// Asynchronously create new subscriptions
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <param name="filter">Filter for subscription</param>
        /// <returns>Return async void</returns>
        Task CreateSubscriptionAsync(string subscriptionName, string filter);

        /// <summary>
        /// Asynchronously delete subscription
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <returns>True if successfully delete</returns>
        Task<bool> DeleteSubscriptionAsync(string subscriptionName);
    }
}