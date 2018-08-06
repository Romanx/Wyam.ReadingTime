using System;
using System.Linq;
using System.Collections.Generic;
using Wyam.Common.Documents;
using Wyam.Common.Execution;
using Wyam.Common.Modules;
using System.Text.RegularExpressions;

namespace Wyam.ReadingTime
{
    /// <summary>
    /// Splits the content of the document provided and adds metadata to it with
    /// how long it would take to read at an average words per minute
    /// </summary>
    public class ReadingTime : IModule
    {
        private static readonly Regex SpacesRegex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly uint _wordsPerMinute;
        private readonly uint _wordsPerSecond;

        /// <summary>
        /// A default contstructor setting the words per minute to the average of 200
        /// </summary>
        public ReadingTime() : this(200)
        {
        }

        /// <summary>
        /// A constructor allowing the user to set the average words per minute. This is useful for other languages
        /// </summary>
        /// <param name="wordsPerMinute"></param>
        public ReadingTime(uint wordsPerMinute)
        {
            _wordsPerMinute = wordsPerMinute;
            _wordsPerSecond = wordsPerMinute / 60;
        }

        /// <summary>
        /// Executes the pipeline in paralell adding the reading time metadata for each document
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
            => inputs
                .AsParallel()
                .Select(input => context.GetDocument(input, GetMetaData(input.Content)));

        private Dictionary<string, object> GetMetaData(string content)
        {
            uint words = (uint)SpacesRegex.Matches(content.Trim()).Count;

            var totalReadingTimeSeconds = words / _wordsPerSecond;
            var minutes = (uint)Math.Floor(totalReadingTimeSeconds / 60m);
            var seconds = (uint)Math.Round(totalReadingTimeSeconds - (minutes * 60m));

            return new Dictionary<string, object> {
                { ReadingTimeKeys.ReadingTime, new ReadingTimeMeta(minutes, seconds, totalReadingTimeSeconds, words) }
            };
        }
    }
}
