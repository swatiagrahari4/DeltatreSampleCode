using DeltatreGrpcService.Server;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltatreGrpcService
{
    public class KeywordsearchService : Keywordsearch.KeywordsearchBase
    {
        private readonly ILogger<KeywordsearchService> _logger;
        private readonly ICacheService _cacheService;
        
        public KeywordsearchService(ILogger<KeywordsearchService> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public override Task<WordReply> SearchWord(WordModel request, ServerCallContext context)
        {
            WordReply reply = new WordReply{ Result = "Not Found" };
            var result = _cacheService.FindWord(request.Word);
            if (result == true)
                reply.Result = "Found";

            return Task.FromResult(reply);
        }

        public override Task<WordReply> UpdateKeyWord(WordSaveModel request, ServerCallContext context)
        {
            WordReply reply = new WordReply { Result = "Not Updated" }; ;
            
            var resultWord = _cacheService.SetKeyword(request);
            if (resultWord == true)
                reply.Result = "Updated";

            return Task.FromResult(reply);
        }

        public override Task<ReturnTopFiveReply> FindTopFiveKeyWord(ReturnTopFiveModel request, ServerCallContext context)
        {
            var resultList = _cacheService.GetTopFive();
            return Task.FromResult(resultList);
        }
    }
}
