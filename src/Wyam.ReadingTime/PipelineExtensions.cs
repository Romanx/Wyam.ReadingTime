using System;
using Wyam.Common.Execution;
using Wyam.Common.Modules;

namespace Wyam.ReadingTime
{
    /// <summary>
    /// Extensions for the pipeline collections to make it easy to integrate into common pipelines
    /// </summary>
    public static class PipelineExtensions
    {
        private const string BlogPosts = nameof(BlogPosts);
        private const string WriteMetadata = nameof(WriteMetadata);

        /// <summary>
        /// A helper for adding reading time meta data to blog post pipelines.
        /// </summary>
        /// <param name="pipelineCollection"></param>
        /// <param name="wordsPerMinute"></param>
        /// <returns></returns>
        public static IPipelineCollection AddReadingTimeMeta(this IPipelineCollection pipelineCollection, uint wordsPerMinute) 
        {
            if (!pipelineCollection.TryGetValue(BlogPosts, out var blogPipeline))
            {
                throw new Exception("Reading time meta can currently only be added to a blog template with this helper!");
            }

            blogPipeline.InsertBefore(WriteMetadata, new ReadingTime(wordsPerMinute));

            return pipelineCollection;
        }
    }
}