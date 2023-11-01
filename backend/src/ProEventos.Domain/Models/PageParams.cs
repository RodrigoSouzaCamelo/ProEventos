namespace ProEventos.Domain.Models
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        public int CurrentPage { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > pageSize ? CurrentPage : value; }
        }

        public string Term { get; set; } = string.Empty;

    }
}
