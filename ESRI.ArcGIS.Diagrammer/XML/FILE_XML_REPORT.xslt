<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
  <xsl:param name="font" select="font" />
  <xsl:param name="backcolor" select="backcolor" />
  <xsl:param name="forecolor" select="forecolor" />
  <xsl:param name="size1" select="size1" />
  <xsl:param name="size2" select="size2" />
  <xsl:param name="size3" select="size3" />
  <xsl:param name="size4" select="size4" />
  <xsl:param name="size5" select="size5" />
  <xsl:param name="title" select="title" />
  <xsl:param name="credit" select="credit" />
  <xsl:param name="disclaimer" select="disclaimer" />
  <xsl:param name="creationdate" select="creationdate" />
  <xsl:param name="username" select="username" />
  <xsl:param name="userdomainname" select="userdomainname" />
  <xsl:param name="machinename" select="machinename" />
  <xsl:param name="osversion" select="osversion" />
  <xsl:param name="dotnetversion" select="dotnetversion" />
  <xsl:param name="assemblyversion" select="assemblyversion" />
  <xsl:template match="/">
    <HTML>
      <HEAD>
        <TITLE>
          <xsl:value-of select="$title" />
        </TITLE>
        <META name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
      </HEAD>
      <BODY>
        <xsl:attribute name="bgcolor">
          <xsl:value-of select="$backcolor" />
        </xsl:attribute>
        <FONT>
          <xsl:attribute name="face">
            <xsl:value-of select="$font" />
          </xsl:attribute>
          <xsl:attribute name="color">
            <xsl:value-of select="$forecolor" />
          </xsl:attribute>
          <!--                                               -->
          <!--  ARCGIS DIAGRAMMER TITLE (AND CREDIT TO APL)  -->
          <!--                                               -->
          <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#ffc0cb" border="0">
            <TR>
              <TD>
                <P align="center">
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute>
                    <xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute>
                    <xsl:attribute name="size">
                      <xsl:value-of select="$size5" />
                    </xsl:attribute>
                    <U>
                      <xsl:value-of select="$title" />
                    </U>
                  </FONT>
                  <BR />
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute>
                    <xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute>
                    <xsl:attribute name="size">
                      <xsl:value-of select="$size3" />
                    </xsl:attribute>
                  </FONT>
                </P>
              </TD>
            </TR>
          </TABLE>
          <P />
          <!--            -->
          <!--  METADATA  -->
          <!--            -->
          <TABLE cellSpacing="0" cellPadding="0" width="100%" border="0" bgColor="#ccffccc">
            <TR>
              <TD width="100%" colspan="3">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                  <B>Report Creation</B>
                </FONT>
              </TD>
            </TR>
            <TR>
              <TD width="5%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                </FONT>
              </TD>
              <TD width="25%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                  Date
                </FONT>
              </TD>
              <TD width="70%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                  <xsl:value-of select="$creationdate" />
                </FONT>
              </TD>
            </TR>
            <TR>
              <TD width="5%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                </FONT>
              </TD>
              <TD width="25%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>Author
                </FONT>
              </TD>
              <TD width="70%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                  <xsl:value-of select="$username" />/<xsl:value-of select="$userdomainname" /> on <xsl:value-of select="$machinename" />
                </FONT>
              </TD>
            </TR>
            <TR>
              <TD width="100%" colspan="2">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                  <B>System Information</B>
                </FONT>
              </TD>
            </TR>
            <TR>
              <TD width="5%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                </FONT>
              </TD>
              <TD width="25%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>Operating System
                </FONT>
              </TD>
              <TD width="70%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                  <xsl:value-of select="$osversion" />
                </FONT>
              </TD>
            </TR>
            <TR>
              <TD width="5%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                </FONT>
              </TD>
              <TD width="25%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>.Net Framework
                </FONT>
              </TD>
              <TD width="70%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                  <xsl:value-of select="$dotnetversion" />
                </FONT>
              </TD>
            </TR>
            <TR>
              <TD width="5%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>
                </FONT>
              </TD>
              <TD width="25%">
                <FONT>
                  <xsl:attribute name="face">
                    <xsl:value-of select="$font" />
                  </xsl:attribute>
                  <xsl:attribute name="color">
                    <xsl:value-of select="$forecolor" />
                  </xsl:attribute>
                  <xsl:attribute name="size">
                    <xsl:value-of select="$size2" />
                  </xsl:attribute>Diagrammer
                </FONT>
              </TD>
              <TD width="70%">
                <FONT><xsl:attribute name="face"><xsl:value-of select="$font" /></xsl:attribute><xsl:attribute name="color"><xsl:value-of select="$forecolor" /></xsl:attribute><xsl:attribute name="size"><xsl:value-of select="$size2" /></xsl:attribute>
                  <xsl:value-of select="$assemblyversion" />
                </FONT>
              </TD>
            </TR>
          </TABLE>
          <P />
          <!--               -->
          <!--  XML REPORT  -->
          <!--               -->
          <!--  HEADER  -->
          <HR />
          <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#ffc0cb" border="0">
            <TR>
              <TD>
                <P align="center">
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute>
                    <xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute>
                    <xsl:attribute name="size">
                      <xsl:value-of select="$size4" />
                    </xsl:attribute>
                    <A name="DR">Xml Report</A>
                  </FONT>
                </P>
              </TD>
            </TR>
          </TABLE>
          <BR />
          <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#faebd7" border="1" bordercolor="#a9a9a9">
            <TR>
              <TD width="15%">
                <B>
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute>
                    <xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute>
                    <xsl:attribute name="size">
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>Severity
                  </FONT>
                </B>
              </TD>
              <TD width="55%">
                <B>
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute>
                    <xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute>
                    <xsl:attribute name="size">
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>Message
                  </FONT>
                </B>
              </TD>
              <TD width="15%">
                <B>
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute>
                    <xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute>
                    <xsl:attribute name="size">
                      <xsl:value-of select="$size2" />
                    </xsl:attribute>LineNumber
                  </FONT>
                </B>
              </TD>
              <TD width="15%">
                <B>
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute>
                    <xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute>
                    <xsl:attribute name="size">
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>LinePosition
                  </FONT>
                </B>
              </TD>
            </TR>
            <!--  ERRORS -->
            <xsl:for-each select="XmlReport/Error">
              <!-- <xsl:sort select="Name" /> -->
                <TR>
                  <TD width="15%">
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute>
                      <xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute>
                      <xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>
                      <xsl:value-of select="Severity" />
                    </FONT>
                  </TD>
                  <TD width="55%">
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute>
                      <xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute>
                      <xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>
                      <xsl:value-of select="Message" />
                    </FONT>
                  </TD>
                  <TD width="15%">
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute>
                      <xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute>
                      <xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>
                      <xsl:value-of select="LineNumber" />
                    </FONT>
                  </TD>
                  <TD width="15%">
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute>
                      <xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute>
                      <xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>
                      <xsl:value-of select="LinePosition" />
                    </FONT>
                  </TD>
                </TR>
            </xsl:for-each>
          </TABLE>
          <P>
            <FONT>
              <xsl:attribute name="face">
                <xsl:value-of select="$font" />
              </xsl:attribute>
              <xsl:attribute name="color">
                <xsl:value-of select="$forecolor" />
              </xsl:attribute>
              <xsl:attribute name="size">
                <xsl:value-of select="$size1" />
              </xsl:attribute>
              <A href="#Top">Back to Top</A>
            </FONT>
          </P>
          <!--          -->
          <!--  FOOTER  -->
          <!--          -->
          <HR />
          <BLOCKQUOTE>
            <FONT>
              <xsl:attribute name="face">
                <xsl:value-of select="$font" />
              </xsl:attribute>
              <xsl:attribute name="color">
                <xsl:value-of select="$forecolor" />
              </xsl:attribute>
              <xsl:attribute name="size">
                <xsl:value-of select="$size1" />
              </xsl:attribute>
              <xsl:value-of select="$disclaimer" />
            </FONT>
          </BLOCKQUOTE>
        </FONT>
        <P />
      </BODY>
    </HTML>
  </xsl:template>
</xsl:stylesheet>