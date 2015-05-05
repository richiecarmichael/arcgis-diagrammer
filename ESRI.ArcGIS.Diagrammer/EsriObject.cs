/*=============================================================================
 * 
 * Copyright © 2007 ESRI. All rights reserved. 
 * 
 * Use subject to ESRI license agreement.
 * 
 * Unpublished—all rights reserved.
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
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;

namespace ESRI.ArcGIS.Diagrammer {
    public abstract class EsriObject : ISerializable, ICloneable {
        private bool _suspend = false;
        //
        // CONSTRUCTOR
        //
        public EsriObject() { }
        public EsriObject(IXPathNavigable path) : this() {
            if (path == null) {
                throw new ArgumentNullException("path", "Path cannot be null. Please call empty constructor.");
            }
        }
        public EsriObject(SerializationInfo info, StreamingContext context) { }
        public EsriObject(EsriObject prototype) { }
        //
        // EVENTS
        //
        public event EventHandler<EventArgs> Invalidated;
        //
        // PROPERITES
        //
        [Browsable(false)]
        public bool Suspended {
            get { return this._suspend; }
        }
        //
        // PROTOTECTED
        //
        protected virtual void OnInvalidated(EventArgs e) {
            EventHandler<EventArgs> handler = Invalidated;
            if (handler != null) {
                handler(this, e);
            }
        }
        protected virtual void WriteInnerXml(XmlWriter writer) { }
        //
        // PUBLIC METHODS
        //
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) { }
        public abstract object Clone();
        public abstract void WriteXml(XmlWriter writer);
        public abstract void Errors(List<Error> list, EsriTable table);
        public void Suspend() {
            this._suspend = true;
        }
        public void Resume() {
            this._suspend = false;
        }
    }
}
