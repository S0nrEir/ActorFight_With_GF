using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    public class TimelineTrack
    {


        public TimelineTrack()
        {
            Name = string.Empty;
            IsEnabled = true;
            TrackColor = Color.green;
        }

        public TimelineTrack( string name, Color color, bool isEnabled = true)
        {
            Name = name;
            IsEnabled = isEnabled;
            TrackColor = color;
        }

        public void SetName( string name ) => Name = name;
        public void SetEnabled( bool isEnabled ) => IsEnabled = isEnabled;
        public void SetTrackColor( Color color ) => TrackColor = color;
        public void SetTrackColor( float r, float g, float b, float a = 1f ) => TrackColor = new Color( r, g, b, a );


        public string Name { get; private set; }
        public bool IsEnabled { get; private set; }
        public Color TrackColor { get; private set; }
    }
}
