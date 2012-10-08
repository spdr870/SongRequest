using System;
using System.Linq;
using System.Collections.Generic;

namespace SongRequest
{
	public class SongPlayerMock : ISongplayer
	{
		private SongLibrary songLibrary;
		
		private List<Song> _queue;
		private Song _currentSong;
		private DateTime _currentSongStart;

		public SongPlayerMock ()
		{
			songLibrary = new SongLibrary();
			
			songLibrary.AddSong(new Song(){ Artist="4 Strings", Name="Summer Sun", Duration = TimeSpan.FromSeconds(6), FileName="4.mp3"});
			songLibrary.AddSong(new Song(){ Artist="Adele", Name="Set Fire To The Rain", Duration = TimeSpan.FromSeconds(41), FileName="A.mp3"});
			songLibrary.AddSong(new Song(){ Artist="Silverblue", Name="Step Back", Duration = TimeSpan.FromSeconds(34), FileName="S.mp3"});
			songLibrary.AddSong(new Song(){ Artist="Ilse DeLange", Name="I'm not so tough", Duration = TimeSpan.FromSeconds(52), FileName="I.mp3"});
			songLibrary.AddSong(new Song(){ Artist="Coldplay", Name="Clocks", Duration = TimeSpan.FromSeconds(33), FileName="C.mp3"});
			songLibrary.AddSong(new Song(){ Artist="Queen", Name="Bohemian Rapsody", Duration = TimeSpan.FromSeconds(21), FileName="Q.mp3"});
			songLibrary.AddSong(new Song(){ Artist="The prodigy", Name="Smack My Bitch Up", Duration = TimeSpan.FromSeconds(36), FileName="Q.mp3"});
			
			_queue = new List<Song>(songLibrary.GetSongs(string.Empty, 0, 3));
		}
		
		public PlayerStatus PlayerStatus 
		{
			get
			{
				if ( _currentSong == null ||
				     _currentSong.Duration == null ||
				    (_currentSong.Duration.Value - (DateTime.Now - _currentSongStart)).TotalSeconds <= 0)
				{
					if (_queue.Count > 0)
					{					
						//Take next song from queue
						_currentSong = _queue[0];
						
						_queue.Remove(_currentSong);
					} else
					{
						//Take random song
						_currentSong = songLibrary.GetRandomSong();
					}
					
					_currentSongStart = DateTime.Now;
				}
				
				PlayerStatus playerStatus = new PlayerStatus();
				playerStatus.Song = _currentSong;
				playerStatus.Position = DateTime.Now - _currentSongStart;
					
				return playerStatus;
			}
		}
		
		public IEnumerable<Song> PlayList 
		{
			get
			{
				return songLibrary.GetSongs(string.Empty, 0, 100);
			}
		}
		
		public IEnumerable<Song> PlayQueue 
		{
			get
			{
				return _queue;
			}
		}
		
		public void Enqueue(Song song)
		{
			_queue.Add(song);
		}
		
		public void Dequeue(Song song)
		{
			_queue.Remove(song);
		}
	}
}
