﻿//-----------------------------------------------------------------------
// <copyright file="ProtocolMonitorLog.cs" company="Microsoft Open Technologies, Inc.">
//
// The copyright in this software is being made available under the BSD License, included below. 
// This software may be subject to other third party and contributor rights, including patent rights, 
// and no such rights are granted under this license.
//
// Copyright (c) 2012, Microsoft Open Technologies, Inc. 
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, 
//   this list of conditions and the following disclaimer in the documentation 
//   and/or other materials provided with the distribution.
// - Neither the name of Microsoft Open Technologies, Inc. nor the names of its contributors 
//   may be used to endorse or promote products derived from this software 
//   without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS “AS IS” AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
// EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// </copyright>
//-----------------------------------------------------------------------

namespace Client.Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Monitor session log.
    /// </summary>
    public class ProtocolMonitorLog
    {
        #region Fields

        /// <summary>
        /// Separator constant
        /// </summary>   
        private const string Separator = "______________________________________\n";

        /// <summary>
        /// list of frames
        /// </summary>   
        private readonly List<FrameLogItem> framesLog = new List<FrameLogItem>();

        /// <summary>
        /// Current line number for SxS output.
        /// </summary>
        private ProtocolReportLines sideBySideLine = ProtocolReportLines.InvalidLine;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolMonitorLog"/> class.
        /// </summary>
        public ProtocolMonitorLog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolMonitorLog"/> class.
        /// </summary>
        /// <param name="theOtherLog">the other object.</param>
        public ProtocolMonitorLog(ProtocolMonitorLog theOtherLog)
        {
            this.TotalSizeDataFrameReceived = theOtherLog.TotalSizeDataFrameReceived;
            this.TotalSizeControlFrameReceived = theOtherLog.TotalSizeControlFrameReceived;
            this.TotalSizeFrameSent = theOtherLog.TotalSizeFrameSent;
            this.TotalCountControlFramesSent = theOtherLog.TotalCountControlFramesSent;
            this.TotalCountControlFramesReceived = theOtherLog.TotalCountControlFramesReceived;
            this.TotalCountDataFramesSent = theOtherLog.TotalCountDataFramesSent;
            this.TotalCountDataFramesReceived = theOtherLog.TotalCountDataFramesReceived;
            this.TotalCountStreamsOpened = theOtherLog.TotalCountStreamsOpened;
            this.LogTitle = theOtherLog.LogTitle;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets max number of lines in totals report
        /// </summary>   
        public int MaxLines 
        { 
            get
            {
                return (int)ProtocolReportLines.ProtocolReportMaxLine;
            } 
        }

        /// <summary>
        /// Gets or sets begin time
        /// </summary>   
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// Gets or sets total size of data frames
        /// </summary>   
        public long TotalSizeDataFrameReceived { get; set; }

        /// <summary>
        /// Gets or sets total size of control frames
        /// </summary>   
        public long TotalSizeControlFrameReceived { get; set; }

        /// <summary>
        /// Gets or sets total size of frames sent
        /// </summary>   
        public long TotalSizeFrameSent { get; set; }

        /// <summary>
        /// Gets or sets total size of control frames sent
        /// </summary>   
        public long TotalCountControlFramesSent { get; set; }

        /// <summary>
        /// Gets or sets total size of frames received
        /// </summary>   
        public long TotalCountControlFramesReceived { get; set; }

        /// <summary>
        /// Gets or sets total count of data frames sent
        /// </summary>   
        public long TotalCountDataFramesSent { get; set; }

        /// <summary>
        /// Gets or sets total count of data frames received
        /// </summary>   
        public long TotalCountDataFramesReceived { get; set; }

        /// <summary>
        /// Gets or sets total count of streams opened
        /// </summary>   
        public long TotalCountStreamsOpened { get; set; }

        /// <summary>
        /// Gets the frames log.
        /// </summary>
        public List<FrameLogItem> FramesLog
        {
            get { return this.framesLog; }
        }

        /// <summary>
        /// Gets the total count control frames.
        /// </summary>
        public long TotalCountControlFrames
        {
            get { return this.TotalCountControlFramesReceived + this.TotalCountControlFramesSent; }
        }

        /// <summary>
        /// Gets or sets title of the log
        /// </summary>   
        public string LogTitle { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts output for side-by-side
        /// </summary>
        public void StartSxSOutput()
        {
            this.sideBySideLine = 0;
        }

        /// <summary>
        /// Starts output for side-by-side
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents one line.
        /// </returns>
        public string GetSxSLine()
        {
            string output = string.Empty;

            if ((this.sideBySideLine == ProtocolReportLines.InvalidLine) || 
                (this.sideBySideLine >= ProtocolReportLines.ProtocolReportMaxLine))
            {
                return output;
            }

            switch (this.sideBySideLine)
            {
                case ProtocolReportLines.LineZero:
                    output = string.Format("Size of data exchanged:         {0,10}", this.TotalSizeDataFrameReceived + this.TotalSizeControlFrameReceived + this.TotalSizeFrameSent);
                    break;
                case ProtocolReportLines.LineOne:
                    output = string.Format("Total size received:            {0,10}", this.TotalSizeDataFrameReceived + this.TotalSizeControlFrameReceived);
                    break;
                case ProtocolReportLines.LineTwo:
                    output = string.Format("Size of frames sent:            {0,10}", this.TotalSizeFrameSent);
                    break;
                case ProtocolReportLines.LineThree:
                    output = string.Format("Size of data frames received:   {0,10}", this.TotalSizeDataFrameReceived);
                    break;
                case ProtocolReportLines.LineFour:
                    output = string.Format("Size of control frames received:{0,10}", this.TotalSizeControlFrameReceived);
                    break;
                case ProtocolReportLines.LineFive:
                    output = string.Format("# streams opened:               {0,10}", this.TotalCountStreamsOpened);
                    break;
                case ProtocolReportLines.LineSix:
                    output = string.Format("# control frames sent:          {0,10}", this.TotalCountControlFramesSent);
                    break;
                case ProtocolReportLines.LineSeven:
                    output = string.Format("# control frames received:      {0,10}", this.TotalCountControlFramesReceived);
                    break;
                case ProtocolReportLines.LineEight:
                    output = string.Format("# control frames:               {0,10}", this.TotalCountControlFrames);
                    break;
                case ProtocolReportLines.LineNine:
                    output = string.Format("# data frames sent:             {0,10}", this.TotalCountDataFramesSent);
                    break;
                case ProtocolReportLines.LineTen:
                    output = string.Format("# data frames received:         {0,10}", this.TotalCountDataFramesReceived);
                    break;
                case ProtocolReportLines.LineEleven:
                    output = string.Format("# frames exchanged:             {0,10}", this.TotalCountControlFrames + this.TotalCountDataFramesSent + this.TotalCountDataFramesReceived);
                    break;
                case ProtocolReportLines.LineTwelve:
                    output = string.Format("# connections opened:           {0,10}", 1);
                    break;
            }

            this.sideBySideLine++;
            return output;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string result = this.FramesLog.Aggregate(string.Empty, (current, frameItem) => current + (frameItem.ToString() + "\r\n"));

            result += this.GetShortLog();
            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents short version of log.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents short version of log.
        /// </returns>
        public string GetShortLog()
        {
            string result = Separator;
            result += "                TOTAL\n";
            this.StartSxSOutput();
            for (ProtocolReportLines i = 0; i < ProtocolReportLines.ProtocolReportMaxLine; i++)
            {
                result += this.GetSxSLine();
                result += "\n";
            }

            result += Separator;
            return result;
        }

        /// <summary>
        /// Reset all statistics and clear frame log.
        /// </summary>
        public void Reset()
        {
            this.TotalSizeDataFrameReceived = 0;
            this.TotalSizeControlFrameReceived = 0;
            this.TotalSizeFrameSent = 0;
            this.TotalCountControlFramesSent = 0;
            this.TotalCountControlFramesReceived = 0;
            this.TotalCountDataFramesSent = 0;
            this.TotalCountDataFramesReceived = 0;
            this.TotalCountStreamsOpened = 0;

            this.framesLog.Clear();
        }

        #endregion
    }
}
