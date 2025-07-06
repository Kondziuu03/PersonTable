namespace PersonTable.Models
{
    public class PersonListModel
    {
        public IEnumerable<PersonModel> Persons { get; set; } = [];

        public string Search { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
