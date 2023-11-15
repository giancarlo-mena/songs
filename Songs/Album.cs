namespace Songs
{
    class Album
    {
        public string _title { get; set; }
        public List<Song> _songs { get; set; }
        public Uri _albumLink { get; set; }

        public Album(string title, List<Song> songs, Uri albumLink)
        {
            _title = title;
            _songs = songs;
            _albumLink = albumLink;
        }
    }
}
