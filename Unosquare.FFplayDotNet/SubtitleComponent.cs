﻿namespace Unosquare.FFplayDotNet
{
    using FFmpeg.AutoGen;
    using System;
    using System.Collections.Generic;
    using Unosquare.FFplayDotNet.Core;

    /// <summary>
    /// Performs subtitle text decoding and extraction logic.
    /// </summary>
    /// <seealso cref="Unosquare.FFplayDotNet.MediaComponent" />
    public sealed unsafe class SubtitleComponent : MediaComponent
    {
        internal SubtitleComponent(MediaContainer container, int streamIndex)
            : base(container, streamIndex)
        {
            // placeholder. Nothing else to change here.
        }

        protected override unsafe Frame CreateFrame(AVSubtitle* frame)
        {
            var frameHolder = new SubtitleFrame(frame, Stream->time_base);
            return frameHolder;
        }

        internal override void Materialize(Frame input, FrameContainer output)
        {
            var source = input as SubtitleFrame;
            var target = output as SubtitleFrameContainer;

            if (source == null || target == null)
                throw new ArgumentNullException($"{nameof(input)} and {nameof(output)} are either null or not of a compatible media type '{MediaType}'");

            // Extract text strings
            var subtitleText = new List<string>(16);

            for (var i = 0; i < source.Pointer->num_rects; i++)
            {
                var rect = source.Pointer->rects[i];
                if (rect->text != null)
                    subtitleText.Add(Utils.PtrToStringUTF8(rect->text));
            }

            // Set the target data
            target.Duration = source.Duration;
            target.EndTime = source.EndTime;
            target.StartTime = source.StartTime;
            target.Text.Clear();
            target.Text.AddRange(subtitleText);
        }
    }

}
