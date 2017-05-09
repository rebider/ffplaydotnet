﻿namespace Unosquare.FFplayDotNet
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a set of options that are used to initialize a media container.
    /// </summary>
    public class MediaContainerOptions
    {
        // TODO: Support specific stream selection for each component, forced input format

        /// <summary>
        /// Gets or sets a value indicating whether [enable low resource].
        /// In theroy this should be 0,1,2,3 for 1, 1/2, 1,4 and 1/8 resolutions.
        /// TODO: We are for now just supporting 1/2 rest (true value)
        /// Port of lowres.
        /// </summary>
        public bool EnableLowRes { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether [enable fast decoding].
        /// Port of fast
        /// </summary>
        public bool EnableFastDecoding { get; set; } = false;

        /// <summary>
        /// A dictionary of Format options.
        /// Supported format options are specified in https://www.ffmpeg.org/ffmpeg-formats.html#Format-Options
        /// </summary>
        public Dictionary<string, string> FormatOptions { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the codec options.
        /// Codec options are documented here: https://www.ffmpeg.org/ffmpeg-codecs.html#Codec-Options
        /// Port of codec_opts
        /// </summary>
        public OptionsCollection CodecOptions { get; } = new OptionsCollection();

        /// <summary>
        /// Gets or sets a value indicating whether PTS are generated automatically and not read
        /// from the packets themselves. Defaults to false.
        /// Port of genpts
        /// </summary>
        public bool GeneratePts { get; set; } = false;

        /// <summary>
        /// Prevent reading from audio stream components.
        /// Port of audio_disable
        /// </summary>
        public bool IsAudioDisabled { get; set; } = false;

        /// <summary>
        /// Prevent reading from video stream components.
        /// Port of video_disable
        /// </summary>
        public bool IsVideoDisabled { get; set; } = false;

        /// <summary>
        /// Prevent reading from subtitle stream components.
        /// Port of subtitle_disable
        /// </summary>
        public bool IsSubtitleDisabled { get; set; } = false;
    }

}
