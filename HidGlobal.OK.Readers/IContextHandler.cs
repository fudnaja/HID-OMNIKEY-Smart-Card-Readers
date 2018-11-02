﻿/*****************************************************************************************
    (c) 2017-2018 HID Global Corporation/ASSA ABLOY AB.  All rights reserved.

      Redistribution and use in source and binary forms, with or without modification,
      are permitted provided that the following conditions are met:
         - Redistributions of source code must retain the above copyright notice,
           this list of conditions and the following disclaimer.
         - Redistributions in binary form must reproduce the above copyright notice,
           this list of conditions and the following disclaimer in the documentation
           and/or other materials provided with the distribution.
           THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
           AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
           THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
           ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
           FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
           (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
           LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
           ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
           (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
           THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*****************************************************************************************/

using System;
using System.Collections.Generic;
using HidGlobal.OK.Readers.Components;

namespace HidGlobal.OK.Readers
{
    public interface IContextHandler : IDisposable
    {
        /// <summary>
        /// Establish context handle for winscard resource manager.
        /// </summary>
        /// <param name="scope">Defines for the scope input parameter of SCardEstablishContext</param>
        void Establish(Scope scope);
        /// <summary>
        /// Release context handle established by <see cref="Establish(Scope)"/>.
        /// </summary>
        void Release();
        /// <summary>
        /// Establish context handle with <see cref="Scope"/> used in previous call of <see cref="Establish(Scope)"/>.
        /// </summary>
        void ReEstablish();
        /// <summary>
        /// Check if curently held context handle is valid.
        /// </summary>
        /// <returns>True if context handle is valid, false otherwise.</returns>
        bool IsValid();
        /// <summary>
        /// Retrives the context handle.
        /// </summary>
        IntPtr Handle { get; }
        /// <summary>
        /// Lists readers within given reader groups.
        /// </summary>
        /// <param name="readerGroups">Names of the reader groups</param>
        /// <returns>String array with names of active readers.</returns>
        IReadOnlyList<string> ListReaders(IReadOnlyList<string> readerGroups);
        /// <summary>
        /// List all available readers.
        /// </summary>
        /// <returns>String array with names of active readers.</returns>
        IReadOnlyList<string> ListReaders();
        /// <summary>
        /// List reader groups existing in the system.
        /// </summary>
        /// <returns>String array with names of available reader groups.</returns>
        IReadOnlyList<string> ListReaderGroups();
        /// <summary>
        /// Introduce new reader group to the smart card resource manager.
        /// </summary>
        /// <remarks>Reader group will be created only after additon of first reader to it.</remarks>
        /// <param name="groupName">Name of the reader group.</param>
        void IntroduceReaderGroup(string groupName);
        /// <summary>
        /// Deletes reader group from smart card resource manager.
        /// </summary>
        /// <param name="groupName">Name of the reader group.</param>
        void ForgetReaderGroup(string groupName);
        /// <summary>
        /// Introduce additional alias of given device to the smart card resource manager.
        /// </summary>
        /// <param name="readerName">Additional name for the device.</param>
        /// <param name="deviceName">Name of the device.</param>
        void IntroduceReader(string readerName, string deviceName);
        /// <summary>
        /// Removes reader from the smart card resource manager.
        /// </summary>
        /// <param name="readerName">Name of the reader.</param>
        void ForgetReader(string readerName);
        /// <summary>
        /// Add specified reader to the reader group. Group need to be introduced by <see cref="IntroduceReaderGroup(string)"/> beforehand.
        /// </summary>
        /// <param name="readerName">Name of the reader.</param>
        /// <param name="groupName">Name of the group.</param>
        void AddReaderToGroup(string readerName, string groupName);
        /// <summary>
        /// Removes reader form the group.
        /// </summary>
        /// <param name="readerName">Name of the reader.</param>
        /// <param name="groupName">Name of the group.</param>
        void RemoveReaderFromGroup(string readerName, string groupName);
        /// <summary>
        /// Cancel blocking request of <see cref="GetStatusChange(int,ReaderState[])"/>.
        /// </summary>
        void Cancel();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="readerStates"></param>
        IReadOnlyList<ReaderState> GetStatusChange(int timeout, IReadOnlyList<ReaderState> readerStates);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerNamesWithStatesDictionary"> Dictionary key is PCSC reader name returned by SCardListReaders(), value is actual reader state <see cref="ReaderStates"/> known by application.</param>
        /// <returns><see cref="ReaderState"/></returns>
        IReadOnlyList<ReaderState> GetReaderState(IReadOnlyDictionary<string, ReaderStates> readerNamesWithStatesDictionary);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader">PCSC reader name returned by SCardListReaders().</param>
        /// <param name="stateOfTheReader">actual reader state <see cref="ReaderStates"/> known by application.</param>
        /// <returns><see cref="ReaderState"/></returns>
        ReaderState GetReaderState(string reader, ReaderStates stateOfTheReader);
             
    }
}
