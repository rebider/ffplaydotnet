﻿namespace Unosquare.FFplayDotNet
{
    partial class FFplay
    {
        #region Enumerations

        internal enum SyncMode
        {
            AV_SYNC_AUDIO_MASTER,
            AV_SYNC_VIDEO_MASTER,
            AV_SYNC_EXTERNAL_CLOCK,
        }

        private enum EventAction
        {
            AllocatePicture,
            Quit,
            ToggleFullScreen,
            TogglePause,
            ToggleMute,
            VolumeUp,
            VolumeDown,
            StepNextFrame,
            CycleAudio,
            CycleVideo,
            CycleSubtitles,
            CycleAll,
            NextChapter,
            PreviousChapter,
            SeekLeft10,
            SeekRight10,
            SeekLeft60,
            SeekLRight60,
            
        }

        #endregion
    }
}