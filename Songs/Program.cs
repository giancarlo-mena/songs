using Songs;
using System.Net;
using System.Text;
using System.Web;

var link = "http://65.186.78.52/MUSIC/ORIG/G/Guns%20N'%20Roses_Guns%20N'%20Roses/";
var client = new HttpClient();
var albums = GetAlbumsFromLink(new Uri(link));

DownloadAlbumsMusics(albums);

List<Album> GetAlbumsFromLink(Uri link)
{
    var albums = new List<Album>();
     var home = client.GetStringAsync(link).Result.Trim();

    home.Split("\n")
        .Where(line => line.Contains("[DIR]"))
        .Select(line => line.Substring(line.IndexOf("href")))
        .Select(line => line.Split("/</a")[0])
        .Select(line => line.Remove(0, 6))
        .ToList()
        .ForEach(line =>
        {
            var albumLink = link + line.Substring(0, line.IndexOf('"'));
            var albumName = line.Substring(line.IndexOf('>') + 1);

            albums.Add(new Album(albumName, new(), new Uri(albumLink)));
        });

    albums.ForEach(album => album._songs = GetAlbumSongs(album._albumLink));

    return albums;
}

List<Song> GetAlbumSongs(Uri link)
{
    Thread.Sleep(250);

    var result = new List<Song>();
    var html = client.GetStringAsync(link).Result.Trim();

    html.Split("\n")
        .Where(line => line.Contains("[SND]"))
        .Select(line => line.Substring(line.IndexOf("href")))
        .Select(line => line.Remove(0, 6))
        .Select(line => line.Replace("</td><td align=\"right\"", string.Empty)
        .Replace("</a>", string.Empty)
        .Replace("</td", string.Empty))
        .ToList()
        .ForEach(line =>
        {
            var infos = line.Split('>');
            var songLink = link + infos[0].Substring(0, infos[0].Length - 1);
            var toAdd = new Song(infos[1], DateTime.Parse(infos[2]), infos[3], new Uri(songLink));
            result.Add(toAdd);
        });

    return result;
}

void DownloadAlbumsMusics(List<Album> albums)
{
    var uri = new Uri(link);
    var segments = uri.Segments;
    var decodedParentDir = HttpUtility.UrlDecode(segments[4]);
    var parentDir = "\\" + decodedParentDir;
    var webClient = new WebClient();

    if (!Directory.Exists(parentDir)) Directory.CreateDirectory(parentDir);

    Console.WriteLine("Downloading musics from: " + decodedParentDir.Replace("/", string.Empty));

    albums.ForEach(album =>
    {
        Console.WriteLine(Environment.NewLine + "- " + album._title + " - Total songs: " + album._songs.Count);
        
        var dir = "C:\\Users\\gianc\\OneDrive\\Área de Trabalho\\" + decodedParentDir + "\\" + album._title;

        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        album._songs.ForEach(song =>
        {
            var songFilePath = dir + "\\" + song._title;
            
            webClient.DownloadFile(song._songLink, songFilePath);

            Console.WriteLine("* Downloaded: " + song._title.Replace(".mp3", string.Empty));
        });
    });
}






