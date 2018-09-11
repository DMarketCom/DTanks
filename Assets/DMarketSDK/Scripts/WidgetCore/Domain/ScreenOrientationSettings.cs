using UnityEngine;

namespace DMarketSDK.Domain
{
    public class ScreenOrientationSettings
    {
        public readonly bool RotateToLandscapeRight;
        public readonly bool RotateToLandscapeLeft;
        public readonly bool RotateToPortraitUp;
        public readonly bool RotateToPortraitDown;

        public ScreenOrientationSettings(bool rotateToLandscapeRight,
            bool rotateToLandscapeLeft, bool rotateToPortraitUp,
            bool rotateToPortraitDown)
        {
            RotateToLandscapeRight = rotateToLandscapeRight;
            RotateToLandscapeLeft = rotateToLandscapeLeft;
            RotateToPortraitUp = rotateToPortraitUp;
            RotateToPortraitDown = rotateToPortraitDown;
        }

        public static ScreenOrientationSettings GetGameOrientationSettings()
        {
            return new ScreenOrientationSettings(Screen.autorotateToLandscapeRight, Screen.autorotateToLandscapeLeft, Screen.autorotateToPortrait, Screen.autorotateToPortraitUpsideDown);
        }

        public static ScreenOrientationSettings GetMarketOrientationSettings()
        {
            bool isPortrait = Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown;

            return new ScreenOrientationSettings(!isPortrait, !isPortrait, isPortrait, isPortrait);
        }
    }
}
