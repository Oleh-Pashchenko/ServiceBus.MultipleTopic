//-----------------------------------------------------------------------
// <copyright file="MultipleTopic.cs" company="MultipleTopic">
//     Copyright (c)  MultipleTopic All rights reserved.
// </copyright>
// <author>Oleh Pashchenko</author>
//-----------------------------------------------------------------------

namespace ServiceBus.MultipleTopic.Implementation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus;
    using ServiceBus.MultipleTopic.Interfaces;

    /// <summary>
    /// Implementation of IMultipleTopic
    /// </summary>
    public sealed class MultipleTopic : IMultipleTopic
    {
        /// <summary>
        /// Dictionary of topics
        /// </summary>
        private Dictionary<string, ITopic> topics = new Dictionary<string, ITopic>();

        /// <summary>
        /// Connection string to Service Bus
        /// </summary>
        private string connectionString;

        /// <summary>
        /// Initializes a new instance of the MultipleTopic class.
        /// </summary>
        /// <param name="connectionString">connection string to Service Bus</param>
        public MultipleTopic(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Instance of NamespaceManager
        /// </summary>
        private NamespaceManager NamespaceManager => NamespaceManager.CreateFromConnectionString(this.connectionString);

        /// <summary>
        /// Gets topic from dictionary
        /// </summary>
        /// <param name="key">key of topic</param>
        /// <returns>Topic from dictionary</returns>
        public ITopic this[string key]
        {
            get
            {
                return this.topics[key];
            }
        }
        
        /// <summary>
        /// Create new topic in Service Bus and dictionary
        /// </summary>
        /// <param name="topicPath">Name of topic and key to dictionary</param>
        public void CreateTopic(string topicPath)
        {
            if (!this.NamespaceManager.TopicExists(topicPath))
            {
                this.NamespaceManager.CreateTopic(topicPath);
            }

            this.topics.Add(topicPath, this.CreateTopicClient(topicPath));
        }

        /// <summary>
        /// Asynchronously create topic
        /// </summary>
        /// <param name="topicPath">Topic name and key to dictionary</param>
        /// <returns>Return async void</returns>
        public async Task CreateTopicAsync(string topicPath)
        {
            if (!await this.NamespaceManager.TopicExistsAsync(topicPath))
            {
                await this.NamespaceManager.CreateTopicAsync(topicPath);
            }

            this.topics.Add(topicPath, this.CreateTopicClient(topicPath));
        }

        /// <summary>
        /// Delete topic from Service Bus and dictionary
        /// </summary>
        /// <param name="topicPath">Topic name and key to dictionary</param>
        /// <returns>True if successfully delete from Service Bus</returns>
        public bool DeleteTopic(string topicPath)
        {
            if (!this.NamespaceManager.TopicExists(topicPath))
            {
                this.NamespaceManager.DeleteTopic(topicPath);
                return this.topics.Remove(topicPath);
            }

            return false;
        }

        /// <summary>
        /// Asynchronously delete topic
        /// </summary>
        /// <param name="topicPath">Topic name</param>
        /// <returns>True if successfully delete from Service Bus</returns>
        public async Task<bool> DeleteTopicAsync(string topicPath)
        {
            if (!await this.NamespaceManager.TopicExistsAsync(topicPath))
            {
                await this.NamespaceManager.DeleteTopicAsync(topicPath);
                return this.topics.Remove(topicPath);
            }

            return false;
        }

        /// <summary>
        /// Create new TopicClient
        /// </summary>
        /// <param name="topicPath">Topic name</param>
        /// <returns>Topic Client</returns>
        private ITopic CreateTopicClient(string topicPath)
        {
            return new Topic(this.connectionString, topicPath);
        }
    }
}