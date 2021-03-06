﻿using System.Collections.Generic;
using System.Linq;

namespace SongRequest
{
    public class FairQueue
    {
        /// <summary>
        /// Lock object
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// Requested songs queue
        /// </summary>
        private List<RequestedSong> _requestedSongs = new List<RequestedSong>();

        /// <summary>
        /// Number of requested songs in queue, read in a lock
        /// </summary>
        public int Count
        {
            get
            {
                lock (lockObject)
                {
                    return _requestedSongs.Count;
                }
            }
        }

        /// <summary>
        /// Add song to queue
        /// </summary>
        public void Add(RequestedSong requestedSong)
        {
            lock (lockObject)
            {
                //Do not allow adding same song twice...
                if (_requestedSongs.Any(r => r.Song == requestedSong.Song))
                    return;

                for (int i = 1; i < _requestedSongs.Count; i++)
                {
                    if (_requestedSongs[i].RequesterName == requestedSong.RequesterName)
                        continue;

                    var groupedRequestedSongs = _requestedSongs.Take(i + 1).GroupBy(r => r.RequesterName).OrderByDescending(g => g.Count());

                    var maxRequesterUntilNow = groupedRequestedSongs.FirstOrDefault();
                    if (maxRequesterUntilNow.First().RequesterName != requestedSong.RequesterName &&
                        maxRequesterUntilNow.Count() > 1 &&
                        (groupedRequestedSongs.FirstOrDefault(g => g.First().RequesterName == requestedSong.RequesterName) == null ||
                         groupedRequestedSongs.FirstOrDefault(g => g.First().RequesterName == requestedSong.RequesterName).Count() < (maxRequesterUntilNow.Count() - 1)))
                    {
                        _requestedSongs.Insert(i, requestedSong);
                        return;
                    }
                }

                _requestedSongs.Add(requestedSong);
            }
        }

        /// <summary>
        /// Remove song from queue
        /// </summary>
        /// <param name="id">Id of song to delete</param>
        public void Remove(string id)
        {
            lock (lockObject)
            {
                bool found = false;
                _requestedSongs.RemoveAll(x =>
                {
                    if (x.Song.TempId == id && !found)
                    {
                        found = true;
                        return true;
                    }
                    return false;
                });
            }
        }

        /// <summary>
        /// Remove song from queue
        /// </summary>
        /// <param name="requestedSong">Song to delete</param>
        public void Remove(RequestedSong requestedSong)
        {
            Remove(requestedSong.Song.TempId);
        }

        /// <summary>
        /// Current queue
        /// </summary>
        public IEnumerable<RequestedSong> Current
        {
            get
            {
                return _requestedSongs;
            }
        }
    }
}
