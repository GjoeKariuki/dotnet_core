using bookStore.Data;
using bookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace bookStore.Repository {

    public class LanguageRepository : ILanguageRepository
    {

        private readonly BookStoreContext _context = null;


        // dependency
        public LanguageRepository(BookStoreContext context) {
            _context = context;
        }

        public async Task<List<LanguageModel>> GetLanguages() {

            return await _context.Language.Select(x => new LanguageModel() {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToListAsync();
        }
    }
}