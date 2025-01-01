using UnityEngine;
using UnityEngine.Playables;

public static class PlayableDirectorExtensions
{
    public enum Direction
    {
        Forward,
        Backward
    }

    public static void Play(this PlayableDirector director, PlayableAsset clip)
    {
        if (director == null)
        {
            Debug.LogWarning("PlayableDirector is missing.");
            return;
        }

        director.playableAsset = clip;
        director.Play();
    }

    // Extension method to play the timeline in a specific direction
    public static void Play(this PlayableDirector director, Direction direction)
    {
        if (director == null)
        {
            Debug.LogWarning("PlayableDirector is missing.");
            return;
        }

        // Depending on the direction, set the PlayableDirector's play rate
        switch (direction)
        {
            case Direction.Forward:
                director.playableGraph.GetRootPlayable(0).SetSpeed(1);
                break;
            case Direction.Backward:
                director.playableGraph.GetRootPlayable(0).SetSpeed(-1);
                // If you're starting from the beginning, move to the end for reverse playback
                if (Mathf.Approximately((float)director.time, 0))
                {
                    director.time = director.duration;
                }
                break;
        }

        director.Play();
    }
}
