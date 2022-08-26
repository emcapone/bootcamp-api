using bootcamp_api.Models;

namespace bootcamp_api.Services;

public static class BookmarkService
{
    static List<Bookmark> Bookmarks { get; }
    static int nextId = 3;
    static BookmarkService()
    {
        Bookmarks = new List<Bookmark>
            {
                new Bookmark { Id = 1, Link = "testlink.co", Petfinder_Id = 12354, Title = "Fig Newton", Note = "Rabbit from Petfinder",
                                SavedAt = DateTime.Now, External_Url = "somelink.com" },
                new Bookmark { Id = 2, Link = "testlink.co", Petfinder_Id = 12354, Title = "Fig Newton", Note = "Rabbit from Petfinder",
                                SavedAt = DateTime.Now, External_Url = "somelink.com" },
            };
    }

    public static List<Bookmark> GetAll() => Bookmarks;

    public static Bookmark? Get(int id) => Bookmarks.FirstOrDefault(p => p.Id == id);

    public static void Add(Bookmark bookmark)
    {
        bookmark.Id = nextId++;
        Bookmarks.Add(bookmark);
    }

    public static void Delete(int id)
    {
        var bookmark = Get(id);
        if (bookmark is null)
            return;

        Bookmarks.Remove(bookmark);
    }

    public static void Update(Bookmark bookmark)
    {
        var index = Bookmarks.FindIndex(p => p.Id == bookmark.Id);
        if (index == -1)
            return;

        Bookmarks[index] = bookmark;
    }
}