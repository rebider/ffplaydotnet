﻿namespace Unosquare.FFplayDotNet.Core
{
    using FFmpeg.AutoGen;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a set of codec options per 
    /// </summary>
    public class CodecOptions
    {
        #region Private Members

        /// <summary>
        /// Holds the internal list of option items
        /// </summary>
        private readonly List<CodecOptionItem> Options = new List<CodecOptionItem>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CodecOptions"/> class.
        /// </summary>
        public CodecOptions()
        {
            // Placeholder
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an option
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="streamType">Type of the stream.</param>
        public void Add(string key, string value, MediaType streamType)
        {
            var option = new CodecOptionItem(new StreamSpecifier(streamType), key, value);
            Options.Add(option);
        }

        /// <summary>
        /// Adds an option
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="streamIndex">Index of the stream.</param>
        public void Add(string key, string value, int streamIndex)
        {
            var option = new CodecOptionItem(new StreamSpecifier(streamIndex), key, value);
            Options.Add(option);
        }

        /// <summary>
        /// Adds an option
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="streamType">Type of the stream.</param>
        /// <param name="streamIndex">Index of the stream.</param>
        public void Add(string key, string value, MediaType streamType, int streamIndex)
        {
            var option = new CodecOptionItem(new StreamSpecifier(streamType, streamIndex), key, value);
            Options.Add(option);
        }

        /// <summary>
        /// Retrieves a dictionary with the options for the specified codec.
        /// Port of filter_codec_opts
        /// </summary>
        /// <param name="codecId">The codec identifier.</param>
        /// <param name="format">The format.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="codec">The codec.</param>
        /// <returns></returns>
        internal unsafe FFDictionary FilterOptions(AVCodecID codecId, AVFormatContext* format, AVStream* stream, AVCodec* codec)
        {
            var result = new FFDictionary();

            if (codec == null)
            {
                codec = (format->oformat != null) ?
                    ffmpeg.avcodec_find_encoder(codecId) : ffmpeg.avcodec_find_decoder(codecId);
            }

            var codecClass = ffmpeg.avcodec_get_class();

            var flags = format->oformat != null ?
                ffmpeg.AV_OPT_FLAG_ENCODING_PARAM : ffmpeg.AV_OPT_FLAG_DECODING_PARAM;

            var streamType = (char)0;

            switch (stream->codecpar->codec_type)
            {
                case AVMediaType.AVMEDIA_TYPE_VIDEO:
                    streamType = 'v';
                    flags |= ffmpeg.AV_OPT_FLAG_VIDEO_PARAM;
                    break;
                case AVMediaType.AVMEDIA_TYPE_AUDIO:
                    streamType = 'a';
                    flags |= ffmpeg.AV_OPT_FLAG_AUDIO_PARAM;
                    break;
                case AVMediaType.AVMEDIA_TYPE_SUBTITLE:
                    streamType = 's';
                    flags |= ffmpeg.AV_OPT_FLAG_SUBTITLE_PARAM;
                    break;
            }

            foreach (var optionItem in Options)
            {
                // Inline port of check_stream_specifier
                var matched = ffmpeg.avformat_match_stream_specifier(format, stream, optionItem.StreamSpecifier.ToString()) > 0;
                if (matched == false) continue;

                if (ffmpeg.av_opt_find(&codecClass, optionItem.Key, null, flags, ffmpeg.AV_OPT_SEARCH_FAKE_OBJ) != null || codec == null
                   || (codec->priv_class != null && ffmpeg.av_opt_find(&codec->priv_class, optionItem.Key, null, flags, ffmpeg.AV_OPT_SEARCH_FAKE_OBJ) != null))
                {
                    result[optionItem.Key] = optionItem.Value;
                }
                else if (optionItem.StreamSpecifier.StreamSuffix[0] == streamType && ffmpeg.av_opt_find(&codecClass, optionItem.Key, null, flags, ffmpeg.AV_OPT_SEARCH_FAKE_OBJ) != null)
                {
                    result[optionItem.Key] = optionItem.Value;
                }

            }

            return result;
        }

        /// <summary>
        /// Retrieves an array of dictionaries, one for each stream index
        /// https://ffmpeg.org/ffplay.html#toc-Options
        /// Port of setup_find_stream_info_opts.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="codecOptions">The codec options.</param>
        /// <returns></returns>
        internal unsafe FFDictionary[] GetPerStreamOptions(AVFormatContext* format)
        {
            if (format->nb_streams == 0)
                return null;

            var result = new FFDictionary[format->nb_streams];
            for (var i = 0; i < format->nb_streams; i++)
                result[i] = FilterOptions(format->streams[i]->codecpar->codec_id, format, format->streams[i], null);

            return result;
        }

        #endregion

    }
}