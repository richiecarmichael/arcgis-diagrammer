/*=============================================================================
 * 
 * Copyright � 2007 ESRI. All rights reserved. 
 * 
 * Use subject to ESRI license agreement.
 * 
 * Unpublished�all rights reserved.
 * Use of this ESRI commercial Software, Data, and Documentation is limited to
 * the ESRI License Agreement. In no event shall the Government acquire greater
 * than Restricted/Limited Rights. At a minimum Government rights to use,
 * duplicate, or disclose is subject to restrictions as set for in FAR 12.211,
 * FAR 12.212, and FAR 52.227-19 (June 1987), FAR 52.227-14 (ALT I, II, and III)
 * (June 1987), DFARS 227.7202, DFARS 252.227-7015 (NOV 1995).
 * Contractor/Manufacturer is ESRI, 380 New York Street, Redlands,
 * CA 92373-8100, USA.
 * 
 * SAMPLE CODE IS PROVIDED "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 * PARTICULAR PURPOSE, ARE DISCLAIMED.  IN NO EVENT SHALL ESRI OR CONTRIBUTORS
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) SUSTAINED BY YOU OR A THIRD PARTY, HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT; STRICT LIABILITY; OR TORT ARISING
 * IN ANY WAY OUT OF THE USE OF THIS SAMPLE CODE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE TO THE FULL EXTENT ALLOWED BY APPLICABLE LAW.
 * 
 * =============================================================================*/

using System;
using System.Collections.Generic;
using System.Text;

namespace ESRI.ArcGIS.Diagrammer {
    /// <summary>
    /// Encoding Enumeration
    /// </summary>
    public enum DiagramEncoding {
        /// <summary>
        /// Encoding for the ASCII (7-bit) character set
        /// </summary>
        ASCII,
        /// <summary>
        /// Encoding for the UTF-16 format using the big-endian byte order
        /// </summary>
        BigEndianUnicode,
        /// <summary>
        /// Encoding for the system's current ANSI code page
        /// </summary>
        Default,
        /// <summary>
        /// Encoding for the UTF-16 format using the little-endian byte order
        /// </summary>
        Unicode,
        /// <summary>
        /// Encoding for the UTF-32 format using the little-endian byte order
        /// </summary>
        UTF32,
        /// <summary>
        /// Encoding for the UTF-7 format
        /// </summary>
        UTF7,
        /// <summary>
        /// Encoding for the UTF-8 format
        /// </summary>
        UTF8
    }
}