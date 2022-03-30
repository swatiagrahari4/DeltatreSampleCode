using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltatreGrpcService.Server
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _redisDB;
        //private readonly IDatabaseAsync _redisDBAsync;
        public const string wordKey = "searchword:key";

        public CacheService(IDatabase redisDB)
        {
            //_redisCache = redis;
            _redisDB = redisDB;
            _redisDB.SortedSetAdd(wordKey, "hello", 1);
            _redisDB.SortedSetAdd(wordKey, "goodbye", 1);
            _redisDB.SortedSetAdd(wordKey, "simple", 1);
            _redisDB.SortedSetAdd(wordKey, "list", 1);
            _redisDB.SortedSetAdd(wordKey, "search", 1);
            _redisDB.SortedSetAdd(wordKey, "filter", 1);
            _redisDB.SortedSetAdd(wordKey, "yes", 1);
            _redisDB.SortedSetAdd(wordKey, "no", 1);

        }
        public bool FindWord(string word)
        {
            double? value = _redisDB.SortedSetScore(wordKey, word.ToLower());
            if (value.HasValue)
                _redisDB.SortedSetIncrement(wordKey, word.ToLower(), 1.0);
            else
                _redisDB.SortedSetAdd(wordKey, word.ToLower(), 1.0);

            return value.HasValue;
        }

        public ReturnTopFiveReply GetTopFive()
        {
            ReturnTopFiveReply returnTopFiveReply = new ReturnTopFiveReply();
            var resultList = _redisDB.SortedSetRangeByRankWithScores(wordKey, 0, 4, Order.Descending);
            int i = 0;
            foreach (var result in resultList) {
                WordDict wordDict = new WordDict();
                wordDict.Word = result.Element;
                wordDict.CountSearch = result.Score;
                returnTopFiveReply.Words.Add(wordDict);
                ++i;
            }
            return returnTopFiveReply;
        }

        public bool SetKeyword(WordSaveModel words)
        {
            foreach (string word in words.Word)
            {
                double? value = _redisDB.SortedSetScore(wordKey, word.ToLower());
                if (!value.HasValue)
                    _redisDB.SortedSetAdd(wordKey, word.ToLower(), 1.0);
            }
            return true;
        }
    }
}
