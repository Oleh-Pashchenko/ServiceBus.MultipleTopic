//-----------------------------------------------------------------------
// <copyright file="Topic.cs" company="MultipleTopic">
//     Copyright (c) MultipleTopic All rights reserved.
// </copyright>
// <author>Oleh Pashchenko</author>
//-----------------------------------------------------------------------

namespace ServiceBus.MultipleTopic.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using ServiceBus.MultipleTopic.Interfaces;

    /// <summary>
    /// Implementation of ITopic
    /// </summary>
    internal sealed class Topic : ITopic
    {
        /// <summary>
        /// Dictionary of SubscriptionClient
        /// </summary>
        private Dictionary<string, SubscriptionClient> subscriptions = new Dictionary<string, SubscriptionClient>();

        /// <summary>
        /// Connection string to the Service Bus
        /// </summary>
        private string connectionString;

        /// <summary>
        /// Topic path/name
        /// </summary>
        private string topicPath;

        /// <summary>
        /// Initializes a new instance of the Topic class.
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="topicPath"> Topic path</param>
        public Topic(string connectionString, string topicPath)
        {
            this.connectionString = connectionString;
            this.topicPath = topicPath;

            TopicClient = TopicClient.CreateFromConnectionString(this.connectionString, this.topicPath);
        }

        /// <summary>
        /// Gets Topic client
        /// </summary>
        public TopicClient TopicClient { get; private set; }

        /// <summary>
        /// Instance of NamespaceManager
        /// </summary>
        private NamespaceManager NamespaceManager => NamespaceManager.CreateFromConnectionString(this.connectionString);

        /// <summary>
        /// Gets SubscriptionClient from dictionary
        /// </summary>
        /// <param name="key">SubscriptionClient key</param>
        /// <returns>Subscription client</returns>
        public SubscriptionClient this[string key]
        {
            get
            {
                return this.subscriptions[key];
            }
        }

        /// <summary>
        /// Create subscription 
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        public void CreateSubscription(string subscriptionName)
        {
            this.CreateSubscription(subscriptionName, "1=1");
        }

        /// <summary>
        /// Create subscription
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <param name="filter">Subscription filter</param>
        public void CreateSubscription(string subscriptionName, string filter)
        {
            if (!this.NamespaceManager.SubscriptionExists(this.topicPath, subscriptionName))
            {
                this.NamespaceManager.CreateSubscription(this.topicPath, subscriptionName, new SqlFilter(filter));
            }

            this.subscriptions.Add(subscriptionName, this.CreateSubscriptionClient(subscriptionName));
        }

        /// <summary>
        /// Create new SubscriptionClient
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <returns>Subscription Client</returns>
        public SubscriptionClient CreateSubscriptionClient(string subscriptionName)
        {
            return SubscriptionClient.CreateFromConnectionString(this.connectionString, this.topicPath, subscriptionName);
        }

        /// <summary>
        /// Asynchronously create subscription
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <returns>Async void</returns>
        public async Task CreateSubscriptionAsync(string subscriptionName)
        {
            await this.CreateSubscriptionAsync(subscriptionName, "1=1");
        }

        /// <summary>
        /// Asynchronously create subscription
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <param name="filter">Subscription filter</param>
        /// <returns>Async void</returns>
        public async Task CreateSubscriptionAsync(string subscriptionName, string filter)
        {
            if (!await this.NamespaceManager.SubscriptionExistsAsync(this.topicPath, subscriptionName))
            {
                await this.NamespaceManager.CreateSubscriptionAsync(this.topicPath, subscriptionName, new SqlFilter(filter));
            }

            this.subscriptions.Add(subscriptionName, this.CreateSubscriptionClient(subscriptionName));
        }

        /// <summary>
        /// Delete subscription from topic and dictionary
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <returns>True if successfully deleted</returns>
        public bool DeleteSubscription(string subscriptionName)
        {
            if (!this.NamespaceManager.SubscriptionExists(this.topicPath, subscriptionName))
            {
                this.NamespaceManager.DeleteSubscription(this.topicPath, subscriptionName);
                return this.subscriptions.Remove(subscriptionName);
            }

            return false;
        }

        /// <summary>
        /// Asynchronously delete subscription from topic and dictionary
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <returns>True if successfully deleted</returns>
        public async Task<bool> DeleteSubscriptionAsync(string subscriptionName)
        {
            if (!await this.NamespaceManager.SubscriptionExistsAsync(this.topicPath, subscriptionName))
            {
                await this.NamespaceManager.DeleteSubscriptionAsync(this.topicPath, subscriptionName);
                return this.subscriptions.Remove(subscriptionName);
            }

            return false;
        }

        /// <summary>
        /// Close all subscriptions of the topic
        /// </summary>
        public void CloseSubscriptions()
        {
            foreach (var item in this.subscriptions)
            {
                this.CloseSubscription(item.Key);
            }
        }

        /// <summary>
        /// Asynchronously close all subscriptions of the topic
        /// </summary>
        /// <returns>Async void</returns>
        public async Task CloseSubscriptionsAsync()
        {
            foreach (var item in this.subscriptions)
            {
                await this.CloseSubscriptionAsync(item.Key);
            }
        }

        /// <summary>
        /// Close subscription by name
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        public void CloseSubscription(string subscriptionName)
        {
            if (this.subscriptions[subscriptionName] != null)
            {
                if (!this.subscriptions[subscriptionName].IsClosed)
                {
                    this.subscriptions[subscriptionName]?.Close();
                }
            }
        }

        /// <summary>
        /// Asynchronously c by lose subscription by name
        /// </summary>
        /// <param name="subscriptionName">Subscription name</param>
        /// <returns>Async void</returns>
        public async Task CloseSubscriptionAsync(string subscriptionName)
        {
            if (this.subscriptions[subscriptionName] != null)
            {
                if (!this.subscriptions[subscriptionName].IsClosed)
                {
                    await this.subscriptions[subscriptionName]?.CloseAsync();
                }
            }
        }
    }
}