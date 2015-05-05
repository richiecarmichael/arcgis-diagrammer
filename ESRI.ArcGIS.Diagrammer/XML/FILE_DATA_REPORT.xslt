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
  <xsl:param name="pathName" select="pathName" />
  <xsl:param name="category" select="category" />
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
                  </xsl:attribute>
                  Author
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
                  </xsl:attribute>
                  Operating System
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
                  </xsl:attribute>
                  .Net Framework
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
                  </xsl:attribute>
                  Diagrammer
                </FONT>
              </TD>
              <TD width="70%">
                <FONT><xsl:attribute name="face"><xsl:value-of select="$font" /></xsl:attribute><xsl:attribute name="color"><xsl:value-of select="$forecolor" /></xsl:attribute><xsl:attribute name="size"><xsl:value-of select="$size2" /></xsl:attribute>
                  <xsl:value-of select="$assemblyversion" />
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
                  <B>Geodatabase</B>
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
                  Workspace Type
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
                  <xsl:value-of select="$category" />
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
                  File
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
                  <xsl:value-of select="$pathName" />
                </FONT>
              </TD>
            </TR>
          </TABLE>
          <P />
          <!--               -->
          <!--  DATA REPORT  -->
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
                    <A name="DR">Data Report</A>
                  </FONT>
                </P>
              </TD>
            </TR>
          </TABLE>
          <BR />
          <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#faebd7" border="1" bordercolor="#a9a9a9">
            <TR>
              <TD width="25%">
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
                    </xsl:attribute>ObjectClass Name
                  </FONT>
                </B>
              </TD>
              <TD width="10%">
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
                    </xsl:attribute>Type
                  </FONT>
                </B>
              </TD>
              <TD width="10%">
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
                    </xsl:attribute>Geometry
                  </FONT>
                </B>
              </TD>
              <TD width="30%" colspan="2">
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
                    </xsl:attribute>Subtype
                  </FONT>
                </B>
              </TD>
              <TD width="5%">
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
                    </xsl:attribute>Total
                  </FONT>
                </B>
              </TD>
              <TD width="10%">
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
                    </xsl:attribute>Extent
                  </FONT>
                </B>
              </TD>
              <TD width="10%">
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
                    </xsl:attribute>Snapshot
                  </FONT>
                </B>
              </TD>
            </TR>
            <!--  FEATUREDATASETS -->
            <xsl:for-each select="DataReport/FeatureDataset">
              <xsl:sort select="Name" />
              <xsl:if test="count(Dataset)>0">
                <TR>
                  <TD width="100%" colspan="8">
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
                        </xsl:attribute>
                        <xsl:value-of select="Name" />
                      </FONT>
                    </B>
                  </TD>
                </TR>
                <xsl:apply-templates select="Dataset" mode="DataReport">
                  <xsl:sort select="Name" />
                </xsl:apply-templates>
              </xsl:if>
            </xsl:for-each>
            <!--  STANDALONE OBJECTCLASS(S) -->
            <xsl:if test="count(DataReport/Dataset)>0">
              <TR>
                <TD width="100%" colspan="8">
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
                      </xsl:attribute>
                      Stand Alone ObjectClass(s)
                    </FONT>
                  </B>
                </TD>
              </TR>
              <xsl:apply-templates select="DataReport/Dataset" mode="DataReport">
                <xsl:sort select="Name" />
              </xsl:apply-templates>
            </xsl:if>
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
  <!--           -->
  <!--  DATASET  -->
  <!--           -->
  <xsl:template match="Dataset" mode="DataReport">
    <TR>
      <!-- Dataset Name -->
      <TD width="25%">
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
          <xsl:value-of select="Name" />
        </FONT>
      </TD>
      <!-- Type -->
      <TD width="10%">
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
          <xsl:value-of select="Type" />
        </FONT>
      </TD>
      <!-- Geometry -->
      <TD width="10%">
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
          <xsl:value-of select="Geometry" />
        </FONT>
      </TD>
      <!-- Subtype + Subtype Row/Feature Count -->
      <xsl:choose>
        <xsl:when test="count(Subtype)=0">
          <!--  Dataset without Subtypes  -->
          <TD width="30%" colspan="2">
            <FONT>
              <xsl:attribute name="face">
                <xsl:value-of select="$font" />
              </xsl:attribute>
              <xsl:attribute name="color">
                <xsl:value-of select="$forecolor" />
              </xsl:attribute>
              <xsl:attribute name="size">
                <xsl:value-of select="$size1" />
              </xsl:attribute>-
            </FONT>
          </TD>
        </xsl:when>
        <xsl:otherwise>
          <!--  Dataset with Subtypes  -->
          <TD width="25%">
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
              <xsl:for-each select="Subtype">
                <xsl:sort select="Name" />
                <xsl:value-of select="Name" />
                <BR />
              </xsl:for-each>
            </FONT>
          </TD>
          <TD width="5%" align="right">
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
              <xsl:for-each select="Subtype">
                <xsl:sort select="Name" />
                <xsl:value-of select="RowCount" />
                <BR />
              </xsl:for-each>
            </FONT>
          </TD>
        </xsl:otherwise>
      </xsl:choose>
      <!-- Dataset Row/Feature Count -->
      <TD width="5%" align="right">
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
          <xsl:value-of select="RowCount" />
        </FONT>
      </TD>
      <!-- Dataset Extent -->
      <TD width="10%">
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
          <xsl:choose>
            <xsl:when test="count(Extent)=1">
              <xsl:value-of select="Extent/XMin" />
              <BR />
              <xsl:value-of select="Extent/XMax" />
              <BR />
              <xsl:value-of select="Extent/YMin" />
              <BR />
              <xsl:value-of select="Extent/YMax" />
              <BR />
            </xsl:when>
            <xsl:otherwise>No Extent</xsl:otherwise>
          </xsl:choose>
        </FONT>
      </TD>
      <!-- Snapshot Thumbnail and Hyperlink -->
      <TD width="10%" valign="middle" align="center">
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
          <xsl:if test="count(SmallImage)=0 and count(LargeImage)=0">-</xsl:if>
          <xsl:if test="count(SmallImage)=1 and count(LargeImage)=0">
            <img alt="">
              <xsl:attribute name="src">
                <xsl:value-of select="SmallImage" />
              </xsl:attribute>
            </img>
          </xsl:if>
          <xsl:if test="count(SmallImage)=0 and count(LargeImage)=1">
            <A target="AD">
              <xsl:attribute name="href">
                <xsl:value-of select="LargeImage" />
              </xsl:attribute>
              snap
            </A>
          </xsl:if>
          <xsl:if test="count(SmallImage)=1 and count(LargeImage)=1">
            <A target="AD">
              <xsl:attribute name="href">
                <xsl:value-of select="LargeImage" />
              </xsl:attribute>
              <img alt="Click for larger picture...">
                <xsl:attribute name="src">
                  <xsl:value-of select="SmallImage" />
                </xsl:attribute>
              </img>
            </A>
          </xsl:if>
        </FONT>
      </TD>
    </TR>
  </xsl:template>
</xsl:stylesheet>