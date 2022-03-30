using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltatreGrpcService.Server
{
    public interface ICacheService
    {
            public bool FindWord(string word);
            public ReturnTopFiveReply GetTopFive();
            public bool SetKeyword(WordSaveModel words);
    }    
}
