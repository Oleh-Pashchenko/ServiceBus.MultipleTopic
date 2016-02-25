//-----------------------------------------------------------------------
// <copyright file="IMultipleTopic.cs" company="MultipleTopic">
//     Copyright (c) MultipleTopic All rights reserved.
// </copyright>
// <author>Oleh Pashchenko</author>
//-----------------------------------------------------------------------

namespace ServiceBus.MultipleTopic.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// This interface describes the methods and indexer for working with topic
    /// </summary>
    public interface IMultipleTopic
    {
        /// <summary>
        /// Return topic from dictionary
        /// </summary>
        /// <param name="key"> Key of dictionary</param>
        /// <returns>Return topic</returns>
        ITopic this[string key] { get; }

        /// <summary>
        /// Create new topic in Service Bus and dictionary
        /// </summary>
        /// <param name="topicPath">Name of topic and key to dictionary</param>
        void CreateTopic(string topicPath);

        /// <summary>
        /// Delete topic from Service Bus and dictionary
        /// </summary>
        /// <param name="topicPath">Topic name and key to dictionary</param>
        /// <returns>True if successfully delete from Service Bus</returns>
        bool DeleteTopic(string topicPath);

        /// <summary>
        /// Close topic by path/name
        /// </summary>
        /// <param name="topicPath">Topic path/name</param>
        void CloseTopic(string topicPath);

        /// <summary>
        /// Close all topics
        /// </summary>
        void CloseTopics();

        /// <summary>
        /// Asynchronously close topic
        /// </summary>
        /// <param name="topicPath">Topic path/name</param>
        /// <returns>Async void</returns>
        Task CloseTopicAsync(string topicPath);

        /// <summary>
        /// Asynchronously close topics
        /// </summary>
        /// <returns>Async void</returns>
        Task CloseTopicsAsync();

        /// <summary>
        /// Asynchronously create topic
        /// </summary>
        /// <param name="topicPath">Topic name and key to dictionary</param>
        /// <returns>Return async void</returns>
        Task CreateTopicAsync(string topicPath);

        /// <summary>
        /// Asynchronously delete topic
        /// </summary>
        /// <param name="topicPath">Topic name</param>
        /// <returns>True if successfully delete from Service Bus</returns>
        Task<bool> DeleteTopicAsync(string topicPath);
    }
}