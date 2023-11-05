
namespace uo.TargetSolution
{

    using System.Collections.Generic;
    using System;
    using System.Linq;


    /// <summary> 
    ///     Class that represents control statistics for solution code distance calculation cache.
    /// </summary> 
    public class DistanceCalculationCacheControlStatistics<E_co>
    {

        private Dictionary<(E_co, E_co), double> _cache;

        private int _cacheHitCount;

        private int _cacheRequestCount;

        private bool _isCaching;

        private int _maxCacheSize;

        public DistanceCalculationCacheControlStatistics(bool isCaching, int maxCacheSize = 0)
        {
            _isCaching = isCaching;
            _maxCacheSize = maxCacheSize;
            _cache = new Dictionary<(E_co, E_co), double>();
            _cacheHitCount = 0;
            _cacheRequestCount = 0;
        }

        //// 
        ////         Property getter and setter for decision if caching is used during calculation of the solution code distances, or not 
        //// 
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

        //// 
        ////         Property getter for maximum size of the cache - if 0 cache is with unlimited size 
        ////         
        public int MaxCacheSize
        {
            get
            {
                return _maxCacheSize;
            }
        }

        //// 
        ////         Property getter and setter for cache 
        //// 
        ////         
        public Dictionary<(E_co, E_co), double> Cache
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

        //// 
        ////         Property getter for number of cache hits during calculation of the solution code distances 
        ////         
        public int CacheHitCount
        {
            get
            {
                return _cacheHitCount;
            }
        }

        //// 
        ////         Increments number of cache hits during calculation of the solution code distances 
        ////         
        public virtual void IncrementCacheHitCount()
        {
            _cacheHitCount += 1;
        }

        //// 
        ////         Property getter for overall number of calculation of the solution code distances 
        ////         
        public object CacheRequestCount
        {
            get
            {
                return _cacheRequestCount;
            }
        }

        //// 
        ////         Increments overall number of calculation of the solution code distances 
        ////         
        public virtual void IncrementCacheRequestCount()
        {
            _cacheRequestCount += 1;
        }

        //// 
        ////         String representation of solution distance calculation cache control statistic 
        //// 
        ////         :param delimiter: delimiter between fields
        ////         :type delimiter: str
        ////         :param indentation: level of indentation
        ////         :type indentation: int, optional, default value 0
        ////         :param indentationSymbol: indentation symbol
        ////         :type indentationSymbol: str, optional, default value ''
        ////         :param groupStart: group start string 
        ////         :type groupStart: str, optional, default value '{'
        ////         :param groupEnd: group end string 
        ////         :type groupEnd: str, optional, default value '}'
        ////         :return: string representation of instance that controls output
        ////         :rtype: str
        ////         
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
            s += "_cache_requests_count=" + _cacheRequestCount.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        //// 
        ////         String representation of the cache control and statistics structure
        //// 
        ////         :return: string representation of the cache control and statistics structure
        ////         :rtype: str
        ////         
        public override string ToString()
        {
            return this.StringRep("|");
        }

    }
}

