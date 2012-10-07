using System;
using System.IO;
using System.Collections.Generic;
using GLib;
using Gst;
using Gst.BasePlugins;

namespace SongRequest
{
	public class SongPlayerGStreamer
	{
		private static MainLoop loop;
		private static PlayBin2 play;
		private List<Song> _songs;
		
		public SongPlayerGStreamer ()
		{
			_songs = new List<Song>();
			
			foreach (string file in Directory.GetFiles("/home/henk/songrequest/Music"))
			{
				Song newSong = new Song();
				newSong.FileName = file;
			}
			
			string apppath = "/home/henk/songrequest";
            System.Environment.SetEnvironmentVariable("GST_PLUGIN_PATH", apppath + @"/gstreamer/plugins");
            System.Environment.SetEnvironmentVariable("GST_PLUGIN_SYSTEM_PATH", apppath + @"/gstreamer/plugins");
            System.Environment.SetEnvironmentVariable("PATH", apppath + @"/gstreamer;"
                                                        	+ apppath + @"/gstreamer");
            System.Environment.SetEnvironmentVariable("GST_REGISTRY", apppath + @"/gstreamer/registry.bin");
			 
			
		    Gst.Application.Init ();
   			loop = new MainLoop ();
			
			play = ElementFactory.Make ("playbin2", "play") as PlayBin2;
						
			play.Uri = _songs[0].FileName;
    		play.Bus.AddWatch (new BusFunc (BusCb));
    		play.SetState (State.Playing);

    		loop.Run ();		
			
		}
		
		  private bool BusCb (Bus bus, Message message) {
		    switch (message.Type) {
		      case MessageType.Error:
		        Enum err;
		        string msg;
		        message.ParseError (out err, out msg);
		        Console.WriteLine ("Gstreamer error: {0}", msg);
		        loop.Quit ();
		        break;
		      case MessageType.Eos:
		          play.SetState (State.Null);
		          play.Uri = _songs[0].FileName;
		          play.SetState (State.Playing);
		        break;
		    }
		
		    return true;
		  }
	}
}

