namespace Songs
{
    class Song
    {
        public string _title { get; set; }
        public DateTime _date { get; set; }
        public string _size { get; set; }
        public Uri _songLink { get; set; }

        public Song(string title, DateTime date, string size, Uri songLink)
        {
            _title = title;
            _date = date;
            _size = size;
            _songLink = songLink;
        }
    }
}
