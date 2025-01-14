// using System;

// public class MediaPlayer
// {
//     public string State { get; set; }

//     public void Play()
//     {
//         if (State == "Playing")
//         {
//             Console.WriteLine("Already playing.");
//         }
//         else if (State == "Paused" || State == "Stopped")
//         {
//             State = "Playing";
//             Console.WriteLine("Media is now playing.");
//         }
//     }

//     public void Pause()
//     {
//         if (State == "Playing")
//         {
//             State = "Paused";
//             Console.WriteLine("Media is paused.");
//         }
//         else
//         {
//             Console.WriteLine("Cannot pause. Media is not playing.");
//         }
//     }

//     public void Stop()
//     {
//         if (State != "Stopped")
//         {
//             State = "Stopped";
//             Console.WriteLine("Media is stopped.");
//         }
//         else
//         {
//             Console.WriteLine("Media is already stopped.");
//         }
//     }
// }

// public class Client
// {
//     public static void Main(string[] args)
//     {
//         var mediaPlayer = new MediaPlayer();

//         // Default state is stopped
//         mediaPlayer.State = "Stopped";

//         // Test operations
//         mediaPlayer.Play();
//         mediaPlayer.Pause();
//         mediaPlayer.Play();
//         mediaPlayer.Stop();
//     }
// }

// using System;

// // State Interface
// public interface IMediaPlayerState
// {
//     void Play(MediaPlayer context);
//     void Pause(MediaPlayer context);
//     void Stop(MediaPlayer context);
// }

// Concrete States

namespace StatePattern;
public interface IMediaPlayerState
{
    void Play(MediaPlayer mediaPlayer);
    void Pause(MediaPlayer mediaPlayer);
    void Stop(MediaPlayer mediaPlayer);
}

public class PlayState : IMediaPlayerState
{
    public void Pause(MediaPlayer mediaPlayer)
    {
        mediaPlayer.SetState(new PauseState());
        System.Console.WriteLine("Media player paused");
    }

    public void Play(MediaPlayer mediaPlayer)
    {
       System.Console.WriteLine("Already playing");
    }

    public void Stop(MediaPlayer mediaPlayer)
    {
        mediaPlayer.SetState(new StopState());
        System.Console.WriteLine("Media player stopped");
    }
}

public class PauseState : IMediaPlayerState
{
    public void Pause(MediaPlayer mediaPlayer)
    {
        System.Console.WriteLine("Media player already paused");
    }

    public void Play(MediaPlayer mediaPlayer)
    {
        mediaPlayer.SetState(new PlayState());

        System.Console.WriteLine("Media player resumed playing");
    }

    public void Stop(MediaPlayer mediaPlayer)
    {
        mediaPlayer.SetState(new StopState());
        System.Console.WriteLine("Media player stopped");
    }
}

public class StopState : IMediaPlayerState
{
    public void Pause(MediaPlayer mediaPlayer)
    {
        mediaPlayer.SetState(new PauseState());

        System.Console.WriteLine("Media player paused");
    }

    public void Play(MediaPlayer mediaPlayer)
    {
       mediaPlayer.SetState(new PlayState());
       System.Console.WriteLine("Media player play");
    }

    public void Stop(MediaPlayer mediaPlayer)
    {
        System.Console.WriteLine("Media player already stopped");
    }
}

public class MediaPlayer
{
    private IMediaPlayerState state;
    public MediaPlayer(IMediaPlayerState state)
    {
        this.state = state;
    }
    public void SetState(IMediaPlayerState state)
    {
        this.state  = state;
    }

    public void Play()
    {
        state.Play(this);
    }

    public void Pause()
    {
        state.Pause(this);
    }

    public void Stop()
    {
        state.Stop(this);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var mediaPlayer = new MediaPlayer(new StopState());

        // Test operations
        mediaPlayer.Play();
        mediaPlayer.Pause();
        mediaPlayer.Play();
        mediaPlayer.Stop();
        mediaPlayer.Pause();
    }
}