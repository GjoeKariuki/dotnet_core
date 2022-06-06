using System.Collections.Generic;
using System.Threading.Tasks;
using bookStore.Models;


namespace bookStore.Repository {

    public interface ILanguageRepository {

        Task<List<LanguageModel>> GetLanguages();
    }
}