namespace bookStore.Data {

    public class Language {

        public int Id {get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // book reference to language
        public ICollection<Book> Book {get; set; }
    }
}