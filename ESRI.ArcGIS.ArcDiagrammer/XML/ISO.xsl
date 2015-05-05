<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/TR/WD-xsl" TYPE="text/javascript">

<!-- An xsl template for displaying ISO and ESRI metadata in ArcCatalog 
     with similar look to traditional FGDC.
     Shows all elements in ISO standard and ESRI profile, and indicates
     which elements have been automatically updated by ArcCatalog.

     Copyright (c) 2001-2008, Environmental Systems Research Institute, Inc. All rights reserved.
	
     Revision History: Created 6/21/01 avienneau
                       Updated 1/18/04 avienneau - added geoprocessing history
                       Updated 3/21/08 avienneau - changed display of topic category codes to show the actual text of the code
-->


<xsl:template match="/">
  <HTML>
  <HEAD>
    <SCRIPT LANGUAGE="JScript"><xsl:comment><![CDATA[

function test() {
  var ua = window.navigator.userAgent
  var msie = ua.indexOf ( "MSIE " )
  if ( msie == -1 ) 
    document.write("<P>" + "Netscape")
}

      function fix(e) {
        var par = e.parentNode;
        e.id = "";
        e.style.marginLeft = "0.42in";
        var pos = e.innerText.indexOf("\n");
        if (pos > 0) {
          while (pos > 0) {
            var t = e.childNodes(0);
            var n = document.createElement("PRE");
            var s = t.splitText(pos);
            e.insertAdjacentElement("afterEnd", n);
            n.appendChild(s);
            n.style.marginLeft = "0.42in";
            e = n;
            pos = e.innerText.indexOf("\n");
          }
          var count = (par.children.length);
          for (var i = 0; i < count; i++) {
            e = par.children(i);
            if (e.tagName == "PRE") {
              pos = e.innerText.indexOf(">");
              if (pos != 0) {
                n = document.createElement("DD");
                e.insertAdjacentElement("afterEnd", n);
                n.innerText = e.innerText;
                e.removeNode(true);
              }
            }
          }
          if (par.children.tags("PRE").length > 0) {
            count = (par.children.length);
            for (i = 0; i < count; i++) {
              e = par.children(i);
              if (e.tagName == "PRE") {
                e.id = "";
                if (i < (count-1)) {
                  var e2 = par.children(i + 1);
                  if (e2.tagName == "PRE") {
                    e.insertAdjacentText("beforeEnd", e2.innerText+"\n");
                    e2.removeNode(true);
                    count = count-1;
                    i = i-1;
                  }
                }
              }
            }
          }
        }
        else {
          n = document.createElement("DD");
          par.appendChild(n);
          n.innerText = e.innerText;
          e.removeNode(true);
        }
      }

    ]]></xsl:comment></SCRIPT>
  </HEAD>

    
  <BODY BGCOLOR="#FFFFFF" ONCONTEXTMENU="return true">
  <FONT COLOR="00008B" SIZE="2" FACE="Verdana">


  <!-- SHOW METADATA SUMMARY -->
  
  <!-- summary doesn't include natvform and file name - ESRI extended elements
        uses distribution format and location instead -->
  <xsl:if test="/metadata[($any$ (dataIdInfo/idCitation/resTitle/text() | 
        Binary/Thumbnail/img | idinfo/browse/img | 
        dataIdInfo/descKeys[keyTyp/KeyTypCd/@value = '005']/keyword/text() | 
        refSysInfo/*/refSysID/identCode/text() | 
	 distInfo/distributor/distorTran/onLineSrc/linkage/text() | 
	 dataIdInfo/idAbs/text()))]">

    <TABLE COLS="2" WIDTH="100%" BGCOLOR="#CCFFCC" CELLPADDING="11" BORDER="0" CELLSPACING="0">

      <!-- show title -->
      <xsl:if test="/metadata[dataIdInfo/idCitation/resTitle/text()]">
        <TR ALIGN="center" VALIGN="center">
          <TD COLSPAN="2">
            <FONT COLOR="#006400" FACE="Verdana" SIZE="4"><B>
            	<xsl:value-of select="/metadata/dataIdInfo/idCitation/resTitle[text()]"/>
            </B></FONT>
          </TD>
        </TR>
      </xsl:if>

      <!-- would also test for natvform and file name -->
      <xsl:if test="/metadata[($any$ Binary/Thumbnail/img | idinfo/browse/img |
            dataIdInfo/descKeys[keyTyp/KeyTypCd/@value = '005']/keyword/text() | 
	     refSysInfo/*/refSysID/identCode/text() | 
	     distInfo/distributor/distorFormat/formatName/text() | 
	     distInfo/distributor/distorTran/onLineSrc/linkage/text() )]">

        <TR ALIGN="left" VALIGN="top">

          <!-- show thumbnail  -->
          <xsl:if test="/metadata[($any$ Binary/Thumbnail/img | idinfo/browse/img)]">
            <TD>
              <xsl:choose>
                <!-- would also test for natvform and file name -->
                <xsl:when test="/metadata[($any$ 
                      dataIdInfo/descKeys[keyTyp/KeyTypCd/@value = '005']/keyword/text() | 
		         refSysInfo/*/refSysID/identCode/text() | 
	     		  distInfo/distributor/distorFormat/formatName/text() | 
	     		  distInfo/distributor/distorTran/onLineSrc/linkage/text() )]">
                  <xsl:attribute name="WIDTH">210</xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="ALIGN">center</xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>

              <FONT COLOR="#006400" FACE="Verdana" SIZE="2">
                <xsl:apply-templates select="/metadata//img[@src]" />
                <xsl:if test="context()[not( /metadata/dataIdInfo/idAbs/text() )]"><BR/></xsl:if>
              </FONT>
            </TD>
          </xsl:if>

          <!-- show format, file name, coordinate system, theme keywords -->
          <xsl:if test="/metadata[($any$ 
                dataIdInfo/descKeys[keyTyp/KeyTypCd/@value = '005']/keyword/text() | 
  	         refSysInfo/*/refSysID/identCode/text() | 
	         distInfo/distributor/distorFormat/formatName/text() | 
	     	  distInfo/distributor/distorTran/onLineSrc/linkage/text() )]">
            <TD>
              <FONT COLOR="#006400" FACE="Verdana" SIZE="2">
                <xsl:if test="/metadata[distInfo/distributor/distorFormat/formatName/text()]">
                  <P>
                    <B>Data format:</B> <xsl:value-of select="/metadata/distInfo/distributor/distorFormat/formatName[text()]"/>
                    <!-- nowhere to get/put the image format -->
<!--                    <xsl:if test="/metadata[(idinfo/natvform = 'Raster Dataset') 
                          and (spdoinfo/rastinfo/rastifor/text())]">
                      - <xsl:value-of select="/metadata/spdoinfo/rastinfo/rastifor" />
                    </xsl:if>  -->
                  </P>
                </xsl:if>

                <xsl:if test="/metadata[refSysInfo/*/refSysID/identCode/text()]">
                  <P><B>Coordinate system:</B> 
                    <xsl:value-of select="/metadata/refSysInfo/*/refSysID/identCode[text()]"/>
                  </P>
                </xsl:if>
                
                <xsl:if test="/metadata[dataIdInfo/descKeys[keyTyp/KeyTypCd/@value = '005']/keyword/text()]">
                  <P><B>Theme keywords:</B>
                    <xsl:for-each select="/metadata/dataIdInfo/descKeys[keyTyp/KeyTypCd/@value = '005'][keyword/text()]">
                      <xsl:for-each select="keyword[text()]">
                        <xsl:value-of /><xsl:if test="context()[not(end()) and (text())]">, </xsl:if>
                      </xsl:for-each><xsl:if test="context()[not(end())]">, </xsl:if>
                    </xsl:for-each>
                  </P>
                </xsl:if>

                <xsl:if test="/metadata[distInfo/distributor/distorTran/onLineSrc/linkage/text()]">
                  <P><B>Location:</B> <xsl:value-of select="/metadata/distInfo/distributor/distorTran/onLineSrc/linkage[text()]"/></P>
                </xsl:if>

                <xsl:if test="context()[not( /metadata/dataIdInfo/idAbs/text() )]"><P/></xsl:if>
              </FONT>
            </TD>
          </xsl:if>
        </TR>
      </xsl:if>
      <xsl:if test="/metadata/dataIdInfo/idAbs[text()]">
        <TR>
          <xsl:for-each select="/metadata/dataIdInfo/idAbs[text()]">
            <TD  COLSPAN="2">
              <FONT COLOR="#006400" FACE="Verdana" SIZE="2">
                <B>Abstract:</B> <xsl:value-of /><BR/><BR/>
              </FONT>
            </TD>
          </xsl:for-each>
        </TR>
      </xsl:if>
    </TABLE>
  </xsl:if>


  <!-- BUILD THE TOC  -->

  <A name="Top"/>
  <H4>ISO and ESRI Metadata:</H4>

  <UL>
    <!-- Metadata Identification -->
    <!-- Root node "metadata" will always exist. Only add to TOC if it contains elements
          that describe the metadata. -->
    <xsl:if test="metadata[($any$ mdFileID | mdLang | mdChar | mdParentID | mdHrLv | 
        mdHrLvName | mdContact | mdDateSt | mdStanName | mdStanVer | mdMaint | mdConst | dataSetURI)]">
      <LI><A HREF="#Metadata_Information">Metadata Information</A></LI>
    </xsl:if>

    <!-- Resource Identification -->
    <!-- DIS version of the DTD doesn't account for data and service subclasses of MD_Identification. 
          This template assumes service elements may appear in metadata/dataIdInfo even though
          those elements aren't in the DTD at all. If subclasses were included, the structure would be
          similar to spatial representation. -->
    <xsl:if expr="(this.selectNodes('metadata/dataIdInfo').length == 1)">
      <xsl:for-each select="metadata/dataIdInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Resource Identification Information
        </A></LI>
      </xsl:for-each>
    </xsl:if>
    <xsl:if expr="(this.selectNodes('metadata/dataIdInfo').length > 1)">
      <LI>Resource Identification Information</LI>
      <xsl:for-each select="metadata/dataIdInfo">
        <LI STYLE="margin-left:0.5in"><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
         Resource <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>
        </A></LI>
      </xsl:for-each>
    </xsl:if>

    <!-- Spatial Representation Information  -->
    <xsl:if expr="(this.selectNodes('metadata/spatRepInfo').length == 1)">
      <xsl:for-each select="metadata/spatRepInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Spatial Representation Information
        </A></LI>
      </xsl:for-each>
    </xsl:if>
    <xsl:if expr="(this.selectNodes('metadata/spatRepInfo').length > 1)">
      <LI>Spatial Representation Information</LI>
      <xsl:for-each select="metadata/spatRepInfo">
        <LI STYLE="margin-left:0.5in"><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
         Representation <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>
        </A></LI>
      </xsl:for-each>
    </xsl:if>

    <!-- Content Information  -->
    <xsl:if expr="(this.selectNodes('metadata/contInfo').length == 1)">
      <xsl:for-each select="metadata/contInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Content Information
        </A></LI>
      </xsl:for-each>
    </xsl:if>
    <xsl:if expr="(this.selectNodes('metadata/contInfo').length > 1)">
      <LI>Content Information</LI>
      <xsl:for-each select="metadata/contInfo">
        <LI STYLE="margin-left:0.5in"><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
         Description <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>
        </A></LI>
      </xsl:for-each>
    </xsl:if>

    <!-- Reference System Information  -->
    <xsl:if expr="(this.selectNodes('metadata/refSysInfo').length == 1)">
      <xsl:for-each select="metadata/refSysInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Reference System Information
        </A></LI>
      </xsl:for-each>
    </xsl:if>
    <xsl:if expr="(this.selectNodes('metadata/refSysInfo').length > 1)">
      <LI>Reference System Information</LI>
      <xsl:for-each select="metadata/refSysInfo">
        <LI STYLE="margin-left:0.5in"><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
         Reference System <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>
        </A></LI>
      </xsl:for-each>
    </xsl:if>

    <!-- Data Quality Information -->
    <xsl:if expr="(this.selectNodes('metadata/dqInfo').length == 1)">
      <xsl:for-each select="metadata/dqInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Data Quality Information
        </A></LI>
      </xsl:for-each>
    </xsl:if>
    <xsl:if expr="(this.selectNodes('metadata/dqInfo').length > 1)">
      <LI>Data Quality Information</LI>
      <xsl:for-each select="metadata/dqInfo">
        <LI STYLE="margin-left:0.5in"><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
         Data Quality <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>
        </A></LI>
      </xsl:for-each>
    </xsl:if>

    <!-- Distribution Information  -->
    <xsl:for-each select="metadata/distInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Distribution Information
        </A></LI>
    </xsl:for-each>

    <!-- Portrayal Catalogue Reference  -->
    <xsl:if expr="(this.selectNodes('metadata/porCatInfo').length == 1)">
      <xsl:for-each select="metadata/porCatInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Portrayal Catalogue Reference
        </A></LI>
      </xsl:for-each>
    </xsl:if>
    <xsl:if expr="(this.selectNodes('metadata/porCatInfo').length > 1)">
      <LI>Portrayal Catalogue Reference</LI>
      <xsl:for-each select="metadata/porCatInfo">
        <LI STYLE="margin-left:0.5in"><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
         Catalogue <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>
        </A></LI>
      </xsl:for-each>
    </xsl:if>

    <!-- Application Schema Information  -->
    <xsl:if expr="(this.selectNodes('metadata/appSchInfo').length == 1)">
      <xsl:for-each select="metadata/appSchInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Application Schema Information
        </A></LI>
      </xsl:for-each>
    </xsl:if>
    <xsl:if expr="(this.selectNodes('metadata/appSchInfo').length > 1)">
      <LI>Application Schema Information</LI>
      <xsl:for-each select="metadata/appSchInfo">
        <LI STYLE="margin-left:0.5in"><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
         Schema <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>
        </A></LI>
      </xsl:for-each>
    </xsl:if>

    <!-- Metadata Extension Information  -->
    <xsl:if expr="(this.selectNodes('metadata/mdExtInfo').length == 1)">
      <xsl:for-each select="metadata/mdExtInfo">
        <LI><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
          Metadata Extension Information
        </A></LI>
      </xsl:for-each>
    </xsl:if>
    <xsl:if expr="(this.selectNodes('metadata/mdExtInfo').length > 1)">
      <LI>Metadata Extension Information</LI>
      <xsl:for-each select="metadata/mdExtInfo">
        <LI STYLE="margin-left:0.5in"><A>
          <xsl:attribute name="HREF">#<xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute>
         Extension <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>
        </A></LI>
      </xsl:for-each>
    </xsl:if>

    <!-- ESRI Geoprocessing History -->
    <xsl:for-each select="metadata/Esri/DataProperties/lineage">
      <LI><A HREF="#Geoprocessing">Geoprocessing History</A></LI>
    </xsl:for-each>

    <!-- ESRI Binary Information (thumbnails and enclosures) -->
    <xsl:for-each select="metadata/Binary">
      <LI><A HREF="#Binary_Enclosures">Binary Enclosures</A></LI>
    </xsl:for-each>

    </UL>

    <!-- Give legend used in this stylesheet -->
    <BLOCKQUOTE><FONT SIZE="1">
      Metadata elements shown with blue text are defined in the
      International Organization for Standardization's (ISO) document 19115
      <I>Geographic Information - Metadata.</I>
      Elements shown with <FONT color="#006400">green</FONT>
      text are defined by ESRI and will be documented as extentions to the
      ISO 19115. Elements shown with a green asterisk (<FONT color="#006400">*</FONT>) 
      will be automatically updated by ArcCatalog.

    </FONT></BLOCKQUOTE>


    <!-- PUT METADATA CONTENT ON THE HTML PAGE  -->

    <!-- Metadata Information -->
    <!-- Root node "metadata" will always exist. Only apply template if it contains elements
          that describe the metadata. -->
    <xsl:if test="metadata[($any$ mdFileID | mdLang | mdChar | mdParentID | mdHrLv | 
        mdHrLvName | mdContact | mdDateSt | mdStanName | mdStanVer | mdMaint | mdConst | dataSetURI)]">
      <xsl:apply-templates select="metadata"/>
    </xsl:if>

    <!-- Resource Identification -->
    <xsl:apply-templates select="metadata/dataIdInfo"/>

    <!-- Spatial Representation Information -->
    <xsl:apply-templates select="metadata/spatRepInfo"/>

    <!-- Content Information -->
    <xsl:apply-templates select="metadata/contInfo"/>

    <!-- Reference System Information -->
    <xsl:apply-templates select="metadata/refSysInfo"/>

    <!-- Data Quality Information -->
    <xsl:apply-templates select="metadata/dqInfo"/>

    <!-- Distribution Information -->
    <xsl:apply-templates select="metadata/distInfo"/>

    <!-- Portrayal Catalogue Reference -->
    <xsl:apply-templates select="metadata/porCatInfo"/>

    <!-- Application Schema Information -->
    <xsl:apply-templates select="metadata/appSchInfo"/>

    <!-- Metadata Extension Information -->
    <xsl:apply-templates select="metadata/mdExtInfo"/>

    <!-- ESRI Geoprocessing History -->
    <xsl:apply-templates select="metadata/Esri/DataProperties/lineage"/>

    <!-- ESRI Binary Information (thumbnails and enclosures) -->
    <xsl:apply-templates select="metadata/Binary"/>

  </FONT>


   <!-- <BR/><BR/><BR/><CENTER><FONT COLOR="#6495ED">Metadata stylesheets are provided courtesy of ESRI.  Copyright (c) 2001-2004, Environmental Systems Research Institute, Inc.  All rights reserved.</FONT></CENTER> -->

  </BODY>
  </HTML>
</xsl:template>
 

-------- 

<!-- TEMPLATES FOR METADATA UML CLASSES -->

<!-- Metadata Information (B.2.1 MD_Metadata - line1) -->
<!-- XML files created by ArcCatalog always have the root "metadata" rather than "Metadata" -->
<xsl:template match="metadata">
  <A name="Metadata_Information"><HR/></A>
  <DL>
  <DT><FONT color="#0000AA" size="3"><B>Metadata Information</B></FONT></DT>
  <BR/><BR/>
  <DL>
  <DD>
    <xsl:for-each select="mdLang">
      <DT><xsl:if test="context()[languageCode/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Metadata language:</B></FONT>
        <xsl:apply-templates select="languageCode" />
      </DT>
    </xsl:for-each>
    <xsl:for-each select="mdChar">
      <DT><xsl:if test="context()[CharSetCd/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Metadata character set:</B></FONT>
        <xsl:apply-templates select="CharSetCd" />
      </DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ mdLang | mdChar)]"><BR/><BR/></xsl:if>
    
    <xsl:for-each select="mdDateSt">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Last update:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:apply-templates select="mdMaint"/>
    <xsl:if test="context()[(mdDateSt) and not (mdMaint)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="mdConst">
      <DT><FONT color="#0000AA"><B>Metadata constraints:</B></FONT></DT>
      <DL>
        <xsl:apply-templates select="Consts"/>
        <xsl:apply-templates select="LegConsts"/>
        <xsl:apply-templates select="SecConsts"/>
      </DL>
    </xsl:for-each>

    <xsl:apply-templates select="mdContact"/>
    
    <xsl:for-each select="mdHrLv">
      <DT><xsl:if test="context()[ScopeCd/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Scope of the data described by the metadata:</B></FONT>
        <xsl:apply-templates select="ScopeCd" />
      </DT>
    </xsl:for-each>
    <xsl:for-each select="mdHrLvName">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Scope name:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ mdHrLv | HrLvName)]"><BR/><BR/></xsl:if>
 
    <xsl:for-each select="mdStanName">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Name of the metadata standard used:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="mdStanVer">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Version of the metadata standard:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ mdStanName | mdStanVer)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="mdFileID">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Metadata identifier:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="mdParentID">
      <DT><FONT color="#0000AA"><B>Parent identifier:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="dataSetURI">
      <DT><FONT color="#0000AA"><B>URI of the data described by the metadata:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ mdFileID | mdParentID | dataSetURI)]"><BR/><BR/></xsl:if>
  </DD>
  </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- 2 letter language code list from ISO 639 : 1988, in alphabetic order by code -->
<xsl:template match="languageCode">
    <xsl:choose>
        <xsl:when test="context()[@value = 'aa']">Afar</xsl:when>
        <xsl:when test="context()[@value = 'ab']">Abkhazian</xsl:when>
        <xsl:when test="context()[@value = 'af']">Afrikaans</xsl:when>
        <xsl:when test="context()[@value = 'am']">Amharic</xsl:when>
        <xsl:when test="context()[@value = 'ar']">Arabic</xsl:when>
        <xsl:when test="context()[@value = 'as']">Assamese</xsl:when>
        <xsl:when test="context()[@value = 'ay']">Aymara</xsl:when>
        <xsl:when test="context()[@value = 'az']">Azerbaijani</xsl:when>
        
        <xsl:when test="context()[@value = 'ba']">Bashkir</xsl:when>
        <xsl:when test="context()[@value = 'be']">Byelorussian</xsl:when>
        <xsl:when test="context()[@value = 'bg']">Bulgarian</xsl:when>
        <xsl:when test="context()[@value = 'bh']">Bihari</xsl:when>
        <xsl:when test="context()[@value = 'bi']">Bislama</xsl:when>
        <xsl:when test="context()[@value = 'bn']">Bengali, Bangla</xsl:when>
        <xsl:when test="context()[@value = 'bo']">Tibetan</xsl:when>
        <xsl:when test="context()[@value = 'br']">Breton</xsl:when>
        
        <xsl:when test="context()[@value = 'ca']">Catalan</xsl:when>
        <xsl:when test="context()[@value = 'co']">Corsican</xsl:when>
        <xsl:when test="context()[@value = 'cs']">Czech</xsl:when>
        <xsl:when test="context()[@value = 'cy']">Welsh</xsl:when>
        
        <xsl:when test="context()[@value = 'da']">Danish</xsl:when>
        <xsl:when test="context()[@value = 'de']">German</xsl:when>
        <xsl:when test="context()[@value = 'dz']">Bhutani</xsl:when>
        
        <xsl:when test="context()[@value = 'el']">Greek</xsl:when>
        <xsl:when test="context()[@value = 'en']">English</xsl:when>
        <xsl:when test="context()[@value = 'eo']">Esperanto</xsl:when>
        <xsl:when test="context()[@value = 'es']">Spanish</xsl:when>
        <xsl:when test="context()[@value = 'et']">Estonian</xsl:when>
        <xsl:when test="context()[@value = 'eu']">Basque</xsl:when>
        
        <xsl:when test="context()[@value = 'fa']">Persian</xsl:when>
        <xsl:when test="context()[@value = 'fi']">Finnish</xsl:when>
        <xsl:when test="context()[@value = 'fj']">Fiji</xsl:when>
        <xsl:when test="context()[@value = 'fo']">Faroese</xsl:when>
        <xsl:when test="context()[@value = 'fr']">French</xsl:when>
        <xsl:when test="context()[@value = 'fy']">Frisian</xsl:when>
        
        <xsl:when test="context()[@value = 'ga']">Irish</xsl:when>
        <xsl:when test="context()[@value = 'gd']">Scots Gaelic</xsl:when>
        <xsl:when test="context()[@value = 'gl']">Galician</xsl:when>
        <xsl:when test="context()[@value = 'gn']">Guarani</xsl:when>
        <xsl:when test="context()[@value = 'gu']">Gujarati</xsl:when>
        
        <xsl:when test="context()[@value = 'ha']">Hausa</xsl:when>
        <xsl:when test="context()[@value = 'hi']">Hindi</xsl:when>
        <xsl:when test="context()[@value = 'hr']">Croatian</xsl:when>
        <xsl:when test="context()[@value = 'hu']">Hungarian</xsl:when>
        <xsl:when test="context()[@value = 'hy']">Armenian</xsl:when>
        
        <xsl:when test="context()[@value = 'ia']">Interlingua</xsl:when>
        <xsl:when test="context()[@value = 'ie']">Interlingue</xsl:when>
        <xsl:when test="context()[@value = 'ik']">Inupiak</xsl:when>
        <xsl:when test="context()[@value = 'in']">Indonesian</xsl:when>
        <xsl:when test="context()[@value = 'is']">Icelandic</xsl:when>
        <xsl:when test="context()[@value = 'it']">Italian</xsl:when>
        <xsl:when test="context()[@value = 'iw']">Hebrew</xsl:when>
        
        <xsl:when test="context()[@value = 'ja']">Japanese</xsl:when>
        <xsl:when test="context()[@value = 'ji']">Yiddish</xsl:when>
        <xsl:when test="context()[@value = 'jw']">Javanese</xsl:when>
        
        <xsl:when test="context()[@value = 'ka']">Georgian</xsl:when>
        <xsl:when test="context()[@value = 'kk']">Kazakh</xsl:when>
        <xsl:when test="context()[@value = 'kl']">Greenlandic</xsl:when>
        <xsl:when test="context()[@value = 'km']">Cambodian</xsl:when>
        <xsl:when test="context()[@value = 'kn']">Kannada</xsl:when>
        <xsl:when test="context()[@value = 'ko']">Korean</xsl:when>
        <xsl:when test="context()[@value = 'ks']">Kashmiri</xsl:when>
        <xsl:when test="context()[@value = 'ku']">Kurdish</xsl:when>
        <xsl:when test="context()[@value = 'ky']">Kirghiz</xsl:when>
        
        <xsl:when test="context()[@value = 'la']">Latin</xsl:when>
        <xsl:when test="context()[@value = 'ln']">Lingala</xsl:when>
        <xsl:when test="context()[@value = 'lo']">Laothian</xsl:when>
        <xsl:when test="context()[@value = 'lt']">Lithuanian</xsl:when>
        <xsl:when test="context()[@value = 'lv']">Latvian, Lettish</xsl:when>
        
        <xsl:when test="context()[@value = 'mg']">Malagasy</xsl:when>
        <xsl:when test="context()[@value = 'mi']">Maori</xsl:when>
        <xsl:when test="context()[@value = 'mk']">Macedonian</xsl:when>
        <xsl:when test="context()[@value = 'ml']">Malayalam</xsl:when>
        <xsl:when test="context()[@value = 'mn']">Mongolian</xsl:when>
        <xsl:when test="context()[@value = 'mo']">Moldavian</xsl:when>
        <xsl:when test="context()[@value = 'mr']">Marathi</xsl:when>
        <xsl:when test="context()[@value = 'ms']">Malay</xsl:when>
        <xsl:when test="context()[@value = 'mt']">Maltese</xsl:when>
        <xsl:when test="context()[@value = 'my']">Burmese</xsl:when>
        
        <xsl:when test="context()[@value = 'na']">Nauru</xsl:when>
        <xsl:when test="context()[@value = 'ne']">Nepali</xsl:when>
        <xsl:when test="context()[@value = 'nl']">Dutch</xsl:when>
        <xsl:when test="context()[@value = 'no']">Norwegian</xsl:when>
        
        <xsl:when test="context()[@value = 'oc']">Occitan</xsl:when>
        <xsl:when test="context()[@value = 'om']">(Afan) Oromo</xsl:when>
        <xsl:when test="context()[@value = 'or']">Oriya</xsl:when>
        
        <xsl:when test="context()[@value = 'pa']">Punjabi</xsl:when>
        <xsl:when test="context()[@value = 'pl']">Polish</xsl:when>
        <xsl:when test="context()[@value = 'ps']">Pashto, Pushto</xsl:when>
        <xsl:when test="context()[@value = 'pt']">Portugese</xsl:when>
        
        <xsl:when test="context()[@value = 'qu']">Quechua</xsl:when>
        
        <xsl:when test="context()[@value = 'rm']">Rhaeto-Romance</xsl:when>
        <xsl:when test="context()[@value = 'rn']">Kirundi</xsl:when>
        <xsl:when test="context()[@value = 'ro']">Romanian</xsl:when>
        <xsl:when test="context()[@value = 'ru']">Russian</xsl:when>
        <xsl:when test="context()[@value = 'rw']">Kinyarwanda</xsl:when>
        
        <xsl:when test="context()[@value = 'sa']">Sanskrit</xsl:when>
        <xsl:when test="context()[@value = 'sd']">Sindhi</xsl:when>
        <xsl:when test="context()[@value = 'sg']">Sangho</xsl:when>
        <xsl:when test="context()[@value = 'sh']">Serbo-Croatian</xsl:when>
        <xsl:when test="context()[@value = 'si']">Singhalese</xsl:when>
        <xsl:when test="context()[@value = 'sk']">Slovak</xsl:when>
        <xsl:when test="context()[@value = 'sl']">Slovenian</xsl:when>
        <xsl:when test="context()[@value = 'sm']">Samoan</xsl:when>
        <xsl:when test="context()[@value = 'sn']">Shona</xsl:when>
        <xsl:when test="context()[@value = 'so']">Somali</xsl:when>
        <xsl:when test="context()[@value = 'sq']">Albanian</xsl:when>
        <xsl:when test="context()[@value = 'sr']">Serbian</xsl:when>
        <xsl:when test="context()[@value = 'ss']">Siswati</xsl:when>
        <xsl:when test="context()[@value = 'st']">Sesotho</xsl:when>
        <xsl:when test="context()[@value = 'su']">Sundanese</xsl:when>
        <xsl:when test="context()[@value = 'sv']">Swedish</xsl:when>
        <xsl:when test="context()[@value = 'sw']">Swahili</xsl:when>
        
        <xsl:when test="context()[@value = 'ta']">Tamil</xsl:when>
        <xsl:when test="context()[@value = 'te']">Telugu</xsl:when>
        <xsl:when test="context()[@value = 'tg']">Tajik</xsl:when>
        <xsl:when test="context()[@value = 'th']">Thai</xsl:when>
        <xsl:when test="context()[@value = 'ti']">Tigrinya</xsl:when>
        <xsl:when test="context()[@value = 'tk']">Turkmen</xsl:when>
        <xsl:when test="context()[@value = 'tl']">Tagalog</xsl:when>
        <xsl:when test="context()[@value = 'tn']">Setswana</xsl:when>
        <xsl:when test="context()[@value = 'to']">Tonga</xsl:when>
        <xsl:when test="context()[@value = 'tr']">Turkish</xsl:when>
        <xsl:when test="context()[@value = 'ts']">Tsonga</xsl:when>
        <xsl:when test="context()[@value = 'tt']">Tatar</xsl:when>
        <xsl:when test="context()[@value = 'tw']">Twi</xsl:when>
        
        <xsl:when test="context()[@value = 'uk']">Ukrainian</xsl:when>
        <xsl:when test="context()[@value = 'ur']">Urdu</xsl:when>
        <xsl:when test="context()[@value = 'uz']">Uzbek</xsl:when>
        
        <xsl:when test="context()[@value = 'vi']">Vietnamese</xsl:when>
        <xsl:when test="context()[@value = 'vo']">Volapuk</xsl:when>
        
        <xsl:when test="context()[@value = 'wo']">Wolof</xsl:when>
        
        <xsl:when test="context()[@value = 'xh']">Xhosa</xsl:when>
        
        <xsl:when test="context()[@value = 'yo']">Yoruba</xsl:when>
        
        <xsl:when test="context()[@value = 'zh']">Chinese</xsl:when>
        <xsl:when test="context()[@value = 'zu']">Zulu</xsl:when>
        
        <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
    </xsl:choose>
</xsl:template>

<!-- Character set code list (B.5.10 MD_CharacterSetCode) -->
<xsl:template match="CharSetCd">
    <xsl:choose>
        <xsl:when test="context()[@value = '001']">ucs2 - 16 bit Universal Character Set</xsl:when>
        <xsl:when test="context()[@value = '002']">ucs4 - 32 bit Universal Character Set</xsl:when>
        <xsl:when test="context()[@value = '003']">utf7 - 7 bit UCS Transfer Format</xsl:when>
        <xsl:when test="context()[@value = '004']">utf8 - 8 bit UCS Transfer Format</xsl:when>
        <xsl:when test="context()[@value = '005']">utf16 - 16 bit UCS Transfer Format</xsl:when>
        <xsl:when test="context()[@value = '006']">8859part1 - Latin-1, Western European</xsl:when>
        <xsl:when test="context()[@value = '007']">8859part2 - Latin-2, Central European</xsl:when>
        <xsl:when test="context()[@value = '008']">8859part3 - Latin-3, South European</xsl:when>
        <xsl:when test="context()[@value = '009']">8859part4 - Latin-4, North European</xsl:when>
        <xsl:when test="context()[@value = '010']">8859part5 - Cyrillic</xsl:when>
        <xsl:when test="context()[@value = '011']">8859part6 - Arabic</xsl:when>
        <xsl:when test="context()[@value = '012']">8859part7 - Greek</xsl:when>
        <xsl:when test="context()[@value = '013']">8859part8 - Hebrew</xsl:when>
        <xsl:when test="context()[@value = '014']">8859part9 - Latin-5, Turkish</xsl:when>
        <xsl:when test="context()[@value = '015']">8859part11 - Thai</xsl:when>
        <xsl:when test="context()[@value = '016']">8859part14 - Latin-8</xsl:when>
        <xsl:when test="context()[@value = '017']">8859part15 - Latin-9</xsl:when>
        <xsl:when test="context()[@value = '018']">jis - Japanese for electronic transmission</xsl:when>
        <xsl:when test="context()[@value = '019']">shiftJIS - Japanese for MS-DOS</xsl:when>
        <xsl:when test="context()[@value = '020']">eucJP - Japanese for UNIX</xsl:when>
        <xsl:when test="context()[@value = '021']">U.S. ASCII</xsl:when>
        <xsl:when test="context()[@value = '022']">ebcdic - IBM mainframe</xsl:when>
        <xsl:when test="context()[@value = '023']">eucKR - Korean</xsl:when>
        <xsl:when test="context()[@value = '024']">big5 - Taiwanese</xsl:when>
        <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
    </xsl:choose>
</xsl:template>

<!-- Scope code list (B.2.25 MD_ScopeCode) -->
<xsl:template match="ScopeCd">
    <xsl:choose>
        <xsl:when test="context()[@value = '001']">attribute</xsl:when>
        <xsl:when test="context()[@value = '002']">attribute type</xsl:when>
        <xsl:when test="context()[@value = '003']">collection hardware</xsl:when>
        <xsl:when test="context()[@value = '004']">collection session</xsl:when>
        <xsl:when test="context()[@value = '005']">dataset</xsl:when>
        <xsl:when test="context()[@value = '006']">series</xsl:when>
        <xsl:when test="context()[@value = '007']">non-geographic dataset</xsl:when>
        <xsl:when test="context()[@value = '008']">dimension group</xsl:when>
        <xsl:when test="context()[@value = '009']">feature</xsl:when>
        <xsl:when test="context()[@value = '010']">feature type</xsl:when>
        <xsl:when test="context()[@value = '011']">property type</xsl:when>
        <xsl:when test="context()[@value = '012']">field session</xsl:when>
        <xsl:when test="context()[@value = '013']">software</xsl:when>
        <xsl:when test="context()[@value = '014']">service</xsl:when>
        <xsl:when test="context()[@value = '015']">model</xsl:when>
        <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
    </xsl:choose>
</xsl:template>

<!-- Maintenance Information (B.2.5 MD_MaintenanceInformation - line142) -->
<xsl:template match="(mdMaint | resMaint)">
    <DD>
    <xsl:choose>
      <xsl:when test="context()[../resMaint]">
        <DT><FONT color="#0000AA"><B>Resource maintenance:</B></FONT></DT>
      </xsl:when>
      <xsl:otherwise>
        <DT><FONT color="#0000AA"><B>Maintenance:</B></FONT></DT>
      </xsl:otherwise>
    </xsl:choose>

    <DD>
    <DL>
      <xsl:for-each select="dateNext">
        <DT><FONT color="#0000AA"><B>Date of next update:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="maintFreq">
        <DT><FONT color="#0000AA"><B>Update frequency:</B></FONT>
        <xsl:for-each select="MaintFreqCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">continual</xsl:when>
            <xsl:when test="context()[@value = '002']">daily</xsl:when>
            <xsl:when test="context()[@value = '003']">weekly</xsl:when>
            <xsl:when test="context()[@value = '004']">fortnightly</xsl:when>
            <xsl:when test="context()[@value = '005']">monthly</xsl:when>
            <xsl:when test="context()[@value = '006']">quarterly</xsl:when>
            <xsl:when test="context()[@value = '007']">biannually</xsl:when>
            <xsl:when test="context()[@value = '008']">annually</xsl:when>
            <xsl:when test="context()[@value = '009']">as needed</xsl:when>
            <xsl:when test="context()[@value = '010']">irregular</xsl:when>
            <xsl:when test="context()[@value = '011']">not planned</xsl:when>
            <xsl:when test="context()[@value = '998']">unknown</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:apply-templates select="usrDefFreq"/>
      <xsl:if test="context()[($any$ dateNext | maintFreq) and not (usrDefFreq)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="maintScp">
        <DT><FONT color="#0000AA"><B>Scope of the updates:</B></FONT>
            <xsl:apply-templates select="ScopeCd" />
        </DT>
      </xsl:for-each>
      <xsl:apply-templates select="upScpDesc"/>
      <xsl:if test="context()[(maintScp) and not (upScpDesc)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="maintNote">
        <DT><FONT color="#0000AA"><B>Other maintenance requirements:</B></FONT> <xsl:value-of/></DT>
        <BR/><BR/>
      </xsl:for-each>
    </DL>
    </DD>
    </DD>
</xsl:template>

<!-- Time Period Information (from 19103 information in 19115 DTD) -->
<xsl:template match="usrDefFreq">
  <DD>
  <DT><FONT color="#0000AA"><B>Time period between updates:</B></FONT></DT>
  <DD>
  <DL>
    <xsl:for-each select="designator">
      <DT><FONT color="#0000AA"><B>Time period designator:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="years">
      <DT><FONT color="#0000AA"><B>Years:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="months">
      <DT><FONT color="#0000AA"><B>Months:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="days">
      <DT><FONT color="#0000AA"><B>Days:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="timeIndicator">
      <DT><FONT color="#0000AA"><B>Time indicator:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="hours">
      <DT><FONT color="#0000AA"><B>Hours:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="minutes">
      <DT><FONT color="#0000AA"><B>Minutes:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="seconds">
      <DT><FONT color="#0000AA"><B>Seconds:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
  </DL>
  </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Scope Description Information (B.2.5.1 MD_ScopeDescription - line149) -->
<xsl:template match="(scpLvlDesc | upScpDesc)">
<DD>
  <xsl:if test="context()[0]">
    <DT><FONT color="#0000AA"><B>Scope description:</B></FONT></DT>
  </xsl:if>

  <xsl:for-each select="attribSet">
    <DD><FONT color="#0000AA"><B>Attributes:</B></FONT> <xsl:value-of/></DD>
  </xsl:for-each>
  <xsl:for-each select="featSet">
    <DD><FONT color="#0000AA"><B>Features:</B></FONT> <xsl:value-of/></DD>
  </xsl:for-each>
  <xsl:for-each select="featIntSet">
    <DD><FONT color="#0000AA"><B>Feature instances:</B></FONT> <xsl:value-of/></DD>
  </xsl:for-each>
  <xsl:for-each select="attribIntSet">
    <DD><FONT color="#0000AA"><B>Attribute instances:</B></FONT> <xsl:value-of/></DD>
  </xsl:for-each>
  <xsl:for-each select="datasetSet">
    <DD><FONT color="#0000AA"><B>Dataset:</B></FONT> <xsl:value-of/></DD>
  </xsl:for-each>
  <xsl:for-each select="other">
    <DD><FONT color="#0000AA"><B>Other:</B></FONT> <xsl:value-of/></DD>
  </xsl:for-each>

  <xsl:if test="context()[end()]"><BR/><BR/></xsl:if>
  </DD>
</xsl:template>

<!-- General Constraint Information (B.2.3 MD_Constraints - line67) -->
<xsl:template match="Consts">
  <DD>
    <DT><FONT color="#0000AA"><B>Constraints:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="useLimit">
        <DT><FONT color="#0000AA"><B>Limitations of use:</B></FONT> <xsl:value-of/></DT>
        <BR/><BR/>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Legal Constraint Information (B.2.3 MD_LegalConstraints - line69) -->
<xsl:template match="LegConsts">
  <DD>
    <DT><FONT color="#0000AA"><B>Legal constraints:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:if test="accessConsts">
        <DT><FONT color="#0000AA"><B>Access constraints:</B></FONT>
        <xsl:for-each select="accessConsts">
              <xsl:apply-templates select="RestrictCd" /> 
          <xsl:if test="context()[not(end())]">, </xsl:if>
        </xsl:for-each>
        </DT>
        <BR/><BR/>
      </xsl:if>

      <xsl:if test="useConsts">
        <DT><FONT color="#0000AA"><B>Use constraints:</B></FONT>
        <xsl:for-each select="useConsts">
            <xsl:apply-templates select="RestrictCd" /> 
          <xsl:if test="context()[not(end())]">, </xsl:if>
        </xsl:for-each>
        </DT>
        <BR/><BR/>
      </xsl:if>

      <xsl:for-each select="othConsts">
        <DT><FONT color="#0000AA"><B>Other constraints:</B></FONT></DT>
        <PRE ID="original"><xsl:value-of /></PRE>
        <SCRIPT>fix(original)</SCRIPT>
        <BR/>
      </xsl:for-each>

      <xsl:for-each select="useLimit">
        <DT><FONT color="#0000AA"><B>Limitations of use:</B></FONT> <xsl:value-of/></DT>
        <BR/><BR/>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Restrictions code list (B.5.24 MD_RestrictionCode) -->
<xsl:template match="RestrictCd">
    <xsl:choose>
        <xsl:when test="context()[@value = '001']">copyright</xsl:when>
        <xsl:when test="context()[@value = '002']">patent</xsl:when>
        <xsl:when test="context()[@value = '003']">patent pending</xsl:when>
        <xsl:when test="context()[@value = '004']">trademark</xsl:when>
        <xsl:when test="context()[@value = '005']">license</xsl:when>
        <xsl:when test="context()[@value = '006']">intellectual property rights</xsl:when>
        <xsl:when test="context()[@value = '007']">restricted</xsl:when>
        <xsl:when test="context()[@value = '008']">other restrictions</xsl:when>
        <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
    </xsl:choose>
</xsl:template>

<!-- Security Constraint Information (B.2.3 MD_SecurityConstraints - line73) -->
<xsl:template match="SecConsts">
  <DD>
    <DT><FONT color="#0000AA"><B>Security constraints:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="class">
        <DT><FONT color="#0000AA"><B>Classification:</B></FONT>
        <xsl:for-each select="ClasscationCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">unclassified</xsl:when>
            <xsl:when test="context()[@value = '002']">restricted</xsl:when>
            <xsl:when test="context()[@value = '003']">confidential</xsl:when>
            <xsl:when test="context()[@value = '004']">secret</xsl:when>
            <xsl:when test="context()[@value = '005']">top secret</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:for-each select="classSys">
        <DT><FONT color="#0000AA"><B>Classification system:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ class | classSys)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="userNote">
        <DT><FONT color="#0000AA"><B>Legal constraints:</B></FONT> <xsl:value-of/></DT>
        <BR/><BR/>
      </xsl:for-each>

      <xsl:for-each select="handDesc">
        <DT><FONT color="#0000AA"><B>Additional restrictions:</B></FONT> <xsl:value-of/></DT>
        <BR/><BR/>
      </xsl:for-each>

      <xsl:for-each select="useLimit">
        <DT><FONT color="#0000AA"><B>Limitations of use:</B></FONT> <xsl:value-of/></DT>
        <BR/><BR/>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
</xsl:template>

-------- 

<!-- RESOURCE IDENTIFICATION -->

<!-- Resource Identification Information (B.2.2 MD_Identification - line23, including MD_DataIdentification - line36) -->
<!-- DTD doesn't account for data and service subclasses of MD_Identification -->
<xsl:template match="metadata/dataIdInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
  <xsl:if expr="(this.selectNodes('/metadata/dataIdInfo').length == 1)">
      <DT><FONT color="#0000AA" size="3"><B>Resource Identification Information:</B></FONT></DT>
  </xsl:if>
  <xsl:if expr="(this.selectNodes('/metadata/dataIdInfo').length > 1)">
      <DT><FONT color="#0000AA" size="3"><B>
        Resource Identification Information - Resource <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>:
      </B></FONT></DT>
  </xsl:if>
  <BR/><BR/>

  <DL>
  <DD>
    <xsl:apply-templates select="idCitation"/>

    <xsl:if test="tpCat">
      <DT><FONT color="#0000AA"><B>Themes or categories of the resource:</B></FONT>
      <xsl:for-each select="tpCat">
        <xsl:for-each select="TopicCatCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">farming</xsl:when>
            <xsl:when test="context()[@value = '002']">biota</xsl:when>
            <xsl:when test="context()[@value = '003']">boundaries</xsl:when>
            <xsl:when test="context()[@value = '004']">climatologyMeteorologyAtmosphere</xsl:when>
            <xsl:when test="context()[@value = '005']">economy</xsl:when>
            <xsl:when test="context()[@value = '006']">elevation</xsl:when>
            <xsl:when test="context()[@value = '007']">environment</xsl:when>
            <xsl:when test="context()[@value = '008']">geoscientificInformation</xsl:when>
            <xsl:when test="context()[@value = '009']">health</xsl:when>
            <xsl:when test="context()[@value = '010']">imageryBaseMapsEarthCover</xsl:when>
            <xsl:when test="context()[@value = '011']">intelligenceMilitary</xsl:when>
            <xsl:when test="context()[@value = '012']">inlandWaters</xsl:when>
            <xsl:when test="context()[@value = '013']">location</xsl:when>
            <xsl:when test="context()[@value = '014']">oceans</xsl:when>
            <xsl:when test="context()[@value = '015']">planningCadastre</xsl:when>
            <xsl:when test="context()[@value = '016']">society</xsl:when>
            <xsl:when test="context()[@value = '017']">structure</xsl:when>
            <xsl:when test="context()[@value = '018']">transportation</xsl:when>
            <xsl:when test="context()[@value = '019']">utilitiesCommunication</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        <xsl:if test="context()[not(end())]">, </xsl:if>
      </xsl:for-each>
      </DT>
      <xsl:if test="context()[end()]"><BR/><BR/></xsl:if>
    </xsl:if>

    <xsl:apply-templates select="descKeys"/>
    
    <xsl:for-each select="idAbs">
      <DT><FONT color="#0000AA"><B>Abstract:</B></FONT></DT>
      <PRE ID="original"><xsl:value-of /></PRE>
      <SCRIPT>fix(original)</SCRIPT>
      <BR/>
    </xsl:for-each>

    <xsl:for-each select="idPurp">
      <DT><FONT color="#0000AA"><B>Purpose:</B></FONT></DT>
      <PRE ID="original"><xsl:value-of /></PRE>
      <SCRIPT>fix(original)</SCRIPT>
      <BR/>
    </xsl:for-each>

    <xsl:apply-templates select="graphOver"/>
    
    <xsl:for-each select="dataLang">
      <DT><xsl:if test="context()[languageCode/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Dataset language:</B></FONT>
          <xsl:apply-templates select="languageCode" />
      </DT>
    </xsl:for-each>
    <xsl:for-each select="dataChar">
      <DT><FONT color="#0000AA"><B>Dataset character set:</B></FONT>
        <xsl:apply-templates select="CharSetCd" />
      </DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ dataLang | dataChar)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="serType">
      <DT><FONT color="#0000AA"><B>Type of service:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="typeProps">
      <DT><FONT color="#0000AA"><B>Attributes of the service:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ serType | typeProps)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="idStatus">
      <DT><FONT color="#0000AA"><B>Status:</B></FONT>
        <xsl:for-each select="ProgCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">completed</xsl:when>
            <xsl:when test="context()[@value = '002']">historical archive</xsl:when>
            <xsl:when test="context()[@value = '003']">obsolete</xsl:when>
            <xsl:when test="context()[@value = '004']">on-going</xsl:when>
            <xsl:when test="context()[@value = '005']">planned</xsl:when>
            <xsl:when test="context()[@value = '006']">required</xsl:when>
            <xsl:when test="context()[@value = '007']">under development</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each> 
        </DT>
    </xsl:for-each>
    <xsl:apply-templates select="resMaint"/>
    <xsl:if test="context()[(idStatus) and not (resMaint)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="resConst">
      <DT><FONT color="#0000AA"><B>Resource constraints:</B></FONT></DT>
      <DL>
        <xsl:apply-templates select="Consts"/>
        <xsl:apply-templates select="LegConsts"/>
        <xsl:apply-templates select="SecConsts"/>
      </DL>
    </xsl:for-each>

    <xsl:apply-templates select="idSpecUse"/>

    <xsl:for-each select="spatRpType">
      <DT><xsl:if test="context()[SpatRepTypCd/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Spatial representation type:</B></FONT>
        <xsl:for-each select="SpatRepTypCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">vector</xsl:when>
            <xsl:when test="context()[@value = '002']">grid</xsl:when>
            <xsl:when test="context()[@value = '003']">text table</xsl:when>
            <xsl:when test="context()[@value = '004']">tin</xsl:when>
            <xsl:when test="context()[@value = '005']">stereo model</xsl:when>
            <xsl:when test="context()[@value = '006']">video</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
    </xsl:for-each>
    <xsl:apply-templates select="dsFormat"/>
    <xsl:if test="context()[(spatRpType) and not (dsFormat)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="envirDesc">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Processing environment:</B></FONT> <xsl:value-of/></DT>
      <BR/><BR/>
    </xsl:for-each>
    
    <xsl:apply-templates select="dataScale"/>
    
   <xsl:apply-templates select="geoBox"/>

    <xsl:apply-templates select="geoDesc"/>

    <xsl:apply-templates select="dataExt"/>
    
    <xsl:for-each select="suppInfo">
      <DT><FONT color="#0000AA"><B>Supplemental information:</B></FONT></DT>
      <PRE ID="original"><xsl:value-of /></PRE>
      <SCRIPT>fix(original)</SCRIPT>
      <BR/>
    </xsl:for-each>

    <xsl:for-each select="idCredit">
      <DT><FONT color="#0000AA"><B>Credits:</B></FONT></DT>
      <PRE ID="original"><xsl:value-of /></PRE>
      <SCRIPT>fix(original)</SCRIPT>
      <BR/>
    </xsl:for-each>
    
    <xsl:apply-templates select="idPoC"/>
  </DD>
  </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- Keyword Information (B.2.2.2 MD_Keywords - line52)-->
<xsl:template match="descKeys">
  <DD>
   <xsl:choose>
     <xsl:when test="keyTyp/KeyTypCd[@value = '001']">
        <DT><FONT color="#0000AA"><B>Discipline keywords:</B></FONT></DT>
     </xsl:when>
     <xsl:when test="keyTyp/KeyTypCd[@value = '002']">
        <DT><FONT color="#0000AA"><B>Place keywords:</B></FONT></DT>
     </xsl:when>
     <xsl:when test="keyTyp/KeyTypCd[@value = '003']">
        <DT><FONT color="#0000AA"><B>Stratum keywords:</B></FONT></DT>
     </xsl:when>
     <xsl:when test="keyTyp/KeyTypCd[@value = '004']">
        <DT><FONT color="#0000AA"><B>Temporal keywords:</B></FONT></DT>
     </xsl:when>
     <xsl:when test="keyTyp/KeyTypCd[@value = '005']">
        <DT><FONT color="#0000AA"><B>Theme keywords:</B></FONT></DT>
     </xsl:when>
     <xsl:otherwise>
        <DT><FONT color="#0000AA"><B>Descriptive keywords:</B></FONT></DT>
     </xsl:otherwise>
   </xsl:choose>
    <DD>
    <DL>
      <xsl:if test="keyTyp/KeyTypCd[(@value != '001') and (@value != '002') and (@value != '003') and (@value != '004') and (@value != '005')]">
        <DT><FONT color="#0000AA"><B>Type of keywords:</B></FONT><xsl:value-of select="@value"/></DT>
        <BR/><BR/>
      </xsl:if>
      
      <xsl:if test="keyword[text()]">
        <DT>
        <xsl:for-each select="keyword[text()]">
          <xsl:if test="context()[0]"><FONT color="#0000AA"><B>Keywords:</B></FONT> </xsl:if>
          <xsl:value-of /><xsl:if test="context()[not(end())]">, </xsl:if>
        </xsl:for-each>
        </DT>
        <BR/><BR/>
      </xsl:if>

      <xsl:apply-templates select="thesaName"/>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Browse Graphic Information (B.2.2.1 MD_BrowGraph - line48) -->
<xsl:template match="graphOver">
  <DD>
    <DT><FONT color="#0000AA"><B>Graphic overview:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="bgFileName">
        <DT><FONT color="#0000AA"><B>File name:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="bgFileType">
        <DT><FONT color="#0000AA"><B>File type:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="bgFileDesc">
        <DT><FONT color="#0000AA"><B>File description:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Usage Information (B.2.2.5 MD_Usage - line62) -->
<xsl:template match="idSpecUse">
  <DD>
    <DT><FONT color="#0000AA"><B>How the resource is used:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="usageDate">
        <DT><FONT color="#0000AA"><B>Date and time of use:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="specUsage">
        <DT><FONT color="#0000AA"><B>Description:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ usageDate | specUsage)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="usrDetLim">
        <DT><FONT color="#0000AA"><B>How the resource must not be used:</B></FONT> <xsl:value-of/></DT>
        <BR/><BR/>
      </xsl:for-each>

      <xsl:apply-templates select="usrCntInfo"/>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Resolution Information (B.2.2.4 MD_Resolution - line59) -->
<xsl:template match="(dataScale | srcScale)">
  <DD>
  <xsl:choose>
    <xsl:when test="../dataScale">
        <DT><FONT color="#0000AA"><B>Spatial resolution:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../srcScale">
        <DT><FONT color="#0000AA"><B>Resolution of the source data:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
        <DT><FONT color="#0000AA"><B>Resolution:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>
  
    <DD>
    <DL>
      <xsl:apply-templates select="equScale"/>

      <xsl:for-each select="scaleDist">
        <DT><FONT color="#0000AA"><B>Ground sample distance:</B></FONT></DT>
        <DD>
        <DL>
            <!-- value will be shown regardless of the subelement Integer, Real, or Decimal -->
            <xsl:for-each select="value">
              <DT><FONT color="#0000AA"><B>Precision of spatial data:</B></FONT> <xsl:value-of/></DT>
            </xsl:for-each>

            <xsl:apply-templates select="uom"/>

            <xsl:if test="context()[(value) and not (uom)]"><BR/><BR/></xsl:if>
        </DL>
        </DD>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Representative Fraction Information (B.2.2.3 MD_RepresentativeFraction - line56) -->
<xsl:template match="equScale">
  <DT><FONT color="#0000AA"><B>Dataset's scale:</B></FONT></DT>
  <DD>
  <DL>
    <xsl:for-each select="rfDenom">
      <DT><FONT color="#0000AA"><B>Scale denominator:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="Scale">
      <DT><FONT color="#0000AA"><B>Fraction is derived from scale:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
  </DL>
  </DD>
  <BR/>
</xsl:template>

<!-- Units of Measurement Types (from ISO 19103 information in 19115 DTD) -->
<xsl:template match="uom">
    <xsl:choose>
      <xsl:when test="context()[UomArea]">
          <DT><FONT color="#0000AA"><B>Units of measure, area:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[UomLength]">
          <DT><FONT color="#0000AA"><B>Units of measure, length:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[UomVolume]">
          <DT><FONT color="#0000AA"><B>Units of measure, volume:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[UomScale]">
          <DT><FONT color="#0000AA"><B>Units of measure, scale:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[UomTime]">
          <DT><FONT color="#0000AA"><B>Units of measure, time:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[UomVelocity]">
          <DT><FONT color="#0000AA"><B>Units of measure, velocity:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[UomAngle]">
          <DT><FONT color="#0000AA"><B>Units of measure, angle:</B></FONT></DT>
      </xsl:when>
      <xsl:otherwise>
          <DT><FONT color="#0000AA"><B>Units of measure:</B></FONT></DT>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:apply-templates select="node()"/>
</xsl:template>

<!-- Units of Measurement (from ISO 19103 information in 19115 DTD) -->
<xsl:template match="(UomArea | UomTime | UomLength | UomVolume | UomVelocity | 
	UomAngle | UomScale | vertUoM | axisUnits | falENUnits | valUnit)">
  <DD>
  <DL>
    <xsl:for-each select="uomName">
       <DT><FONT color="#0000AA"><B>Units:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="conversionToISOstandardUnit">
      <DT><FONT color="#0000AA"><B>Conversion to metric:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
  </DL>
  </DD>
  <BR/>
</xsl:template>

-------- 

<!-- SPATIAL REPRESENTATION -->

<!-- Spatial Representation Information (B.2.6  MD_SpatialRepresentation - line156) -->
<xsl:template match="metadata/spatRepInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
    <xsl:choose>
      <xsl:when test="context()[GridSpatRep]">
        <DT><FONT color="#0000AA" size="3"><B>Spatial Representation - Grid:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[Georect]">
        <DT><FONT color="#0000AA" size="3"><B>Spatial Representation - Georectified Grid:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[Georef]">
        <DT><FONT color="#0000AA" size="3"><B>Spatial Representation - Georeferenceable Grid:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[VectSpatRep]">
        <DT><FONT color="#0000AA" size="3"><B>Spatial Representation - Vector:</B></FONT></DT>
      </xsl:when>
      <xsl:otherwise>
        <DT><FONT color="#0000AA" size="3"><B>Spatial Representation:</B></FONT></DT>
      </xsl:otherwise>
    </xsl:choose>
  <BR/><BR/>
  
  <DL>
    <DD>
      <xsl:apply-templates />
    </DD>
  </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- Grid Information (B.2.6  MD_GridSpatialRepresentation - line157, 
		MD_Georectified - line162, and MD_Georeferenceable - line170) -->
<xsl:template match="(GridSpatRep | Georect | Georef)">
    <xsl:for-each select="numDims">
      <DT><FONT color="#0000AA"><B>Number of dimensions:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:apply-templates select="axDimProps"/>
    <xsl:if test="context()[(numDims) and not (axDimProps)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="cellGeo">
      <DT><xsl:if test="context()[CellGeoCd/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Cell geometry:</B></FONT>
        <xsl:for-each select="CellGeoCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">point</xsl:when>
            <xsl:when test="context()[@value = '002']">area</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
    </xsl:for-each>
      <xsl:for-each select="ptInPixel">
        <DT><FONT color="#0000AA"><B>Point in pixel:</B></FONT>
        <xsl:for-each select="PixOrientCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">center</xsl:when>
            <xsl:when test="context()[@value = '002']">lower left</xsl:when>
            <xsl:when test="context()[@value = '003']">lower right</xsl:when>
            <xsl:when test="context()[@value = '004']">upper right</xsl:when>
            <xsl:when test="context()[@value = '005']">upper left</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ cellGeo | ptInPixel)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="tranParaAv">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Transformation parameters are available:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
      </DT>      
    </xsl:for-each>
      <xsl:for-each select="transDimDesc">
        <DT><FONT color="#0000AA"><B>Transformation dimension description:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="transDimMap">
        <DT><FONT color="#0000AA"><B>Tranformation dimension mapping:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ tranParaAv | transDimDesc | transDimMap)]"><BR/><BR/></xsl:if>
    
      <xsl:for-each select="chkPtAv">
        <DT><FONT color="#0000AA"><B>Check points are available:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="chkPtDesc">
        <DT><FONT color="#0000AA"><B>Check point description:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="cornerPts">
        <DT><FONT color="#0000AA"><B>Corner points:</B></FONT></DT>
	    <DD>
	    <DL>
	      <xsl:for-each select="coordinates">
	        <DT><FONT color="#0000AA"><B>Coordinates:</B></FONT> <xsl:value-of/></DT>
	        <BR/><BR/>
	      </xsl:for-each>
	      
	       <xsl:if test="MdCoRefSys">
	        <DT><FONT color="#0000AA"><B>Coordinate system for points:</B></FONT></DT>
	        <DD>
	        <DL>
	          <xsl:apply-templates select="MdCoRefSys"/>
	        </DL>
	        </DD>
	      </xsl:if>     
	    </DL>
	    </DD>
      </xsl:for-each>
      <xsl:for-each select="centerPt">
        <DT><FONT color="#0000AA"><B>Center point:</B></FONT></DT>
	    <DD>
	    <DL>
	      <xsl:for-each select="coordinates">
	        <DT><FONT color="#0000AA"><B>Coordinates:</B></FONT> <xsl:value-of/></DT>
	        <BR/><BR/>
	      </xsl:for-each>
	      
	       <xsl:if test="MdCoRefSys">
	        <DT><FONT color="#0000AA"><B>Point's coordinate system:</B></FONT></DT>
	        <DD>
	        <DL>
	          <xsl:apply-templates select="MdCoRefSys"/>
	        </DL>
	        </DD>
	      </xsl:if>     
	    </DL>
	    </DD>
      </xsl:for-each>
      <xsl:if test="context()[($any$ chkPtAv | chkPtDesc | cornerPts | centerPt)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="ctrlPtAv">
        <DT><FONT color="#0000AA"><B>Control points are available:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="orieParaAv">
        <DT><FONT color="#0000AA"><B>Orientation parameters are available:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
      </DT>      
      </xsl:for-each>
      <xsl:for-each select="orieParaDs">
        <DT><FONT color="#0000AA"><B>Orientation parameter description:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ ctrlPtAv | orieParaAv | orieParaDs)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="georefPars">
        <DT><FONT color="#0000AA"><B>Georeferencing parameters:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:apply-templates select="paraCit"/>
      <xsl:if test="context()[(georefPars) and not (paraCit)]"><BR/><BR/></xsl:if>
</xsl:template>

<!-- Dimension Information (B.2.6.1 MD_Dimension - line179) -->
<xsl:template match="axDimProps">
  <DD>
    <DT><FONT color="#0000AA"><B>Axis dimensions properties:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="Dimen">
	    <DT><FONT color="#0000AA"><B>Dimension:</B></FONT></DT>
	    <DD>
	    <DL>
	      <xsl:for-each select="dimName">
	        <DT><xsl:if test="context()[DimNameTypCd/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Dimension name:</B></FONT>
	        <xsl:for-each select="DimNameTypCd">
	          <xsl:choose>
	            <xsl:when test="context()[@value = '001']">row (y-axis)</xsl:when>
	            <xsl:when test="context()[@value = '002']">column (x-axis)</xsl:when>
	            <xsl:when test="context()[@value = '003']">vertical (z-axis)</xsl:when>
	            <xsl:when test="context()[@value = '004']">track (along direction of motion)</xsl:when>
	            <xsl:when test="context()[@value = '005']">cross track (perpendicular to direction of motion)</xsl:when>
	            <xsl:when test="context()[@value = '006']">scal line of sensor</xsl:when>
	            <xsl:when test="context()[@value = '007']">sample (element along scan line)</xsl:when>
	            <xsl:when test="context()[@value = '008']">time duration</xsl:when>
                   <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
		   </xsl:choose>
	        </xsl:for-each> 
	        </DT>
	      </xsl:for-each>
	      <xsl:for-each select="dimSize">
	        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Dimension size:</B></FONT> <xsl:value-of/></DT>
	      </xsl:for-each>
	      <xsl:for-each select="dimResol">
	        <DT><FONT color="#0000AA"><B>Resolution:</B></FONT></DT>
	        <DL>
	          <xsl:for-each select="value">
	            <DT><xsl:if test="context()[@Sync = 'TRUE']">
                    <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Distance:</B></FONT> <xsl:value-of/></DT>
	          </xsl:for-each>
                 <xsl:for-each select="uom/*/uomName">
	            <DT><xsl:if test="context()[@Sync = 'TRUE']">
                    <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Units of measure:</B></FONT> <xsl:value-of/></DT>
	          </xsl:for-each>
	          <xsl:for-each select="uom/*/conversionToISOstandardUnit">
	            <DT><xsl:if test="context()[@Sync = 'TRUE']">
                    <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Conversion to ISO units:</B></FONT> <xsl:value-of/></DT>
	          </xsl:for-each>
	        </DL>
	      </xsl:for-each>
	    </DL>
	    </DD>
	    <BR/>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Vector Information (B.2.6  MD_VectorSpatialRepresentation - line176) -->
<xsl:template match="VectSpatRep">
      <xsl:for-each select="topLvl">
        <DT><xsl:if test="context()[TopoLevCd/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Level of topology for this dataset:</B></FONT>
        <xsl:for-each select="TopoLevCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">geometry only</xsl:when>
            <xsl:when test="context()[@value = '002']">topology 1D</xsl:when>
            <xsl:when test="context()[@value = '003']">planar graph</xsl:when>
            <xsl:when test="context()[@value = '004']">full planar graph</xsl:when>
            <xsl:when test="context()[@value = '005']">surface graph</xsl:when>
            <xsl:when test="context()[@value = '006']">full surface graph</xsl:when>
            <xsl:when test="context()[@value = '007']">topology 3D</xsl:when>
            <xsl:when test="context()[@value = '008']">full topology 3D</xsl:when>
            <xsl:when test="context()[@value = '009']">abstract</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:apply-templates select="geometObjs"/>
      <xsl:if test="context()[(topLvl) and not (geometObjs)]"><BR/><BR/></xsl:if>
</xsl:template>

<!-- Geometric Object Information (B.2.6.2 MD_GeometricObjects - line183) -->
<xsl:template match="geometObjs">
  <DD>
    <DT><FONT color="#0000AA"><B>Geometric objects:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="@Name">
        <DT><FONT color="#006400">*</FONT><FONT color="#006400"><B>Name:</B> </FONT><xsl:value-of /></DT>      
      </xsl:for-each>
      <xsl:for-each select="geoObjTyp">
        <DT><xsl:if test="context()[GeoObjTypCd/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Object type:</B></FONT>
        <xsl:for-each select="GeoObjTypCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">complexes</xsl:when>
            <xsl:when test="context()[@value = '002']">composites</xsl:when>
            <xsl:when test="context()[@value = '003']">curve</xsl:when>
            <xsl:when test="context()[@value = '004']">point</xsl:when>
            <xsl:when test="context()[@value = '005']">solid</xsl:when>
            <xsl:when test="context()[@value = '006']">surface</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:for-each select="geoObjCnt">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Object count:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Identifier Information (B.2.7.2 MD_Identifier - line205) -->
<xsl:template match="(geoId | refSysID | projection | ellipsoid | datum | refSysName | 
      MdIdent | RS_Identifier | datumID)">
  <DD>
  <xsl:choose>
    <xsl:when test="../geoId">
        <DT><FONT color="#0000AA"><B>Geographic identifier:</B></FONT></DT>
    </xsl:when>
    <!-- can't use this method to add headings for refSysID, projection, ellipsoid, and datum
          because all exist together inside MdCoRefSys - also affects RefSystem -->
    <xsl:when test="../refSysName">
        <DT><FONT color="#0000AA"><B>Reference system name identifier:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../datumID">
        <DT><FONT color="#0000AA"><B>Vertical datum:</B></FONT></DT>
    </xsl:when>
    <!-- don't include an xsl:otherwise so the identCode value will appear correctly indented under the heading -->
  </xsl:choose>

    <DD>
    <DL>
      <xsl:for-each select="identCode">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Value:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>

      <xsl:apply-templates select="identAuth"/>
      
      <xsl:if test="context()[(identCode) and not (identAuth)]"><BR/><BR/></xsl:if>
    </DL>
    </DD>
  </DD>
</xsl:template>

-------- 

<!-- CONTENT INFORMATION -->

<!-- Content Information (B.2.8 MD_ContentInformation - line232) -->
<xsl:template match="contInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
    <xsl:choose>
      <xsl:when test="context()[ContInfo]">
        <DT><FONT color="#0000AA" size="3"><B>Content Information:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[FetCatDesc]">
        <DT><FONT color="#0000AA" size="3"><B>Content Information - Feature Catalogue Description:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[CovDesc]">
        <DT><FONT color="#0000AA" size="3"><B>Content Information - Coverage Description:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[ImgDesc]">
        <DT><FONT color="#0000AA" size="3"><B>Content Information - Image Description:</B></FONT></DT>
      </xsl:when>
      <xsl:otherwise>
        <DT><FONT color="#0000AA" size="3"><B>Content Information:</B></FONT></DT>
      </xsl:otherwise>
    </xsl:choose>
  <BR/><BR/>
  
  <DL>
    <DD>
        <xsl:apply-templates />
    </DD>
    </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- Content Information (B.2.8 MD_ContentInformation ABSTRACT - line232) -->
<xsl:template match="ContInfo">
    <DT><FONT color="#0000AA"><B>Content Description:</B></FONT> <xsl:value-of/></DT>
</xsl:template>

<!-- Feature Catalogue Description (B.2.8 MD_FeatureCatalogueDescription - line233) -->
<xsl:template match="FetCatDesc">
      <xsl:for-each select="catLang">
        <DT><FONT color="#0000AA"><B>Feature catalogue's language:</B></FONT>
            <xsl:apply-templates select="languageCode" />
        </DT>
      </xsl:for-each>
      <xsl:for-each select="incWithDS">
        <DT><FONT color="#0000AA"><B>Catalogue accompanies the dataset:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="compCode">
        <DT><FONT color="#0000AA"><B>Catalogue complies with ISO 19110:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:if test="context()[($any$ catLang | incWithDS | compCode)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="catFetTyps">
        <DT><FONT color="#0000AA"><B>Feature types in the dataset:</B></FONT></DT>
        <xsl:apply-templates />
      </xsl:for-each>

     <xsl:apply-templates select="catCitation"/>
</xsl:template>

<!-- Coverage Description (B.2.8 MD_CoverageDescription - line239) -->
<xsl:template match="CovDesc">
      <xsl:for-each select="contentTyp">
        <DT><FONT color="#0000AA"><B>Type of information:</B></FONT>
        <xsl:for-each select="ContentTypCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">image</xsl:when>
            <xsl:when test="context()[@value = '002']">thematic classification</xsl:when>
            <xsl:when test="context()[@value = '003']">physical measurement</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:for-each select="attDesc">
        <DT><FONT color="#0000AA"><B>Attribute described by cell values:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ attDesc | contentTyp)]"><BR/><BR/></xsl:if>

      <xsl:apply-templates select="covDim"/>
</xsl:template>

<!-- Range dimension Information (B.2.8.1 MD_RangeDimension - line256) -->
<xsl:template match="covDim">
    <DD>
    <xsl:choose>
      <xsl:when test="context()[RangeDim]">
        <DT><FONT color="#0000AA"><B>Range of cell values:</B></FONT></DT>
      </xsl:when>
      <xsl:when test="context()[Band]">
        <DT><FONT color="#0000AA"><B>Band information:</B></FONT></DT>
      </xsl:when>
      <xsl:otherwise>
        <DT><FONT color="#0000AA"><B>Cell value information:</B></FONT></DT>
      </xsl:otherwise>
    </xsl:choose>

    <DD>
    <DL>
      <xsl:for-each select="*/dimDescrp">
        <DT><FONT color="#0000AA"><B>Minimum and maximum values:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="*/seqID">
        <DT><FONT color="#0000AA"><B>Band identifier:</B></FONT></DT>
        <xsl:apply-templates select="*/seqID" />
      </xsl:if>
      <xsl:if test="context()[(*/dimDescrp) and not (*/seqID)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="Band">
      <xsl:for-each select="maxVal">
        <DT><FONT color="#0000AA"><B>Longest wavelength:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="minVal">
        <DT><FONT color="#0000AA"><B>Shortest wavelength:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="pkResp">
        <DT><FONT color="#0000AA"><B>Peak response wavelength:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="valUnit">
        <DT><FONT color="#0000AA"><B>Wavelength units:</B></FONT></DT>
        <xsl:apply-templates select="valUnit"/>
      </xsl:if>
      <xsl:if test="context()[($any$ maxVal | minVal | pkResp) and not (valUnit)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="bitsPerVal">
        <DT><FONT color="#0000AA"><B>Number of bits per value:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="toneGrad">
        <DT><FONT color="#0000AA"><B>Number of discrete values:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="sclFac">
        <DT><FONT color="#0000AA"><B>Scale factor applied to values:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="offset">
        <DT><FONT color="#0000AA"><B>Offset of values from zero:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ bitsPerVal | toneGrad | sclFac | offset)]"><BR/><BR/></xsl:if>
    </xsl:for-each>
    </DL>
    </DD>
    </DD>
</xsl:template>

<!-- Member Names (from ISO 19103 information in 19115 DTD) -->
<!-- Local Name and Scoped Name -->
<xsl:template match="(LocalName | ScopedName)">
    <DD>
    <DL>
      <xsl:for-each select="scope">
        <DT><FONT color="#0000AA"><B>Scope:</B></FONT> <xsl:value-of /></DT>
      </xsl:for-each>
    </DL>
    </DD>
    <BR/>
</xsl:template>

<!-- Type Name -->
<xsl:template match="TypeName">
    <DD>
    <DL>
      <xsl:for-each select="scope">
        <DT><FONT color="#0000AA"><B>Scope:</B></FONT> <xsl:value-of /></DT>
      </xsl:for-each>
      <xsl:for-each select="aName">
        <DT><FONT color="#0000AA"><B>Name:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
    <BR/>
</xsl:template>

<!-- Member Name -->
<xsl:template match="(MemberName | seqID)">
    <DD>
    <DL>
      <xsl:for-each select="scope">
        <DT><FONT color="#0000AA"><B>Scope:</B></FONT> <xsl:value-of /></DT>
      </xsl:for-each>
      <xsl:for-each select="aName">
        <DT><FONT color="#0000AA"><B>Name:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="attributeType">
        <DT><FONT color="#0000AA"><B>Attribute type:</B></FONT></DT>
        <DD>
        <DL>
          <xsl:for-each select="scope">
            <DT><FONT color="#0000AA"><B>Scope:</B></FONT> <xsl:value-of /></DT>
          </xsl:for-each>
          <xsl:for-each select="aName">
            <DT><FONT color="#0000AA"><B>Name:</B></FONT> <xsl:value-of/></DT>
          </xsl:for-each>
        </DL>
        </DD>
      </xsl:for-each>
    </DL>
    </DD>
    <BR/>
</xsl:template>

<!-- Image Description (B.2.8 MD_ImageDescription - line243) -->
<xsl:template match="ImgDesc">
      <xsl:for-each select="contentTyp">
        <DT><FONT color="#0000AA"><B>Type of information:</B></FONT>
        <xsl:for-each select="ContentTypCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">image</xsl:when>
            <xsl:when test="context()[@value = '002']">thematic classification</xsl:when>
            <xsl:when test="context()[@value = '003']">physical measurement</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:for-each select="attDesc">
        <DT><FONT color="#0000AA"><B>Attribute described by cell values:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ attDesc | contentTyp)]"><BR/><BR/></xsl:if>

      <xsl:apply-templates select="covDim"/>

      <xsl:for-each select="illElevAng">
        <DT><FONT color="#0000AA"><B>Illumination elevation angle:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="illAziAng">
        <DT><FONT color="#0000AA"><B>Illumination azimuth angle:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="imagCond">
        <DT><FONT color="#0000AA"><B>Imaging condition:</B></FONT>
        <xsl:for-each select="ImgCondCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">blurred image</xsl:when>
            <xsl:when test="context()[@value = '002']">cloud</xsl:when>
            <xsl:when test="context()[@value = '003']">degrading obliquity</xsl:when>
            <xsl:when test="context()[@value = '004']">fog</xsl:when>
            <xsl:when test="context()[@value = '005']">heavy smoke or dust</xsl:when>
            <xsl:when test="context()[@value = '006']">night</xsl:when>
            <xsl:when test="context()[@value = '007']">rain</xsl:when>
            <xsl:when test="context()[@value = '008']">semi-darkness</xsl:when>
            <xsl:when test="context()[@value = '009']">shadow</xsl:when>
            <xsl:when test="context()[@value = '010']">snow</xsl:when>
            <xsl:when test="context()[@value = '011']">terrain masking</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:for-each select="cloudCovPer">
        <DT><FONT color="#0000AA"><B>Percent cloud cover:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ illElevAng | illAziAng | imagCond | cloudCovPer)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="imagQuCode">
        <DT><FONT color="#0000AA"><B>Image quality code:</B></FONT></DT>
        <xsl:apply-templates />
      </xsl:for-each>

      <xsl:for-each select="prcTypCde">
        <DT><FONT color="#0000AA"><B>Processing level code:</B></FONT></DT>
        <xsl:apply-templates />
      </xsl:for-each>

      <xsl:for-each select="cmpGenQuan">
        <DT><FONT color="#0000AA"><B>Number of lossy compression cycles:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="trianInd">
        <DT><FONT color="#0000AA"><B>Triangulation has been performed:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="radCalDatAv">
        <DT><FONT color="#0000AA"><B>Radiometric calibration is available:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="camCalInAv">
        <DT><FONT color="#0000AA"><B>Camera calibration is available:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="filmDistInAv">
        <DT><FONT color="#0000AA"><B>Film distortion information is available:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="lensDistInAv">
        <DT><FONT color="#0000AA"><B>Lens distortion information is available:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:if test="context()[($any$ cmpGenQuan | trainInd | radCalDatAv | camCalInAv | filmDistInAv | lensDistInAv)]"><BR/><BR/></xsl:if>
</xsl:template>

-------- 

<!-- REFERENCE SYSTEM -->

<!-- Reference System Information (B.2.7 MD_ReferenceSystem - line186) -->
<xsl:template match="refSysInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
  <xsl:if expr="(this.selectNodes('/metadata/refSysInfo').length == 1)">
      <DT><FONT color="#0000AA" size="3"><B>Reference System Information:</B></FONT></DT>
  </xsl:if>
  <xsl:if expr="(this.selectNodes('/metadata/refSysInfo').length > 1)">
      <DT><FONT color="#0000AA" size="3"><B>
        Reference System Information - System <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>:
      </B></FONT></DT>
  </xsl:if>
  <BR/><BR/>
  
    <DL>
    <DD>
      <xsl:apply-templates select="RefSystem"/>

      <xsl:apply-templates select="MdCoRefSys"/>

	<!-- no support in the DIS DTD for RS_ReferenceSystem information and TMRefSys, SIRefSys, SCRefSys -->
    </DD>
    </DL>
    </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- Reference System Information (B.2.7 MD_ReferenceSystem - line186) -->
<xsl:template match="RefSystem">
      <xsl:if test="refSysID">
        <DT><FONT color="#0000AA"><B>Reference system identifier:</B></FONT></DT>
        <xsl:apply-templates select="refSysID"/>
      </xsl:if>

      <xsl:if test="context()[not (*)]"><BR/></xsl:if>
</xsl:template>

<!-- Metadata for Coordinate Systems (B.2.7 MD_CRS - line189) -->
<xsl:template match="MdCoRefSys">
      <xsl:if test="refSysID">
        <DT><FONT color="#0000AA"><B>Reference system identifier:</B></FONT></DT>
        <xsl:apply-templates select="refSysID"/>
      </xsl:if>

      <xsl:if test="projection">
        <DT><FONT color="#0000AA"><B>Projection identifier:</B></FONT></DT>
        <xsl:apply-templates select="projection"/>
      </xsl:if>
      
      <xsl:if test="ellipsoid">
        <DT><FONT color="#0000AA"><B>Ellipsoid identifier:</B></FONT></DT>
        <xsl:apply-templates select="ellipsoid"/>
      </xsl:if>
      
      <xsl:if test="datum">
        <DT><FONT color="#0000AA"><B>Datum identifier:</B></FONT></DT>
        <xsl:apply-templates select="datum"/>
      </xsl:if>

      <xsl:apply-templates select="projParas"/>

      <xsl:apply-templates select="ellParas"/>
      
      <xsl:if test="context()[not (*)]"><BR/></xsl:if>
</xsl:template>

<!-- Projection Parameter Information (B.2.7.5 MD_ProjectionParameters - line215) -->
<xsl:template match="projParas">
  <DD>
    <DT><FONT color="#0000AA"><B>Projection parameters:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="zone">
        <DT><FONT color="#0000AA"><B>Zone number:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="stanParal">
        <DT><FONT color="#0000AA"><B>Standard parallel:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="longCntMer">
        <DT><FONT color="#0000AA"><B>Longitude of central meridian:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="latProjOri">
        <DT><FONT color="#0000AA"><B>Latitude of projection origin:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="sclFacEqu">
        <DT><FONT color="#0000AA"><B>Scale factor at equator:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="hgtProsPt">
        <DT><FONT color="#0000AA"><B>Height of prospective point above surface:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="longProjCnt">
        <DT><FONT color="#0000AA"><B>Longitude of projection center:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="latProjCnt">
        <DT><FONT color="#0000AA"><B>Latitude of projection center:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="sclFacCnt">
        <DT><FONT color="#0000AA"><B>Scale factor at center line:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="stVrLongPl">
        <DT><FONT color="#0000AA"><B>Straight vertical longitude from pole:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="sclFacPrOr">
        <DT><FONT color="#0000AA"><B>Scale factor at projection origin:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ zoneNum | stanParal | longCntMer | latProjOri | sclFacEqu | 
      		hgtProsPt | longProjCnt | latProjCnt | sclFacCnt | stVrLongPl | sclFacPrOr)]"><BR/><BR/></xsl:if>

      <xsl:apply-templates select="obLnAziPars"/>

      <xsl:apply-templates select="obLnPtPars"/>
      
      <xsl:for-each select="falEastng">
        <DT><FONT color="#0000AA"><B>False easting:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="falNorthng">
        <DT><FONT color="#0000AA"><B>False northing:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="falENUnits">
        <DT><FONT color="#0000AA"><B>False easting northing units:</B></FONT></DT>
        <xsl:apply-templates select="falENUnits"/>
      </xsl:if>
      <xsl:if test="context()[($any$ falEastng | falNorthng) and not (falENUnits)]"><BR/><BR/></xsl:if>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Oblique Line Azimuth Information (B.2.7.3 MD_ObliqueLineAzimuth - line210) -->
<xsl:template match="obLnAziPars">
  <DD>
    <DT><FONT color="#0000AA"><B>Oblique line azimuth parameter:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="aziAngle">
        <DT><FONT color="#0000AA"><B>Azimuth angle:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="aziPtLong">
        <DT><FONT color="#0000AA"><B>Azimuth measure point longitude:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Oblique Line Point Information (B.2.7.4 MD_ObliqueLinePoint - line212) -->
<xsl:template match="obLnPtPars">
  <DD>
    <DT><FONT color="#0000AA"><B>Oblique line point parameter:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="obLineLat">
        <DT><FONT color="#0000AA"><B>Oblique line latitude:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="obLineLong">
        <DT><FONT color="#0000AA"><B>Oblique line longitude:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Ellipsoid Parameter Information (B.2.7.1 MD_EllipsoidParameters - line201) -->
<xsl:template match="ellParas">
  <DD>
    <DT><FONT color="#0000AA"><B>Ellipsoid parameters:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="semiMajAx">
        <DT><FONT color="#0000AA"><B>Semi-major axis:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="axisUnits">
        <DT><FONT color="#0000AA"><B>Axis units:</B></FONT></DT>
        <xsl:apply-templates select="axisUnits"/>
      </xsl:if>
      <xsl:for-each select="denFlatRat">
        <DT><FONT color="#0000AA"><B>Denominator of flattening ratio:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

-------- 

<!-- DATA QUALITY -->

<!-- Data Quality Information  (B.2.4 DQ_DataQuality - line78) -->
<xsl:template match="metadata/dqInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
  <xsl:if expr="(this.selectNodes('/metadata/dqInfo').length == 1)">
      <DT><FONT color="#0000AA" size="3"><B>Data Quality Information:</B></FONT></DT>
  </xsl:if>
  <xsl:if expr="(this.selectNodes('/metadata/dqInfo').length > 1)">
      <DT><FONT color="#0000AA" size="3"><B>
       Data Quality - Description <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>:
      </B></FONT></DT>
  </xsl:if>
  <BR/><BR/>

  <DL>
  <DD>
    <xsl:apply-templates select="dqScope"/>

    <xsl:apply-templates select="dataLineage"/>

    <xsl:for-each select="dqReport">
      <xsl:apply-templates select="node()"/>
    </xsl:for-each>
  </DD>
  </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- Scope Information (B.2.4.4 DQ_Scope - line138) -->
<xsl:template match="dqScope">
    <DD>
    <DT><FONT color="#0000AA"><B>Scope of quality information:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="scpLvl">
        <DT><FONT color="#0000AA"><B>Level of the data:</B></FONT>
            <xsl:apply-templates select="ScopeCd" />
        </DT>
      </xsl:for-each>
      <xsl:apply-templates select="scpLvlDesc"/>
      <xsl:if test="context()[(scpLvl) and not (scpLvlDesc)]"><BR/><BR/></xsl:if>

      <xsl:apply-templates select="scpExt"/>
    </DL>
    </DD>
    </DD>
</xsl:template>

<!-- Lineage Information (B.2.4.1 LI_Lineage - line82) -->
<xsl:template match="dataLineage">
  <DD>
  <DT><FONT color="#0000AA"><B>Lineage:</B></FONT></DT>
  <DD>
  <DL>
    <xsl:for-each select="statement">
      <DT><FONT color="#0000AA"><B>Lineage statement:</B></FONT></DT>
      <PRE ID="original"><xsl:value-of /></PRE>
      <SCRIPT>fix(original)</SCRIPT>
      <BR/>
    </xsl:for-each>

    <xsl:apply-templates select="prcStep"/>

    <xsl:apply-templates select="dataSource"/>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Process Step Information (B.2.4.1.1 LI_ProcessStep - line86) -->
<xsl:template match="(prcStep | srcStep)">
  <DD>
  <DT><FONT color="#0000AA"><B>Process step:</B></FONT></DT>
  <DD>
  <DL>
    <xsl:for-each select="stepDateTm">
      <DT><FONT color="#0000AA"><B>When the process occurred:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="stepDesc">
      <DT><FONT color="#0000AA"><B>Description:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="stepRat">
      <DT><FONT color="#0000AA"><B>Rationale:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ stepDateTm | stepDesc | stepRat)]"><BR/><BR/></xsl:if>

    <xsl:apply-templates select="stepProc"/>

    <xsl:if test="context()[not (../srcStep)]">
      <xsl:apply-templates select="stepSrc"/>
    </xsl:if>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Source Information (B.2.4.1.2 LI_Source - line92) -->
<xsl:template match="(dataSource | stepSrc)">
  <DD>
  <DT><FONT color="#0000AA"><B>Source data:</B></FONT></DT>
  <DD>
  <DL>
      <xsl:for-each select="srcDesc">
        <DT><FONT color="#0000AA"><B>Level of the source data:</B></FONT> <xsl:value-of/></DT>
        <BR/><BR/>
      </xsl:for-each>
      
      <xsl:apply-templates select="srcScale"/>
      
      <xsl:apply-templates select="srcCitatn"/>
      
      <xsl:for-each select="srcRefSys">
        <DT><FONT color="#0000AA"><B>Source reference system:</B></FONT></DT>
	    <DD>
	    <DL>
	      <xsl:apply-templates select="RefSystem"/>
	
	      <xsl:apply-templates select="MdCoRefSys"/>
	    </DL>
	    </DD>
      </xsl:for-each>
      
      <xsl:apply-templates select="srcExt"/>

      <xsl:if test="context()[not (../stepSrc)]">
        <xsl:apply-templates select="srcStep"/>
      </xsl:if>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Data Quality Element Information (B.2.4.2 DQ_Element - line99) -->
<xsl:template match="(DQComplete | DQCompComm | DQCompOm | DQLogConsis | DQConcConsis | 
      DQDomConsis | DQFormConsis | DQTopConsis | DQPosAcc | DQAbsExtPosAcc | 
      DQGridDataPosAcc | DQRelIntPosAcc | DQTempAcc | DQAccTimeMeas | DQTempConsis | 
      DQTempValid | DQThemAcc | DQThemClassCor | DQNonQuanAttAcc | DQQuanAttAcc)">
  <DD>
  <xsl:choose>
    <xsl:when test="../DQComplete">
        <DT><FONT color="#0000AA"><B>Data quality report - Completeness:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQCompComm">
        <DT><FONT color="#0000AA"><B>Data quality report - Completeness commission:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQCompOm">
        <DT><FONT color="#0000AA"><B>Data quality report - Completeness omission:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQLogConsis">
        <DT><FONT color="#0000AA"><B>Data quality report - Logical consistency:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQConcConsis">
        <DT><FONT color="#0000AA"><B>Data quality report - Conceptual consistency:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQDomConsis">
        <DT><FONT color="#0000AA"><B>Data quality report - Domain consistency:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQFormConsis">
        <DT><FONT color="#0000AA"><B>Data quality report - Formal consistency:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQTopConsis">
        <DT><FONT color="#0000AA"><B>Data quality report - Topological consistency:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQPosAcc">
        <DT><FONT color="#0000AA"><B>Data quality report - Positional accuracy:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQAbsExtPosAcc">
        <DT><FONT color="#0000AA"><B>Data quality report - Absolute external positional accuracy:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQGridDataPosAcc">
        <DT><FONT color="#0000AA"><B>Data quality report - Gridded data positional accuracy:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQRelIntPosAcc">
        <DT><FONT color="#0000AA"><B>Data quality report - Relative internal positional accuracy:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQTempAcc">
        <DT><FONT color="#0000AA"><B>Data quality report - Temporal accuracy:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQAccTimeMeas">
        <DT><FONT color="#0000AA"><B>Data quality report - Accuracy of a time measurement:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQTempConsis">
        <DT><FONT color="#0000AA"><B>Data quality report - Temporal consistency:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQTempValid">
        <DT><FONT color="#0000AA"><B>Data quality report - Temporal validity:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQThemAcc">
        <DT><FONT color="#0000AA"><B>Data quality report - Thematic accuracy:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQThemClassCor">
        <DT><FONT color="#0000AA"><B>Data quality report - Thematic classification correctness:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQNonQuanAttAcc">
        <DT><FONT color="#0000AA"><B>Data quality report - Non quantitative attribute accuracy:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../DQQuanAttAcc">
        <DT><FONT color="#0000AA"><B>Data quality report - Quantitative attribute accuracy:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
        <DT><FONT color="#0000AA"><B>Data quality report:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>

  <DD>
  <DL>
    <xsl:for-each select="measName">
      <DT><FONT color="#0000AA"><B>Name of the test:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="evalMethType">
      <DT><FONT color="#0000AA"><B>Type of test:</B></FONT>
        <xsl:for-each select="EvalMethTypeCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">direct internal</xsl:when>
            <xsl:when test="context()[@value = '002']">direct external</xsl:when>
            <xsl:when test="context()[@value = '003']">indirect</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
    </xsl:for-each>
    <xsl:for-each select="measDateTm">
      <DT><FONT color="#0000AA"><B>Date of the test:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ measName | evalMethType | measDateTm)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="measDesc">
      <DT><FONT color="#0000AA"><B>Measure produced by the test:</B></FONT> <xsl:value-of/></DT>
      <BR/><BR/>
    </xsl:for-each>

    <xsl:for-each select="evalMethDesc">
      <DT><FONT color="#0000AA"><B>Evaluation method:</B></FONT> <xsl:value-of/></DT>
      <BR/><BR/>
    </xsl:for-each>

    <xsl:for-each select="measId">
      <DT><FONT color="#0000AA"><B>Registered standard procedure:</B></FONT></DT>
      <xsl:apply-templates select="node()"/>
    </xsl:for-each>

    <xsl:apply-templates select="evalProc"/>

    <xsl:for-each select="measResult">
        <xsl:for-each select="Result">
          <DT><FONT color="#0000AA"><B>General test results:</B></FONT> <xsl:value-of/></DT>
          <BR/><BR/>
        </xsl:for-each>
        <xsl:apply-templates select="ConResult"/>
        <xsl:apply-templates select="QuanResult"/>
    </xsl:for-each>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Conformance Result Information (B.2.4.3 DQ_ConformanceResult - line129) -->
<xsl:template match="ConResult">
  <DD>
  <DT><FONT color="#0000AA"><B>Conformance test results:</B></FONT> </DT>
  <DD>
  <DL>
    <xsl:for-each select="conPass">
      <DT><FONT color="#0000AA"><B>Test passed:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
      </DT>      
    </xsl:for-each>
    <xsl:for-each select="conExpl">
      <DT><FONT color="#0000AA"><B>Meaning of the result:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ conPass | conExpl)]"><BR/><BR/></xsl:if>

    <xsl:apply-templates select="conSpec"/>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Quantitative Result Information (B.2.4.3 DQ_QuantitativeResult - line133) -->
<xsl:template match="QuanResult">
  <DD>
  <DT><FONT color="#0000AA"><B>Quality test results:</B></FONT> </DT>
  <DD>
  <DL>
    <xsl:for-each select="quanValType">
      <DT><FONT color="#0000AA"><B>Values required for conformance:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="quanValUnit">
      <DT><FONT color="#0000AA"><B>Value units:</B></FONT></DT>
      <DD>
      <DL>
          <!-- value will be shown regardless of the subelement Integer, Real, or Decimal -->
          <xsl:for-each select="value">
            <DT><FONT color="#0000AA"><B>Precision:</B></FONT> <xsl:value-of/></DT>
          </xsl:for-each>

          <xsl:apply-templates select="uom"/>

          <xsl:if test="context()[(value) and not (uom)]"><BR/><BR/></xsl:if>
      </DL>
      </DD>
    </xsl:for-each>
    <xsl:if test="context()[(quanValType) and not (quanValUnit)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="errStat">
      <DT><FONT color="#0000AA"><B>Statistical method used to determine values:</B></FONT> <xsl:value-of/></DT>
      <BR/><BR/>
    </xsl:for-each>

    <!-- Standard uses short name quanVal, but DTD uses quanValue -->
    <xsl:if test="context()[($any$ quanValue | quanVal)]">
      <xsl:for-each select="quanValue">
        <DT><FONT color="#0000AA"><B>Result value:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="quanVal">
        <DT><FONT color="#0000AA"><B>Result value:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <BR/><BR/>
    </xsl:if>
  </DL>
  </DD>
  </DD>
</xsl:template>

-------- 

<!-- DISTRIBUTION INFORMATION -->

<!-- Distribution Information (B.2.10 MD_Distribution - line270) -->
<xsl:template match="metadata/distInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
  <DT><FONT color="#0000AA" size="3"><B>Distribution Information:</B></FONT></DT>
  <BR/><BR/>

  <DL>
  <DD>
      <xsl:apply-templates select="distributor"/>

      <xsl:apply-templates select="distFormat"/>

      <xsl:apply-templates select="distTranOps"/>
  </DD>
  </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>


<!-- Distributor Information (B.2.10.2 MD_Distributor - line279) -->
<xsl:template match="(distributor | formatDist)">
  <DD>
  <DT><FONT color="#0000AA"><B>Distributor:</B></FONT></DT>

  <DD>
  <DL>
    <xsl:apply-templates select="distorCont"/>

    <xsl:if test="context()[not ((../../dsFormat) or (../../distorFormat) or (../../distFormat))]">
      <xsl:apply-templates select="distorFormat"/>
    </xsl:if>

    <xsl:apply-templates select="distorOrdPrc"/>

    <xsl:apply-templates select="distorTran"/>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Format Information (B.2.10.3 MD_Format - line284) -->
<xsl:template match="(dsFormat | distorFormat | distFormat)">
  <DD>
  <xsl:choose>
    <xsl:when test="../dsFormat">
        <DT><FONT color="#0000AA"><B>Resource format:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../distorFormat">
        <DT><FONT color="#0000AA"><B>Available format:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../distFormat">
        <DT><FONT color="#0000AA"><B>Distribution format:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
        <DT><FONT color="#0000AA"><B>Format:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>

  <DD>
  <DL>
    <xsl:for-each select="formatName">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Format name:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="formatVer">
      <DT><FONT color="#0000AA"><B>Format version:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="formatAmdNum">
      <DT><FONT color="#0000AA"><B>Format amendment number:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="formatSpec">
      <DT><FONT color="#0000AA"><B>Format specification:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="fileDecmTech">
      <DT><FONT color="#0000AA"><B>File decompression technique:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ formatName | formatVer | formatAmdNum | formatSpec | fileDecmTech)]"><BR/><BR/></xsl:if>

    <xsl:if test="context()[not ((../../distributor) or (../../formatDist))]">
      <xsl:apply-templates select="formatDist"/>
    </xsl:if>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Standard Order Process Information (B.2.10.5 MD_StandardOrderProcess - line298) -->
<xsl:template match="distorOrdPrc">
  <DD>
  <DT><FONT color="#0000AA"><B>Ordering process:</B></FONT></DT>
  <DD>
  <DL>
    <xsl:for-each select="resFees">
      <DT><FONT color="#0000AA"><B>Terms and fees:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="planAvDtTm">
      <DT><FONT color="#0000AA"><B>Date of availability:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="ordTurn">
      <DT><FONT color="#0000AA"><B>Turnaround time:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="ordInstr">
      <DT><FONT color="#0000AA"><B>Instructions:</B></FONT></DT>
      <PRE ID="original"><xsl:value-of /></PRE>
      <SCRIPT>fix(original)</SCRIPT>
    </xsl:for-each>
  </DL>
  </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Digital Transfer Options Information (B.2.10.1 MD_DigitalTransferOptions - line274) -->
<xsl:template match="(distorTran | distTranOps)">
  <DD>
  <DT><FONT color="#0000AA"><B>Transfer options:</B></FONT></DT>

  <DD>
  <DL>
    <xsl:for-each select="transSize">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Transfer size:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="unitsODist">
      <DT><FONT color="#0000AA"><B>Units of distribution (e.g., tiles):</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ transSize | unitsODist)]"><BR/><BR/></xsl:if>

    <xsl:apply-templates select="onLineSrc"/>

    <xsl:apply-templates select="offLineMed"/>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Medium Information (B.2.10.4 MD_Medium - line291) -->
<xsl:template match="offLineMed">
  <DD>
  <DT><FONT color="#0000AA"><B>Medium of distribution:</B></FONT></DT>
  <DD>
  <DL>
    <xsl:for-each select="medName">
      <DT><FONT color="#0000AA"><B>Medium name:</B></FONT>
        <xsl:for-each select="MedNameCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">CD-ROM</xsl:when>
            <xsl:when test="context()[@value = '002']">DVD</xsl:when>
            <xsl:when test="context()[@value = '003']">DVD-ROM</xsl:when>
            <xsl:when test="context()[@value = '004']">3.5 inch floppy disk</xsl:when>
            <xsl:when test="context()[@value = '005']">5.25 inch floppy disk</xsl:when>
            <xsl:when test="context()[@value = '006']">7 track tape</xsl:when>
            <xsl:when test="context()[@value = '007']">9 track tape</xsl:when>
            <xsl:when test="context()[@value = '008']">3480 cartridge</xsl:when>
            <xsl:when test="context()[@value = '009']">3490 cartridge</xsl:when>
            <xsl:when test="context()[@value = '010']">3580 cartridge</xsl:when>
            <xsl:when test="context()[@value = '011']">4mm cartridge tape</xsl:when>
            <xsl:when test="context()[@value = '012']">8mm cartridge tape</xsl:when>
            <xsl:when test="context()[@value = '013']">0.25 inch cartridge tape</xsl:when>
            <xsl:when test="context()[@value = '014']">digital linear tape</xsl:when>
            <xsl:when test="context()[@value = '015']">online link</xsl:when>
            <xsl:when test="context()[@value = '016']">satellite link</xsl:when>
            <xsl:when test="context()[@value = '017']">telephone link</xsl:when>
            <xsl:when test="context()[@value = '018']">hardcopy</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
    </xsl:for-each>
    <xsl:for-each select="medVol">
      <DT><FONT color="#0000AA"><B>Number of media items:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ medName | medVol)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="medFormat">
      <DT><FONT color="#0000AA"><B>How the medium is written:</B></FONT>
        <xsl:for-each select="MedFormCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">cpio</xsl:when>
            <xsl:when test="context()[@value = '002']">tar</xsl:when>
            <xsl:when test="context()[@value = '003']">high sierra file system</xsl:when>
            <xsl:when test="context()[@value = '004']">iso9660 (CD-ROM)</xsl:when>
            <xsl:when test="context()[@value = '005']">iso9660 Rock Ridge</xsl:when>
            <xsl:when test="context()[@value = '006']">iso9660 Apple HFS</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
      </DT>
    </xsl:for-each>
    <xsl:for-each select="medDensity">
      <DT><FONT color="#0000AA"><B>Recording density:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="medDenUnits">
      <DT><FONT color="#0000AA"><B>Density units of measure:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ medDensity | medDenUnits | medFormat)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="medNote">
      <DT><FONT color="#0000AA"><B>Limitations for using the medium:</B></FONT> <xsl:value-of/></DT>
      <xsl:if test="context()[end()]"><BR/><BR/></xsl:if>
    </xsl:for-each>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Portrayal Catalogue Reference (B.2.9 MD_PortrayalCatalogueReference - line268) -->
<xsl:template match="porCatInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
  <xsl:if expr="(this.selectNodes('/metadata/porCatInfo').length == 1)">
      <DT><FONT color="#0000AA" size="3"><B>Portrayal Catalogue Reference:</B></FONT></DT>
  </xsl:if>
  <xsl:if expr="(this.selectNodes('/metadata/porCatInfo').length > 1)">
      <DT><FONT color="#0000AA" size="3"><B>
        Portrayal Catalogue Reference - Catalogue <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>:
      </B></FONT></DT>
  </xsl:if>
  <BR/><BR/>

  <DL>
  <DD>
    <xsl:apply-templates select="portCatCit"/>
  </DD>
  </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

-------- 

<!-- APPLICATION SCHEMA -->

<!-- Application schema Information (B.2.12 MD_ApplicationSchemaInformation - line320) -->
<xsl:template match="appSchInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
  <xsl:if expr="(this.selectNodes('/metadata/appSchInfo').length == 1)">
      <DT><FONT color="#0000AA" size="3"><B>Application Schema Information:</B></FONT></DT>
  </xsl:if>
  <xsl:if expr="(this.selectNodes('/metadata/appSchInfo').length > 1)">
      <DT><FONT color="#0000AA" size="3"><B>
        Application Schema Information - Schema <xsl:eval>formatIndex(childNumber(this), "1")</xsl:eval>:
      </B></FONT></DT>
  </xsl:if>
  <BR/><BR/>
    
    <DL>
    <DD>
      <xsl:apply-templates select="asName"/>

      <xsl:for-each select="asSchLang">
        <DT><FONT color="#0000AA"><B>Schema language used:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="asCstLang">
        <DT><FONT color="#0000AA"><B>Formal language used in schema:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ asSchLang | asCstLang)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="asAscii">
        <DT><FONT color="#0000AA"><B>Schema ASCII file:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="asGraFile">
        <DT><FONT color="#0000AA"><B>Schema graphics file:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="asSwDevFile">
        <DT><FONT color="#0000AA"><B>Schema software development file:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="asSwDevFiFt">
        <DT><FONT color="#0000AA"><B>Software development file format:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ asAscii | asGraFile | asSwDevFile | asSwDevFiFt)]"><BR/><BR/></xsl:if>

      <xsl:apply-templates select="featCatSup"/>
    </DD>
    </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- Spatial Attribute Supplement Information (B.2.12.2 MD_SpatialAttributeSupplement - line332) -->
<xsl:template match="featCatSup">
  <DD>
    <DT><FONT color="#0000AA"><B>Feature catalogue supplement:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:apply-templates select="featTypeList"/>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Feature Type List Information (B.2.12.1 MD_FeatureTypeList - line329 -->
<xsl:template match="featTypeList">
  <DD>
    <DT><FONT color="#0000AA"><B>Feature type list:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="spatObj">
        <DT><FONT color="#0000AA"><B>Spatial object:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="spatSchName">
        <DT><FONT color="#0000AA"><B>Spatial schema name:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

-------- 

<!-- METADATA EXTENSIONS -->

<!-- Metadata Extension Information (B.2.11 MD_MetadataExtensionInformation - line303) -->
<xsl:template match="mdExtInfo">
  <A><xsl:attribute name="NAME"><xsl:eval>uniqueID(this)</xsl:eval></xsl:attribute><HR/></A>
  <DL>
    <DT><FONT color="#0000AA" size="3"><B>Metadata extension information:</B></FONT></DT>
    <BR/><BR/>

    <DL>
    <DD>
      <xsl:apply-templates select="extOnRes"/>

      <xsl:apply-templates select="extEleInfo"/>
    </DD>
    </DL>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- Extended Element Information (B.2.11.1 MD_ExtendedElementInformation - line306) -->
<xsl:template match="extEleInfo">
    <DD>
    <DT><FONT color="#0000AA"><B>Extended element information:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="extEleName">
        <DT><FONT color="#0000AA"><B>Element name:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="extShortName">
        <DT><FONT color="#0000AA"><B>Short name:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="extDomCode">
        <DT><FONT color="#0000AA"><B>Codelist value:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="extEleDef">
        <DT><FONT color="#0000AA"><B>Definition:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ extEleName | extShortName | extDomCode | extEleDef)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="extEleOb">
        <DT><FONT color="#0000AA"><B>Obligation:</B></FONT>
        <xsl:for-each select="ObCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">mandatory</xsl:when>
            <xsl:when test="context()[@value = '002']">optional</xsl:when>
            <xsl:when test="context()[@value = '003']">conditional</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:for-each select="extEleCond">
        <DT><FONT color="#0000AA"><B>Condition:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="extEleMxOc">
        <DT><FONT color="#0000AA"><B>Maximum occurrence:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="eleDataType">
        <DT><FONT color="#0000AA"><B>Data type:</B></FONT>
        <xsl:for-each select="DatatypeCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">class</xsl:when>
            <xsl:when test="context()[@value = '002']">codelist</xsl:when>
            <xsl:when test="context()[@value = '003']">enumeration</xsl:when>
            <xsl:when test="context()[@value = '004']">codelist element</xsl:when>
            <xsl:when test="context()[@value = '005']">abstract class</xsl:when>
            <xsl:when test="context()[@value = '006']">aggregate class</xsl:when>
            <xsl:when test="context()[@value = '007']">specified class</xsl:when>
            <xsl:when test="context()[@value = '008']">datatype class</xsl:when>
            <xsl:when test="context()[@value = '009']">interface class</xsl:when>
            <xsl:when test="context()[@value = '010']">union class</xsl:when>
            <xsl:when test="context()[@value = '011']">meta class</xsl:when>
            <xsl:when test="context()[@value = '012']">type class</xsl:when>
            <xsl:when test="context()[@value = '013']">character string</xsl:when>
            <xsl:when test="context()[@value = '014']">integer</xsl:when>
            <xsl:when test="context()[@value = '015']">association</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
      <xsl:for-each select="extEleDomVal">
        <DT><FONT color="#0000AA"><B>Domain:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ extEleOb | extEleCond | extEleMxOc | eleDataType | extEleDomVal)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="extEleParEnt">
        <DT><FONT color="#0000AA"><B>Parent element:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="extEleRule">
        <DT><FONT color="#0000AA"><B>Relationship to existing elements:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="extEleRat">
        <DT><FONT color="#0000AA"><B>Why the element was created:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ extEleParEnt | extEleRule | extEleRat)]"><BR/><BR/></xsl:if>

      <xsl:apply-templates select="extEleSrc"/>
    </DL>
    </DD>
    </DD>
</xsl:template>

-------- 

<!-- TEMPLATES FOR DATA TYPE CLASSES -->

<!-- CITATION AND CONTACT INFORMATION -->

<!-- Citation Information (B.3.2 CI_Citation - line359) -->
<xsl:template match="(idCitation | thesaName | identAuth | srcCitatn | evalProc | 
      conSpec | paraCit | portCatCit | catCitation | asName)">
  <DD>
  <xsl:choose>
    <xsl:when test="../idCitation">
      <DT><FONT color="#0000AA"><B>Citation:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../thesaName">
      <DT><FONT color="#0000AA"><B>Thesaurus name:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../identAuth">
      <DT><FONT color="#0000AA"><B>Reference that defines the value:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../srcCitatn">
      <DT><FONT color="#0000AA"><B>Source citation:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../evalProc">
      <DT><FONT color="#0000AA"><B>Description of evaluation procedure:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../conSpec">
      <DT><FONT color="#0000AA"><B>Description of conformance requirements:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../paraCit">
      <DT><FONT color="#0000AA"><B>Georeferencing parameters citation:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../portCatCit">
      <DT><FONT color="#0000AA"><B>Portrayal catalogue citation:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../catCitation">
      <DT><FONT color="#0000AA"><B>Feature catalogue citation:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../asName">
      <DT><FONT color="#0000AA"><B>Application schema name:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
      <DT><FONT color="#0000AA"><B>Citation:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>

  <DD>
  <DL>
    <xsl:for-each select="resTitle">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Title:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="resAltTitle">
	    <DT><FONT color="#0000AA"><B>Alternate titles:</B></FONT> 
	      <xsl:for-each select="resAltTitle[text()]">
	        <xsl:value-of/><xsl:if test="context()[not(end())]">, </xsl:if>
	      </xsl:for-each>
	    </DT>
    </xsl:if>
    <xsl:if test="context()[($any$ resTitle | resAltTitle)]"><BR/><BR/></xsl:if>

    <xsl:apply-templates select="resRefDate"/>

    <xsl:for-each select="resEd">
      <DT><FONT color="#0000AA"><B>Edition:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="resEdDate">
      <DT><FONT color="#0000AA"><B>Edition date:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="presForm">
      <DT><xsl:if test="context()[PresFormCd/@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Presentation format:</B></FONT>
        <xsl:for-each select="PresFormCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">digital document</xsl:when>
            <xsl:when test="context()[@value = '002']">hardcopy document</xsl:when>
            <xsl:when test="context()[@value = '003']">digital image</xsl:when>
            <xsl:when test="context()[@value = '004']">hardcopy image</xsl:when>
            <xsl:when test="context()[@value = '005']">digital map</xsl:when>
            <xsl:when test="context()[@value = '006']">hardcopy map</xsl:when>
            <xsl:when test="context()[@value = '007']">digital model</xsl:when>
            <xsl:when test="context()[@value = '008']">hardcopy model</xsl:when>
            <xsl:when test="context()[@value = '009']">digital profile</xsl:when>
            <xsl:when test="context()[@value = '010']">hardcopy profile</xsl:when>
            <xsl:when test="context()[@value = '011']">digital table</xsl:when>
            <xsl:when test="context()[@value = '012']">hardcopy table</xsl:when>
            <xsl:when test="context()[@value = '013']">digital video</xsl:when>
            <xsl:when test="context()[@value = '014']">hardcopy video</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ resEd | resEdDate | presForm)]"><BR/><BR/></xsl:if>
    
    <xsl:for-each select="collTitle">
      <DT><FONT color="#0000AA"><B>Collection title:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:apply-templates select="datasetSeries"/>
    <xsl:if test="context()[(collTitle) and not (datasetSeries)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="isbn">
      <DT><FONT color="#0000AA"><B>ISBN:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="issn">
      <DT><FONT color="#0000AA"><B>ISSN:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="citId">
      <DT><FONT color="#0000AA"><B>Unique resource identifier:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="citIdType">
      <DT><FONT color="#0000AA"><B>Type of identifier:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ isbn | issn | citId | citIdType)]"><BR/><BR/></xsl:if>

    <xsl:for-each select="otherCitDet">
      <DT><FONT color="#0000AA"><B>Other citation details:</B></FONT> <xsl:value-of/></DT>
      <BR/><BR/>
    </xsl:for-each>
    
    <xsl:apply-templates select="citRespParty"/>

    <xsl:if test="context()[not (text()) and not(*)]"><BR/></xsl:if>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Date Information (B.3.2.3 CI_Date) -->
<xsl:template match="resRefDate">
  <DD>
    <DT><FONT color="#0000AA"><B>Reference date:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="refDate">
        <DT><FONT color="#0000AA"><B>Date:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="refDateType">
        <DT><FONT color="#0000AA"><B>Type of date:</B></FONT>
        <xsl:for-each select="DateTypCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">creation</xsl:when>
            <xsl:when test="context()[@value = '002']">publication</xsl:when>
            <xsl:when test="context()[@value = '003']">revision</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Responsible Party Information (B.3.2 CI_ResponsibleParty - line374) -->
<xsl:template match="(mdContact | idPoC | usrCntInfo | stepProc | distorCont | 
      citRespParty | extEleSrc)">
  <DD>
  <xsl:choose>
    <xsl:when test="../mdContact">
      <DT><FONT color="#0000AA"><B>Metadata contact:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../idPoC">
      <DT><FONT color="#0000AA"><B>Point of contact:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../usrCntInfo">
      <DT><FONT color="#0000AA"><B>Party using the resource:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../stepProc">
      <DT><FONT color="#0000AA"><B>Process contact:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../distorCont">
      <DT><FONT color="#0000AA"><B>Contact information:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../citRespParty">
      <DT><FONT color="#0000AA"><B>Party responsible for the resource:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../extEleSrc">
      <DT><FONT color="#0000AA"><B>Extension source:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
      <DT><FONT color="#0000AA"><B>Contact information:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>

  <DD>
  <DL>
    <xsl:for-each select="rpIndName">
      <DT><FONT color="#0000AA"><B>Individual's name:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="rpOrgName">
      <DT><FONT color="#0000AA"><B>Organization's name:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="rpPosName">
      <DT><FONT color="#0000AA"><B>Contact's position:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="role">
      <DT><FONT color="#0000AA"><B>Contact's role:</B></FONT>
        <xsl:for-each select="RoleCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">resource provider</xsl:when>
            <xsl:when test="context()[@value = '002']">custodian</xsl:when>
            <xsl:when test="context()[@value = '003']">owner</xsl:when>
            <xsl:when test="context()[@value = '004']">user</xsl:when>
            <xsl:when test="context()[@value = '005']">distributor</xsl:when>
            <xsl:when test="context()[@value = '006']">originator</xsl:when>
            <xsl:when test="context()[@value = '007']">point of contact</xsl:when>
            <xsl:when test="context()[@value = '008']">principal investigator</xsl:when>
            <xsl:when test="context()[@value = '009']">processor</xsl:when>
            <xsl:when test="context()[@value = '010']">publisher</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
    </xsl:for-each>
    <xsl:if test="context()[($any$ rpIndName | rpOrgName | rpPosName | role)]"><BR/><BR/></xsl:if>

    <xsl:apply-templates select="rpCntInfo"/>
  </DL>
  </DD>
  </DD>
</xsl:template>

<!-- Contact Information (B.3.2.2 CI_Contact - line387) -->
<xsl:template match="rpCntInfo">
  <DD>
    <DT><FONT color="#0000AA"><B>Contact information:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:apply-templates select="cntPhone"/>

      <xsl:apply-templates select="cntAddress"/>

      <xsl:apply-templates select="cntOnlineRes"/>

      <xsl:for-each select="cntHours">
        <DT><FONT color="#0000AA"><B>Hours of service:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="cntInstr">
        <DT><FONT color="#0000AA"><B>Contact instructions:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ cntHours | cntInstr)]"><BR/><BR/></xsl:if>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Telephone Information (B.3.2.6 CI_Telephone - line407) -->
<xsl:template match="cntPhone">
  <DD>
    <DT><FONT color="#0000AA"><B>Phone:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="voiceNum">
        <DT><FONT color="#0000AA"><B>Voice:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="faxNum">
        <DT><FONT color="#0000AA"><B>Fax:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Address Information (B.3.2.1 CI_Address - line380) -->
<xsl:template match="cntAddress">
  <DD>
    <DT><FONT color="#0000AA"><B>Address:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="delPoint">
        <DT><FONT color="#0000AA"><B>Delivery point:</B></FONT></DT>
        <PRE ID="original"><xsl:value-of /></PRE>
        <SCRIPT>fix(original)</SCRIPT>
      </xsl:for-each>
      <xsl:for-each select="city">
        <DT><FONT color="#0000AA"><B>City:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="adminArea">
        <DT><FONT color="#0000AA"><B>Administrative area:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="postCode">
        <DT><FONT color="#0000AA"><B>Postal code:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="country">
        <DT><FONT color="#0000AA"><B>Country:</B></FONT>
          <!-- 2 letter country codes from ISO 3661-1, in alphabetical order by name -->
          <xsl:choose>
            <xsl:when test="context()[(. = 'af') or (. = 'AF')]">Afghanistan</xsl:when>
            <xsl:when test="context()[(. = 'al') or (. = 'AL')]">Albania</xsl:when>
            <xsl:when test="context()[(. = 'dz') or (. = 'DZ')]">Algeria</xsl:when>
            <xsl:when test="context()[(. = 'as') or (. = 'AS')]">American Samoa</xsl:when>
            <xsl:when test="context()[(. = 'ad') or (. = 'AD')]">Andorra</xsl:when>
            <xsl:when test="context()[(. = 'ao') or (. = 'AO')]">Angola</xsl:when>
            <xsl:when test="context()[(. = 'ai') or (. = 'AI')]">Anguilla</xsl:when>
            <xsl:when test="context()[(. = 'aq') or (. = 'AQ')]">Antarctica</xsl:when>
            <xsl:when test="context()[(. = 'ag') or (. = 'AG')]">Antigua and Barbuda</xsl:when>
            <xsl:when test="context()[(. = 'ar') or (. = 'AR')]">Argentina</xsl:when>
            <xsl:when test="context()[(. = 'am') or (. = 'AM')]">Armenia</xsl:when>
            <xsl:when test="context()[(. = 'aw') or (. = 'AW')]">Aruba</xsl:when>
            <xsl:when test="context()[(. = 'au') or (. = 'AU')]">Australia</xsl:when>
            <xsl:when test="context()[(. = 'at') or (. = 'AT')]">Austria</xsl:when>
            <xsl:when test="context()[(. = 'az') or (. = 'AZ')]">Azerbaijan</xsl:when>
            
            <xsl:when test="context()[(. = 'bs') or (. = 'BS')]">Bahamas</xsl:when>
            <xsl:when test="context()[(. = 'bh') or (. = 'BH')]">Bahrain</xsl:when>
            <xsl:when test="context()[(. = 'bd') or (. = 'BD')]">Bangladesh</xsl:when>
            <xsl:when test="context()[(. = 'bb') or (. = 'BB')]">Barbados</xsl:when>
            <xsl:when test="context()[(. = 'by') or (. = 'BY')]">Belarus</xsl:when>
            <xsl:when test="context()[(. = 'be') or (. = 'BE')]">Belgium</xsl:when>
            <xsl:when test="context()[(. = 'bz') or (. = 'BZ')]">Belize</xsl:when>
            <xsl:when test="context()[(. = 'bj') or (. = 'BJ')]">Benin</xsl:when>
            <xsl:when test="context()[(. = 'bm') or (. = 'BM')]">Bermuda</xsl:when>
            <xsl:when test="context()[(. = 'bt') or (. = 'BT')]">Bhutan</xsl:when>
            <xsl:when test="context()[(. = 'bo') or (. = 'BO')]">Bolivia</xsl:when>
            <xsl:when test="context()[(. = 'ba') or (. = 'BA')]">Bosnia and Herzegovina</xsl:when>
            <xsl:when test="context()[(. = 'bw') or (. = 'BW')]">Botswana</xsl:when>
            <xsl:when test="context()[(. = 'bv') or (. = 'BV')]">Bouvet Island</xsl:when>
            <xsl:when test="context()[(. = 'br') or (. = 'BR')]">Brazil</xsl:when>
            <xsl:when test="context()[(. = 'io') or (. = 'IO')]">British Indian Ocean Territory</xsl:when>
            <xsl:when test="context()[(. = 'bn') or (. = 'BN')]">Brunei Darussalam</xsl:when>
            <xsl:when test="context()[(. = 'bg') or (. = 'BG')]">Bulgaria</xsl:when>
            <xsl:when test="context()[(. = 'bf') or (. = 'BF')]">Burkina Faso</xsl:when>
            <xsl:when test="context()[(. = 'bi') or (. = 'BI')]">Burundi</xsl:when>
            
            <xsl:when test="context()[(. = 'kh') or (. = 'KH')]">Cambodia</xsl:when>
            <xsl:when test="context()[(. = 'cm') or (. = 'CM')]">Cameroon</xsl:when>
            <xsl:when test="context()[(. = 'ca') or (. = 'CA')]">Canada</xsl:when>
            <xsl:when test="context()[(. = 'cv') or (. = 'CV')]">Cape Verde</xsl:when>
            <xsl:when test="context()[(. = 'ky') or (. = 'KY')]">Cayman Islands</xsl:when>
            <xsl:when test="context()[(. = 'cf') or (. = 'CF')]">Central African Republic</xsl:when>
            <xsl:when test="context()[(. = 'td') or (. = 'TD')]">Chad</xsl:when>
            <xsl:when test="context()[(. = 'cl') or (. = 'CL')]">Chile</xsl:when>
            <xsl:when test="context()[(. = 'cn') or (. = 'CN')]">China</xsl:when>
            <xsl:when test="context()[(. = 'cx') or (. = 'CX')]">Christmas Island</xsl:when>
            <xsl:when test="context()[(. = 'cc') or (. = 'CC')]">Cocos (Keeling) Islands</xsl:when>
            <xsl:when test="context()[(. = 'co') or (. = 'CO')]">Colombia</xsl:when>
            <xsl:when test="context()[(. = 'km') or (. = 'KM')]">Comoros</xsl:when>
            <xsl:when test="context()[(. = 'cg') or (. = 'CG')]">Congo</xsl:when>
            <xsl:when test="context()[(. = 'cd') or (. = 'CD')]">Congo, Democratic Republic of the</xsl:when>
            <xsl:when test="context()[(. = 'ck') or (. = 'CK')]">Cook Islands</xsl:when>
            <xsl:when test="context()[(. = 'cr') or (. = 'CR')]">Costa Rica</xsl:when>
            <xsl:when test="context()[(. = 'ci') or (. = 'CI')]">Cote D'Ivoire</xsl:when>
            <xsl:when test="context()[(. = 'hr') or (. = 'HR')]">Croatia</xsl:when>
            <xsl:when test="context()[(. = 'cu') or (. = 'CU')]">Cuba</xsl:when>
            <xsl:when test="context()[(. = 'cy') or (. = 'CY')]">Cyprus</xsl:when>
            <xsl:when test="context()[(. = 'cz') or (. = 'CZ')]">Czech Republic</xsl:when>
            
            <xsl:when test="context()[(. = 'dk') or (. = 'DK')]">Denmark</xsl:when>
            <xsl:when test="context()[(. = 'dj') or (. = 'DJ')]">Djibouti</xsl:when>
            <xsl:when test="context()[(. = 'dm') or (. = 'DM')]">Dominica</xsl:when>
            <xsl:when test="context()[(. = 'do') or (. = 'DO')]">Dominican Republic</xsl:when>
            
            <xsl:when test="context()[(. = 'tp') or (. = 'TP')]">East Timor</xsl:when>
            <xsl:when test="context()[(. = 'ec') or (. = 'EC')]">Ecuador</xsl:when>
            <xsl:when test="context()[(. = 'eg') or (. = 'EG')]">Egypt</xsl:when>
            <xsl:when test="context()[(. = 'sv') or (. = 'SV')]">El Salvador</xsl:when>
            <xsl:when test="context()[(. = 'gq') or (. = 'GQ')]">Equatorial Guinea</xsl:when>
            <xsl:when test="context()[(. = 'er') or (. = 'ER')]">Eritrea</xsl:when>
            <xsl:when test="context()[(. = 'ee') or (. = 'EE')]">Estonia</xsl:when>
            <xsl:when test="context()[(. = 'et') or (. = 'ET')]">Ethiopia</xsl:when>
            
            <xsl:when test="context()[(. = 'fk') or (. = 'FK')]">Falkland Islands (Malvinias)</xsl:when>
            <xsl:when test="context()[(. = 'fo') or (. = 'FO')]">Faroe Islands</xsl:when>
            <xsl:when test="context()[(. = 'fj') or (. = 'FJ')]">Fiji</xsl:when>
            <xsl:when test="context()[(. = 'fi') or (. = 'FI')]">Finland</xsl:when>
            <xsl:when test="context()[(. = 'fr') or (. = 'FR')]">France</xsl:when>
            <xsl:when test="context()[(. = 'gf') or (. = 'GF')]">French Guiana</xsl:when>
            <xsl:when test="context()[(. = 'pf') or (. = 'PF')]">French Polynesia</xsl:when>
            <xsl:when test="context()[(. = 'tf') or (. = 'TF')]">French Southern Territories</xsl:when>
            
            <xsl:when test="context()[(. = 'ga') or (. = 'GA')]">Gabon</xsl:when>
            <xsl:when test="context()[(. = 'gm') or (. = 'GM')]">Gambia</xsl:when>
            <xsl:when test="context()[(. = 'ge') or (. = 'GE')]">Georgia</xsl:when>
            <xsl:when test="context()[(. = 'de') or (. = 'DE')]">Germany</xsl:when>
            <xsl:when test="context()[(. = 'gh') or (. = 'GH')]">Ghana</xsl:when>
            <xsl:when test="context()[(. = 'gi') or (. = 'GI')]">Gibraltar</xsl:when>
            <xsl:when test="context()[(. = 'gr') or (. = 'GR')]">Greece</xsl:when>
            <xsl:when test="context()[(. = 'gl') or (. = 'GL')]">Greenland</xsl:when>
            <xsl:when test="context()[(. = 'gd') or (. = 'GD')]">Grenada</xsl:when>
            <xsl:when test="context()[(. = 'gp') or (. = 'GP')]">Guadeloupe</xsl:when>
            <xsl:when test="context()[(. = 'gu') or (. = 'GU')]">Guam</xsl:when>
            <xsl:when test="context()[(. = 'gt') or (. = 'GT')]">Guatemala</xsl:when>
            <xsl:when test="context()[(. = 'gn') or (. = 'GN')]">Guinea</xsl:when>
            <xsl:when test="context()[(. = 'gw') or (. = 'GW')]">Guinea-Bissau</xsl:when>
            <xsl:when test="context()[(. = 'gy') or (. = 'GY')]">Guyana</xsl:when>
            
            <xsl:when test="context()[(. = 'ht') or (. = 'HT')]">Haiti</xsl:when>
            <xsl:when test="context()[(. = 'hm') or (. = 'HM')]">Heard Island and McDonald Islands</xsl:when>
            <xsl:when test="context()[(. = 'va') or (. = 'VA')]">Holy See / Vatican City State</xsl:when>
            <xsl:when test="context()[(. = 'hn') or (. = 'HN')]">Honduras</xsl:when>
            <xsl:when test="context()[(. = 'hk') or (. = 'HK')]">Hong Kong</xsl:when>
            <xsl:when test="context()[(. = 'hu') or (. = 'HU')]">Hungary</xsl:when>
            
            <xsl:when test="context()[(. = 'is') or (. = 'IS')]">Iceland</xsl:when>
            <xsl:when test="context()[(. = 'in') or (. = 'IN')]">India</xsl:when>
            <xsl:when test="context()[(. = 'id') or (. = 'ID')]">Indonesia</xsl:when>
            <xsl:when test="context()[(. = 'ir') or (. = 'IR')]">Iran, Islamic Republic of</xsl:when>
            <xsl:when test="context()[(. = 'iq') or (. = 'IQ')]">Iraq</xsl:when>
            <xsl:when test="context()[(. = 'ie') or (. = 'IE')]">Ireland</xsl:when>
            <xsl:when test="context()[(. = 'il') or (. = 'IL')]">Israel</xsl:when>
            <xsl:when test="context()[(. = 'it') or (. = 'IT')]">Italy</xsl:when>
            
            <xsl:when test="context()[(. = 'jm') or (. = 'JM')]">Jamaica</xsl:when>
            <xsl:when test="context()[(. = 'jp') or (. = 'JP')]">Japan</xsl:when>
            <xsl:when test="context()[(. = 'jo') or (. = 'JO')]">Jordan</xsl:when>
            
            <xsl:when test="context()[(. = 'kz') or (. = 'KZ')]">Kazakstan</xsl:when>
            <xsl:when test="context()[(. = 'ke') or (. = 'KE')]">Kenya</xsl:when>
            <xsl:when test="context()[(. = 'ki') or (. = 'KI')]">Kiribati</xsl:when>
            <xsl:when test="context()[(. = 'kp') or (. = 'KP')]">Korea, Democratic People's Republic of</xsl:when>
            <xsl:when test="context()[(. = 'kr') or (. = 'KR')]">Korea, Republic of</xsl:when>
            <xsl:when test="context()[(. = 'kw') or (. = 'KW')]">Kuwait</xsl:when>
            <xsl:when test="context()[(. = 'kg') or (. = 'KG')]">Kyrgyzstan</xsl:when>
            
            <xsl:when test="context()[(. = 'la') or (. = 'LA')]">Lao People's Demoratic Republic</xsl:when>
            <xsl:when test="context()[(. = 'lv') or (. = 'LV')]">Latvia</xsl:when>
            <xsl:when test="context()[(. = 'lb') or (. = 'LB')]">Lebanon</xsl:when>
            <xsl:when test="context()[(. = 'ls') or (. = 'LS')]">Lesotho</xsl:when>
            <xsl:when test="context()[(. = 'lr') or (. = 'LR')]">Liberia</xsl:when>
            <xsl:when test="context()[(. = 'ly') or (. = 'LY')]">Libyan Arab Jamahiriya</xsl:when>
            <xsl:when test="context()[(. = 'li') or (. = 'LI')]">Liechtenstein</xsl:when>
            <xsl:when test="context()[(. = 'lt') or (. = 'LT')]">Lithuania</xsl:when>
            <xsl:when test="context()[(. = 'lu') or (. = 'LU')]">Luxembourg</xsl:when>
            
            <xsl:when test="context()[(. = 'mo') or (. = 'MO')]">Macau</xsl:when>
            <xsl:when test="context()[(. = 'mk') or (. = 'MK')]">Macedonia, The Former Yugoslav Republic of</xsl:when>
            <xsl:when test="context()[(. = 'mg') or (. = 'MG')]">Madagascar</xsl:when>
            <xsl:when test="context()[(. = 'mw') or (. = 'MW')]">Malawi</xsl:when>
            <xsl:when test="context()[(. = 'my') or (. = 'MY')]">Malaysia</xsl:when>
            <xsl:when test="context()[(. = 'mv') or (. = 'MV')]">Maldives</xsl:when>
            <xsl:when test="context()[(. = 'ml') or (. = 'ML')]">Mali</xsl:when>
            <xsl:when test="context()[(. = 'mt') or (. = 'MT')]">Malta</xsl:when>
            <xsl:when test="context()[(. = 'mh') or (. = 'MH')]">Marshall Islands</xsl:when>
            <xsl:when test="context()[(. = 'mq') or (. = 'MQ')]">Martinique</xsl:when>
            <xsl:when test="context()[(. = 'mr') or (. = 'MR')]">Mauritania</xsl:when>
            <xsl:when test="context()[(. = 'mu') or (. = 'MU')]">Mauritius</xsl:when>
            <xsl:when test="context()[(. = 'yt') or (. = 'YT')]">Mayotte</xsl:when>
            <xsl:when test="context()[(. = 'mx') or (. = 'MX')]">Mexico</xsl:when>
            <xsl:when test="context()[(. = 'fm') or (. = 'FM')]">Micronesia, Federated States of</xsl:when>
            <xsl:when test="context()[(. = 'md') or (. = 'MD')]">Moldova, Republic of</xsl:when>
            <xsl:when test="context()[(. = 'mc') or (. = 'MC')]">Monaco</xsl:when>
            <xsl:when test="context()[(. = 'mn') or (. = 'MN')]">Mongolia</xsl:when>
            <xsl:when test="context()[(. = 'ms') or (. = 'MS')]">Montserrat</xsl:when>
            <xsl:when test="context()[(. = 'ma') or (. = 'MA')]">Morocco</xsl:when>
            <xsl:when test="context()[(. = 'mz') or (. = 'MZ')]">Mozambique</xsl:when>
            <xsl:when test="context()[(. = 'mm') or (. = 'MM')]">Myanmar</xsl:when>
            
            <xsl:when test="context()[(. = 'na') or (. = 'NA')]">Namibia</xsl:when>
            <xsl:when test="context()[(. = 'nr') or (. = 'NR')]">Nauru</xsl:when>
            <xsl:when test="context()[(. = 'np') or (. = 'NP')]">Nepal</xsl:when>
            <xsl:when test="context()[(. = 'nl') or (. = 'NL')]">Netherlands</xsl:when>
            <xsl:when test="context()[(. = 'an') or (. = 'AN')]">Netherlands Antilles</xsl:when>
            <xsl:when test="context()[(. = 'nc') or (. = 'NC')]">New Caledonia</xsl:when>
            <xsl:when test="context()[(. = 'nz') or (. = 'NZ')]">New Zealand</xsl:when>
            <xsl:when test="context()[(. = 'ni') or (. = 'NI')]">Nicaragua</xsl:when>
            <xsl:when test="context()[(. = 'ne') or (. = 'NE')]">Niger</xsl:when>
            <xsl:when test="context()[(. = 'ng') or (. = 'NG')]">Nigeria</xsl:when>
            <xsl:when test="context()[(. = 'nu') or (. = 'NU')]">Niue</xsl:when>
            <xsl:when test="context()[(. = 'nf') or (. = 'NF')]">Norfolk Island</xsl:when>
            <xsl:when test="context()[(. = 'mp') or (. = 'MP')]">Northern Mariana Islands</xsl:when>
            <xsl:when test="context()[(. = 'no') or (. = 'NO')]">Norway</xsl:when>
            
            <xsl:when test="context()[(. = 'om') or (. = 'OM')]">Oman</xsl:when>
            
            <xsl:when test="context()[(. = 'pk') or (. = 'PK')]">Pakistan</xsl:when>
            <xsl:when test="context()[(. = 'pw') or (. = 'PW')]">Palau</xsl:when>
            <xsl:when test="context()[(. = 'ps') or (. = 'PS')]">Palestinian Territory, Occupied</xsl:when>
            <xsl:when test="context()[(. = 'pa') or (. = 'PA')]">Panama</xsl:when>
            <xsl:when test="context()[(. = 'pg') or (. = 'PG')]">Papua New Guinea</xsl:when>
            <xsl:when test="context()[(. = 'py') or (. = 'PY')]">Paraguay</xsl:when>
            <xsl:when test="context()[(. = 'pe') or (. = 'PE')]">Peru</xsl:when>
            <xsl:when test="context()[(. = 'ph') or (. = 'PH')]">Phillippines</xsl:when>
            <xsl:when test="context()[(. = 'pn') or (. = 'PN')]">Pitcairn</xsl:when>
            <xsl:when test="context()[(. = 'pl') or (. = 'PL')]">Poland</xsl:when>
            <xsl:when test="context()[(. = 'pt') or (. = 'PT')]">Portugal</xsl:when>
            <xsl:when test="context()[(. = 'pr') or (. = 'PR')]">Puerto Rico</xsl:when>
            
            <xsl:when test="context()[(. = 'qa') or (. = 'QA')]">Qatar</xsl:when>
            
            <xsl:when test="context()[(. = 're') or (. = 'RE')]">Reunion</xsl:when>
            <xsl:when test="context()[(. = 'ro') or (. = 'RO')]">Romania</xsl:when>
            <xsl:when test="context()[(. = 'ru') or (. = 'RU')]">Russian Federation</xsl:when>
            <xsl:when test="context()[(. = 'rw') or (. = 'RW')]">Rwanda</xsl:when>
            
            <xsl:when test="context()[(. = 'sh') or (. = 'SH')]">Saint Helena</xsl:when>
            <xsl:when test="context()[(. = 'kn') or (. = 'KN')]">Saint Kitts and Nevis</xsl:when>
            <xsl:when test="context()[(. = 'lc') or (. = 'LC')]">Saint Lucia</xsl:when>
            <xsl:when test="context()[(. = 'pm') or (. = 'PM')]">Saint Pierre and Miquelon</xsl:when>
            <xsl:when test="context()[(. = 'vc') or (. = 'VC')]">Saint Vincent and the Grenadines</xsl:when>
            <xsl:when test="context()[(. = 'ws') or (. = 'WS')]">Samoa</xsl:when>
            <xsl:when test="context()[(. = 'sm') or (. = 'SM')]">San Marino</xsl:when>
            <xsl:when test="context()[(. = 'st') or (. = 'ST')]">Sao Tome and Principe</xsl:when>
            <xsl:when test="context()[(. = 'sa') or (. = 'SA')]">Saudi Arabia</xsl:when>
            <xsl:when test="context()[(. = 'sn') or (. = 'SN')]">Senegal</xsl:when>
            <xsl:when test="context()[(. = 'sc') or (. = 'SC')]">Seychelles</xsl:when>
            <xsl:when test="context()[(. = 'sl') or (. = 'SL')]">Sierra Leone</xsl:when>
            <xsl:when test="context()[(. = 'sg') or (. = 'SG')]">Singapore</xsl:when>
            <xsl:when test="context()[(. = 'sk') or (. = 'SK')]">Slovakia</xsl:when>
            <xsl:when test="context()[(. = 'si') or (. = 'SI')]">Slovenia</xsl:when>
            <xsl:when test="context()[(. = 'sb') or (. = 'SB')]">Solomon Islands</xsl:when>
            <xsl:when test="context()[(. = 'so') or (. = 'S0')]">Somalia</xsl:when>
            <xsl:when test="context()[(. = 'za') or (. = 'ZA')]">South Africa</xsl:when>
            <xsl:when test="context()[(. = 'gs') or (. = 'GS')]">South Georgia and the South Sandwich Islands</xsl:when>
            <xsl:when test="context()[(. = 'es') or (. = 'ES')]">Spain</xsl:when>
            <xsl:when test="context()[(. = 'lk') or (. = 'LK')]">Sri Lanka</xsl:when>
            <xsl:when test="context()[(. = 'sd') or (. = 'SD')]">Sudan</xsl:when>
            <xsl:when test="context()[(. = 'sr') or (. = 'SR')]">Suriname</xsl:when>
            <xsl:when test="context()[(. = 'sj') or (. = 'SJ')]">Svalbard and Jan Mayen</xsl:when>
            <xsl:when test="context()[(. = 'sz') or (. = 'SZ')]">Swaziland</xsl:when>
            <xsl:when test="context()[(. = 'se') or (. = 'SE')]">Sweden</xsl:when>
            <xsl:when test="context()[(. = 'ch') or (. = 'CH')]">Switzerland</xsl:when>
            <xsl:when test="context()[(. = 'sy') or (. = 'SY')]">Syrian Arab Republic</xsl:when>
            
            <xsl:when test="context()[(. = 'tw') or (. = 'TW')]">Taiwan, Province of China</xsl:when>
            <xsl:when test="context()[(. = 'tj') or (. = 'TJ')]">Tajikistan</xsl:when>
            <xsl:when test="context()[(. = 'tz') or (. = 'TZ')]">Tanzania, United Republic of</xsl:when>
            <xsl:when test="context()[(. = 'th') or (. = 'TH')]">Thailand</xsl:when>
            <xsl:when test="context()[(. = 'tg') or (. = 'TG')]">Togo</xsl:when>
            <xsl:when test="context()[(. = 'tk') or (. = 'TK')]">Tokelau</xsl:when>
            <xsl:when test="context()[(. = 'to') or (. = 'TO')]">Tonga</xsl:when>
            <xsl:when test="context()[(. = 'tt') or (. = 'TT')]">Trinidad and Tobago</xsl:when>
            <xsl:when test="context()[(. = 'tn') or (. = 'TN')]">Tunisia</xsl:when>
            <xsl:when test="context()[(. = 'tr') or (. = 'TR')]">Turkey</xsl:when>
            <xsl:when test="context()[(. = 'tm') or (. = 'TM')]">Turkmenistan</xsl:when>
            <xsl:when test="context()[(. = 'tc') or (. = 'TC')]">Turks and Caicos Islands</xsl:when>
            <xsl:when test="context()[(. = 'tv') or (. = 'TV')]">Tuvalu</xsl:when>
            
            <xsl:when test="context()[(. = 'ug') or (. = 'UG')]">Uganda</xsl:when>
            <xsl:when test="context()[(. = 'ua') or (. = 'UA')]">Ukraine</xsl:when>
            <xsl:when test="context()[(. = 'ae') or (. = 'AE')]">United Arab Emirates</xsl:when>
            <xsl:when test="context()[(. = 'gb') or (. = 'GB')]">United Kingdom</xsl:when>
            <xsl:when test="context()[(. = 'us') or (. = 'US')]">United States</xsl:when>
            <xsl:when test="context()[(. = 'um') or (. = 'UM')]">United States Minor Outlying Islands</xsl:when>
            <xsl:when test="context()[(. = 'uy') or (. = 'UY')]">Uruguay</xsl:when>
            <xsl:when test="context()[(. = 'uz') or (. = 'UZ')]">Uzbekistan</xsl:when>
            
            <xsl:when test="context()[(. = 'vu') or (. = 'VU')]">Vanuatu</xsl:when>
            <xsl:when test="context()[(. = 've') or (. = 'VE')]">Venezuela</xsl:when>
            <xsl:when test="context()[(. = 'vn') or (. = 'VN')]">Viet Nam</xsl:when>
            <xsl:when test="context()[(. = 'vg') or (. = 'VG')]">Virgin Islands, British</xsl:when>
            <xsl:when test="context()[(. = 'vi') or (. = 'VI')]">Virgin Islands, U.S.</xsl:when>
            
            <xsl:when test="context()[(. = 'wf') or (. = 'WF')]">Wallis and Futuna</xsl:when>
            <xsl:when test="context()[(. = 'eh') or (. = 'EH')]">Western Sahara</xsl:when>
            
            <xsl:when test="context()[(. = 'ye') or (. = 'YE')]">Yemen</xsl:when>
            <xsl:when test="context()[(. = 'yu') or (. = 'YU')]">Yugoslavia</xsl:when>
            
            <xsl:when test="context()[(. = 'zm') or (. = 'ZM')]">Zambia</xsl:when>
            <xsl:when test="context()[(. = 'zw') or (. = 'ZW')]">Zimbabwe</xsl:when>
            
            <xsl:otherwise><xsl:value-of/></xsl:otherwise>
	   </xsl:choose>
        </DT>
      </xsl:for-each>
      <xsl:for-each select="eMailAdd">
        <DT><FONT color="#0000AA"><B>e-mail address:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Online Resource Information (B.3.2.4 CI_OnLineResource - line396) -->
<xsl:template match="(cntOnlineRes | onLineSrc | extOnRes)">
  <DD>
  <xsl:choose>
    <xsl:when test="../onLineSrc">
      <DT><FONT color="#0000AA"><B>Online source:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../extOnRes">
      <DT><FONT color="#0000AA"><B>Extension online resource:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
      <DT><FONT color="#0000AA"><B>Online resource:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>

  <DD>
  <DL>
    <xsl:for-each select="orName">
      <DT><FONT color="#0000AA"><B>Name of resource:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="linkage">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Online location (URL):</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="protocol">
      <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Connection protocol:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
    <xsl:for-each select="orFunct">
      <DT><FONT color="#0000AA"><B>Function performed:</B></FONT>
        <xsl:for-each select="OnFunctCd">
          <xsl:choose>
            <xsl:when test="context()[@value = '001']">download</xsl:when>
            <xsl:when test="context()[@value = '002']">information</xsl:when>
            <xsl:when test="context()[@value = '003']">offline access</xsl:when>
            <xsl:when test="context()[@value = '004']">order</xsl:when>
            <xsl:when test="context()[@value = '005']">search</xsl:when>
            <xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
	   </xsl:choose>
        </xsl:for-each>
        </DT>
    </xsl:for-each>
    <xsl:for-each select="orDesc">
      <DT><FONT color="#0000AA"><B>Description:</B></FONT>
        <xsl:choose>
            <xsl:when test="context()[. = '001']">Live Data and Maps</xsl:when>
            <xsl:when test="context()[. = '002']">Downloadable Data</xsl:when>
            <xsl:when test="context()[. = '003']">Offline Data</xsl:when>
            <xsl:when test="context()[. = '004']">Static Map Images</xsl:when>
            <xsl:when test="context()[. = '005']">Other Documents</xsl:when>
            <xsl:when test="context()[. = '006']">Applications</xsl:when>
            <xsl:when test="context()[. = '007']">Geographic Services</xsl:when>
            <xsl:when test="context()[. = '008']">Clearinghouses</xsl:when>
            <xsl:when test="context()[. = '009']">Map Files</xsl:when>
            <xsl:when test="context()[. = '010']">Geographic Activities</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>
    </xsl:for-each>
    <xsl:for-each select="appProfile">
      <DT><FONT color="#0000AA"><B>Application profile:</B></FONT> <xsl:value-of/></DT>
    </xsl:for-each>
  </DL>
  </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Series Information (B.3.2.5 CI_Series - line403) -->
<xsl:template match="datasetSeries">
  <DD>
    <DT><FONT color="#0000AA"><B>Series:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="seriesName">
        <DT><FONT color="#0000AA"><B>Name:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="issId">
        <DT><FONT color="#0000AA"><B>Issue:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="artPage">
        <DT><FONT color="#0000AA"><B>Pages:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

-------- 

<!-- EXTENT INFORMATION -->

<!-- Extent Information (B.3.1 EX_Extent - line334) -->
<xsl:template match="(dataExt | scpExt | srcExt)">
  <DD>
  <xsl:choose>
    <xsl:when test="../dataExt">
      <DT><FONT color="#0000AA"><B>Other extent information:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../scpExt">
      <DT><FONT color="#0000AA"><B>Scope extent:</B></FONT></DT>
    </xsl:when>
    <xsl:when test="../srcExt">
      <DT><FONT color="#0000AA"><B>Extent of the source data:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
      <DT><FONT color="#0000AA"><B>Extent:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>

    <DD>
    <DL>
      <xsl:for-each select="exDesc">
        <DT><FONT color="#0000AA"><B>Extent description:</B></FONT></DT>
        <PRE ID="original"><xsl:value-of /></PRE>
        <SCRIPT>fix(original)</SCRIPT>
        <BR/>
      </xsl:for-each>

      <xsl:for-each select="geoEle">
        <DT><FONT color="#0000AA"><B>Geographic extent:</B></FONT></DT>
        <DD>
          <DD>
          <DL>
            <xsl:apply-templates select="BoundPoly"/>
            <xsl:apply-templates select="GeoBndBox"/>
            <xsl:apply-templates select="GeoDesc"/>
          </DL>
          </DD>
        </DD>
        <xsl:if test="context()[not (*)]"><BR/></xsl:if>
      </xsl:for-each>

      <xsl:for-each select="tempEle">
        <xsl:apply-templates select="TempExtent"/>
        <xsl:apply-templates select="SpatTempEx"/>
      </xsl:for-each>

      <xsl:apply-templates select="vertEle"/>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Bounding Polygon Information (B.3.1.1 EX_BoundingPolygon - line341) -->
<xsl:template match="BoundPoly">
  <DD>
  <DT><FONT color="#0000AA"><B>Bounding polygon:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="exTypeCode">
        <DT><FONT color="#0000AA"><B>Extent contains the resource:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="polygon/GM_Polygon/coordinates">
        <DT><FONT color="#0000AA"><B>Coordinates:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="context()[($any$ exTypeCode | polygon/GM_Polygon/coordinates)]"><BR/><BR/></xsl:if>
      
       <xsl:if test="polygon/GM_Polygon/MdCoRefSys">
        <DT><FONT color="#0000AA"><B>Polygon coordinate system:</B></FONT></DT>
        <DD>
        <DL>
          <xsl:apply-templates select="polygon/GM_Polygon/MdCoRefSys"/>
        </DL>
        </DD>
      </xsl:if>     
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Bounding Box Information (B.3.1.1 EX_GeographicBoundingBox - line343) -->
<xsl:template match="(geoBox | GeoBndBox)">
  <DD>
  <xsl:choose>
    <xsl:when test="../geoBox">
      <DT><FONT color="#0000AA"><B>Resource's bounding rectangle:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
      <DT><FONT color="#0000AA"><B>Bounding rectangle:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>

    <DD>
    <DL>
      <xsl:for-each select="@esriExtentType">
        <DT><FONT color="#006400">*</FONT><FONT color="#006400"><B>Extent type:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = 'native']">Full extent in the data's coordinate system</xsl:when>
            <xsl:when test="context()[. = 'decdegrees']">Full extent in decimal degrees</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="exTypeCode">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Extent contains the resource:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:for-each select="westBL">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>West longitude:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="eastBL">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>East longitude:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="northBL">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>North latitude:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="southBL">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>South latitude:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <BR/>
</xsl:template>

<!-- Geographic Description Information (B.3.1.1 EX_GeographicDescription - line348) -->
<xsl:template match="(geoDesc | GeoDesc)">
  <DD>
  <xsl:choose>
    <xsl:when test="../geoDesc">
      <DT><FONT color="#0000AA"><B>Description of the resource's location:</B></FONT></DT>
    </xsl:when>
    <xsl:otherwise>
      <DT><FONT color="#0000AA"><B>Geographic description:</B></FONT></DT>
    </xsl:otherwise>
  </xsl:choose>

    <DD>
    <DL>
      <xsl:for-each select="exTypeCode">
        <DT><FONT color="#0000AA"><B>Extent contains the resource:</B></FONT>
          <xsl:choose>
            <xsl:when test="context()[. = '1']">Yes</xsl:when>
            <xsl:when test="context()[. = '0']">No</xsl:when>
            <xsl:otherwise><xsl:value-of /></xsl:otherwise>
	   </xsl:choose>
        </DT>      
      </xsl:for-each>
      <xsl:apply-templates select="geoId"/>
      <xsl:if test="context()[(exTypeCode) and not (geoId)]"><BR/><BR/></xsl:if>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Temporal Extent Information (B.3.1.2 EX_TemporalExtent - line350) -->
<xsl:template match="TempExtent">
  <DD>
  <DT><FONT color="#0000AA"><B>Temporal extent:</B></FONT></DT>
  <xsl:apply-templates select="exTemp/TM_GeometricPrimitive"/>
  </DD>
</xsl:template>

<!-- temporal extent Information from ISO 19103 as defined is DTD -->
<xsl:template match="TM_GeometricPrimitive">
  <DD>
  <DL>
  <xsl:for-each select="TM_Period">
      <xsl:for-each select="begin">
        <DT><FONT color="#0000AA"><B>Beginning date:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="end">
        <DT><FONT color="#0000AA"><B>Ending date:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
  </xsl:for-each>
  
  <xsl:for-each select="TM_Instant">
      <xsl:for-each select=".//calDate">
        <DT><FONT color="#0000AA"><B>Calendar date:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select=".//clkTime">
        <DT><FONT color="#0000AA"><B>Clock time:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
  </xsl:for-each>
  
  </DL>
  </DD>
  <BR/>
</xsl:template>

<!-- Spatial Temporal Extent Information (B.3.1.2 EX_SpatialTemporalExtent - line352) -->
<xsl:template match="SpatTempEx">
  <DD>
  <DT><FONT color="#0000AA"><B>Spatial and temporal extent:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="exTemp">
        <DT><FONT color="#0000AA"><B>Temporal extent:</B></FONT></DT>
        <xsl:apply-templates select="TM_GeometricPrimitive"/>
      </xsl:for-each>

      <xsl:for-each select="exSpat">
        <DT><FONT color="#0000AA"><B>Spatial extent:</B></FONT></DT>
        <DD>
          <DD>
          <DL>
            <xsl:apply-templates select="BoundPoly"/>
            <xsl:apply-templates select="GeoBndBox"/>
            <xsl:apply-templates select="GeoDesc"/>
          </DL>
          </DD>
        </DD>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
</xsl:template>

<!-- Vertical Extent Information (B.3.1.3 EX_VerticalExtent - line354) -->
<xsl:template match="vertEle">
  <DD>
  <DT><FONT color="#0000AA"><B>Vertical extent:</B></FONT></DT>
    <DD>
    <DL>
      <xsl:for-each select="vertMinVal">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Minimum value:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:for-each select="vertMaxVal">
        <DT><xsl:if test="context()[@Sync = 'TRUE']">
                <FONT color="#006400">*</FONT></xsl:if><FONT color="#0000AA"><B>Maximum value:</B></FONT> <xsl:value-of/></DT>
      </xsl:for-each>
      <xsl:if test="vertUoM">
        <DT><FONT color="#0000AA"><B>Units of measure, length:</B></FONT></DT>
        <xsl:apply-templates select="vertUoM"/>
      </xsl:if>
      <xsl:if test="context()[($any$ vertMinVal | vertMaxVal) and not (vertUoM)]"><BR/><BR/></xsl:if>

      <xsl:for-each select="vertDatum">
        <xsl:apply-templates select="datumID"/>
      </xsl:for-each>
    </DL>
    </DD>
  </DD>
  <xsl:if test="context()[not (*)]"><BR/></xsl:if>
</xsl:template>

-------- 

<!-- ESRI EXTENDED ELEMENTS -->

<!-- 	GEOPROCESSING HISTORY -->

<xsl:template match="/metadata/Esri/DataProperties/lineage">
  <A name="Geoprocessing"><HR/></A>
  <DL>
    <DT><FONT color="#006400" size="3"><B>Geoprocessing History:</B></FONT></DT>
    <BR/><BR/>
    <DD>
    <DL>
      <xsl:for-each select="Process">
        <DT><FONT color="#006400"><B>Process:</B></FONT></DT>
        <DD>
        <DL>
          <xsl:if test="@Name">
            <DT><FONT color="#006400">*<B>Process name:</B></FONT> <xsl:value-of select="@Name"/></DT>
          </xsl:if>
          <xsl:if test="@Date">
            <DT><FONT color="#006400">*<B>Date:</B></FONT> <xsl:value-of select="@Date"/></DT>
          </xsl:if>
          <xsl:if test="@Time">
            <DT><FONT color="#006400">*<B>Time:</B></FONT> <xsl:value-of select="@Time"/></DT>
          </xsl:if>
          <xsl:if test="@ToolSource">
            <DT><FONT color="#006400">*<B>Tool location:</B></FONT> <xsl:value-of select="@ToolSource"/></DT>
          </xsl:if>
          <DT><FONT color="#006400">*<B>Command issued:</B></FONT> <xsl:value-of /></DT>
          <BR/><BR/>
        </DL>
        </DD>
      </xsl:for-each>
    </DL>
    </DD>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>

<!-- BINARY INFORMATION -->

<!-- Thumbnail -->
<xsl:template match="/metadata/Binary/Thumbnail/img[@src]">
      <IMG ID="thumbnail" ALIGN="absmiddle" STYLE="width:217; 
          border:'2 outset #FFFFFF'; position:relative">
        <xsl:attribute name="SRC"><xsl:value-of select="@src"/></xsl:attribute>
      </IMG>
</xsl:template>
<xsl:template match="/metadata/idinfo/browse/img[@src]">
  <xsl:if test="context()[not (/metadata/Binary/Thumbnail/img)]">
      <xsl:if test="../@BrowseGraphicType[. = 'Thumbnail']">
        <IMG ID="thumbnail" ALIGN="absmiddle" STYLE="height:144; 
            border:'2 outset #FFFFFF'; position:relative">
          <xsl:attribute name="SRC"><xsl:value-of select="@src"/></xsl:attribute>
        </IMG>
        <BR/><BR/>
      </xsl:if>
      <xsl:if test="context()[not (../../browse/@BrowseGraphicType)]">
        <IMG ID="thumbnail" ALIGN="absmiddle" STYLE="height:144; 
            border:'2 outset #FFFFFF'; position:relative">
          <xsl:attribute name="SRC"><xsl:value-of select="@src"/></xsl:attribute>
        </IMG>
        <BR/><BR/>
      </xsl:if>
  </xsl:if>
</xsl:template>

<!-- Enclosures -->
<xsl:template match="Binary">
  <A name="Binary_Enclosures"><HR/></A>
  <DL>
    <DT><FONT color="#006400" size="3"><B>Binary Enclosures:</B></FONT></DT>
    <BR/><BR/>
    <DD>
    <DL>
      <xsl:for-each select="Thumbnail">
        <DT><FONT color="#006400"><B>Thumbnail:</B></FONT></DT>
        <DD>
        <DL>
          <xsl:for-each select="img">
            <DT><FONT color="#006400"><B>Enclosure type:</B></FONT> Picture</DT>
            <BR/><BR/>
            <IMG ID="thumbnail" ALIGN="absmiddle" STYLE="height:144; 
                border:'2 outset #FFFFFF'; position:relative">
              <xsl:attribute name="SRC"><xsl:value-of select="@src"/></xsl:attribute>
            </IMG>
          </xsl:for-each>
        </DL>
        </DD>
        <BR/>
      </xsl:for-each>

      <xsl:for-each select="Enclosure">
        <DT><FONT color="#006400"><B>Enclosure:</B></FONT></DT>
        <DD>
        <DL>
          <xsl:for-each select="*/@EsriPropertyType">
            <DT><FONT color="#006400"><B>Enclosure type:</B></FONT> <xsl:value-of/></DT>
          </xsl:for-each>
          <xsl:for-each select="img">
            <DT><FONT color="#006400"><B>Enclosure type:</B></FONT> Image</DT>
           </xsl:for-each>
          <xsl:for-each select="*/@OriginalFileName">
            <DT><FONT color="#006400"><B>Original file name:</B></FONT> <xsl:value-of/></DT>
          </xsl:for-each>
          <xsl:for-each select="Descript">
            <DT><FONT color="#006400"><B>Description of enclosure:</B></FONT> <xsl:value-of/></DT>
          </xsl:for-each>
          <xsl:for-each select="img">
            <DD>
              <BR/>
              <IMG STYLE="height:144; border:'2 outset #FFFFFF'; position:relative">
                <xsl:attribute name="TITLE"><xsl:value-of select="img/@OriginalFileName"/></xsl:attribute>
                <xsl:attribute name="SRC"><xsl:value-of select="@src"/></xsl:attribute>
              </IMG>
            </DD>
           </xsl:for-each>
        </DL>
        </DD>
        <BR/>
      </xsl:for-each>
    </DL>
    </DD>
  </DL>
  <A HREF="#Top">Back to Top</A>
</xsl:template>


</xsl:stylesheet>