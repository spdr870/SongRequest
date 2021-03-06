using System.Collections.Generic;

namespace SongRequest
{
    public delegate void StatusChangedEventHandler(string status);

    public interface ISongplayer
    {
        event StatusChangedEventHandler LibraryStatusChanged;
        event StatusChangedEventHandler PlayerStatusChanged;

        PlayerStatus PlayerStatus { get; }

        IEnumerable<Song> GetPlayList(string filter, string sortBy, bool ascending);

        IEnumerable<RequestedSong> PlayQueue { get; }

        void Next(string requesterName);

        void Enqueue(string id, string requesterName);
        void Enqueue(Song song, string requesterName);

        void Dequeue(string id, string requesterName);
        void Dequeue(Song song, string requesterName);

        void Pause(string requesterName);

        void Rescan(string requesterName);

        int Volume { get; set; }
    }
}

