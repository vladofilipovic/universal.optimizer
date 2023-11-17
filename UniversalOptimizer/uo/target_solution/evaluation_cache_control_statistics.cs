namespace uo.TargetSolution
{

    using System.Collections.Generic;

    using System;

    using System.Linq;


    /// <summary>
    /// Class that represents control statistics for evaluation caching.
    /// </summary>
    public class EvaluationCacheControlStatistics
    {

        private Dictionary<string, QualityOfSolution> _cache;

        private int _cacheHitCount;

        private int _cacheRequestCount;

        private bool _isCaching;

        private int _maxCacheSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationCacheControlStatistics"/> class.
        /// </summary>
        /// <param name="isCaching">if set to <c>true</c> [is caching].</param>
        /// <param name="maxCacheSize">Maximum size of the cache.</param>
        public EvaluationCacheControlStatistics(bool isCaching = false, int maxCacheSize = 0)
        {
            _isCaching = isCaching;
            _maxCacheSize = maxCacheSize;
            _cache = new Dictionary<string, QualityOfSolution>();
            _cacheHitCount = 0;
            _cacheRequestCount = 0;
        }

        /// <summary>
        /// Property getter and setter for decision if caching is used during evaluation, or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is caching; otherwise, <c>false</c>.
        /// </value>
        public bool IsCaching
        {
            get
            {
                return _isCaching;
            }
            set
            {
                _isCaching = value;
            }
        }

        /// <summary>
        /// Property getter for maximum size of the cache - if 0 cache is with unlimited size.
        /// </summary>
        /// <value>
        /// The maximum size of the cache.
        /// </value>
        /// 
        public int MaxCacheSize
        {
            get
            {
                return _maxCacheSize;
            }
        }

        /// <summary>
        /// Property getter and setter for cache.
        /// </summary>
        /// <value>
        /// The cache.
        /// </value>
        /// 
        public Dictionary<string, QualityOfSolution> Cache
        {
            get
            {
                return _cache;
            }
            set
            {
                _cache = value;
            }
        }

        /// <summary>
        /// Property getter for cache hit count.
        /// </summary>
        /// <value>
        /// The cache hit count.
        /// </value>
        /// 
        public int CacheHitCount
        {
            get
            {
                return _cacheHitCount;
            }
        }

        /// <summary>
        /// Increments the cache hit count - number of cache hits during evaluation.
        /// </summary>
        public virtual void IncrementCacheHitCount()
        {
            _cacheHitCount += 1;
        }

        /// <summary>
        /// Property getter for cache request count.
        /// </summary>
        /// <value>
        /// The cache request count.
        /// </value>
        /// 
        public int CacheRequestCount
        {
            get
            {
                return _cacheRequestCount;
            }
        }

        /// <summary>
        /// Increments number of cache requests.
        /// </summary>
        /// 
        public virtual void IncrementCacheRequestCount()
        {
            _cacheRequestCount += 1;
        }

        /// <summary>
        /// String representation of the `EvaluationCacheControlStatistics` instance.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns></returns>
        /// 
        public virtual string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}")
        {
            var s = delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupStart + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_isCaching=" + _isCaching.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_cacheHitCount=" + _cacheHitCount.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_cacheRequestCount=" + _cacheRequestCount.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// <summary>
        /// String representation of the `EvaluationCacheControlStatistics` instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this `EvaluationCacheControlStatistics` instance.
        /// </returns>
        public override string ToString()
        {
            return this.StringRep("|", 0, "", "{", "}");
        }

    }
}
