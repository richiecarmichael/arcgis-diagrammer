<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format"
	xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
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
  <xsl:param name="file" select="file" />
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
                  <xsl:value-of select="$assemblyversion" />
                </FONT>
              </TD>
            </TR>
            <xsl:if test="count(esri:Workspace/WorkspaceDefinition) > 0">
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
                  <FONT font="$font" color="$forecolor" size="$size2" />
                </TD>
                <TD width="25%">
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute><xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute><xsl:attribute name="size">
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
                    <xsl:apply-templates select="esri:Workspace/WorkspaceDefinition/WorkspaceType" />
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
                    <xsl:value-of select="$file" />
                  </FONT>
                </TD>
              </TR>
            </xsl:if>
          </TABLE>
          <P />
          <!--                     -->
          <!--  TABLE OF CONTENTS  -->
          <!--                     -->
          <HR />
          <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#e0ffff" border="0">
            <TR>
              <TD colspan="2">
                <P align="center">
                  <FONT>
                    <xsl:attribute name="face">
                      <xsl:value-of select="$font" />
                    </xsl:attribute><xsl:attribute name="color">
                      <xsl:value-of select="$forecolor" />
                    </xsl:attribute><xsl:attribute name="size">
                      <xsl:value-of select="$size4" />
                    </xsl:attribute>
                    Table Of Contents
                  </FONT>
                </P>
              </TD>
            </TR>
            <!-- TOC:DOMAINS -->
            <xsl:if test="count(esri:Workspace/WorkspaceDefinition/Domains/Domain) > 0">
              <TR>
                <TD width="30%">
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
                      </xsl:attribute>
                      <A href="#DM">Domains</A>
                    </FONT>
                  </B>
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
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>
                    <EM>
                      Listing of Coded Value and Range Domains.
                    </EM>
                  </FONT>
                </TD>
              </TR>
            </xsl:if>
            <!-- ObjectClasses -->
            <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement) > 0">
              <TR>
                <TD width="30%">
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
                      </xsl:attribute>
                      <A href="#OC">ObjectClasses</A>
                    </FONT>
                  </B>
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
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>
                    <EM>
                      Listing of Tables and FeatureClasses.
                    </EM>
                  </FONT>
                </TD>
              </TR>
            </xsl:if>
            <!-- Relationships -->
            <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DERelationshipClass']) > 0 or count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DERelationshipClass']) > 0">
              <TR>
                <TD width="30%">
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
                      </xsl:attribute>
                      <A href="#RL">Relationships</A>
                    </FONT>
                  </B>
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
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>
                    <EM>
                      Listing of Geodatabase Relatioships.
                    </EM>
                  </FONT>
                </TD>
              </TR>
            </xsl:if>
            <!-- Geometric Network -->
            <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DEGeometricNetwork']) > 0">
              <TR>
                <TD width="30%">
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
                      </xsl:attribute>
                      <A href="#GN">Geometry Networks</A>
                    </FONT>
                  </B>
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
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>
                    <EM>
                      Listing of GeometricNetworks (including rules and weights).
                    </EM>
                  </FONT>
                </TD>
              </TR>
            </xsl:if>
            <!-- Topologies -->
            <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DETopology']) > 0">
              <TR>
                <TD width="30%">
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
                      </xsl:attribute>
                      <A href="#TP">Topologies</A>
                    </FONT>
                  </B>
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
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>
                    <EM>
                      Listing of Topology Datasets.
                    </EM>
                  </FONT>
                </TD>
              </TR>
            </xsl:if>
            <!-- Spatial Reference -->
            <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']) > 0 or count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureClass']) > 0">
              <TR>
                <TD width="30%">
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
                      </xsl:attribute>
                      <A href="#SR">Spatial Reference</A>
                    </FONT>
                  </B>
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
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>
                    <EM>
                      Listing of Spatial References used by FeatureClasses and FeatureDatasets.
                    </EM>
                  </FONT>
                </TD>
              </TR>
            </xsl:if>
            <!-- Data Report -->
            <xsl:if test="count(esri:Workspace/GeodatabaseReporter/DataReport) > 0">
              <TR>
                <TD width="30%">
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
                      </xsl:attribute>
                      <A href="#DR">Data Report</A>
                    </FONT>
                  </B>
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
                      <xsl:value-of select="$size1" />
                    </xsl:attribute>
                    <EM>
                      Row/Feature Count for all Tables, FeatureClass and Subtypes.
                    </EM>
                  </FONT>
                </TD>
              </TR>
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
          <!--           -->
          <!--  DOMAINS  -->
          <!--           -->
          <xsl:if test="count(esri:Workspace/WorkspaceDefinition/Domains/Domain) > 0">
            <HR />
            <!--  DOMAINS HEARDER -->
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
                      <A name="DM">Domains</A>
                    </FONT>
                  </P>
                </TD>
              </TR>
            </TABLE>
            <BR />
            <!--  DOMAINS SUMMARY -->
            <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#faebd7" border="0">
              <TR>
                <TD width="50%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size2" />
                      </xsl:attribute>Domain Name
                    </FONT>
                  </B>
                </TD>
                <TD width="30%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size2" />
                      </xsl:attribute>Owner
                    </FONT>
                  </B>
                </TD>
                <TD width="20%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size2" />
                      </xsl:attribute>Domain Type
                    </FONT>
                  </B>
                </TD>
              </TR>
              <xsl:for-each select="esri:Workspace/WorkspaceDefinition/Domains/Domain">
                <xsl:sort select="DomainName" />
                <TR>
                  <TD width="50%">
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
                      <A>
                        <xsl:attribute name="HREF">#DM_<xsl:value-of select="DomainName" /></xsl:attribute>
                        <xsl:value-of select="DomainName" />
                      </A>
                    </FONT>
                  </TD>
                  <TD width="30%">
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
                      <xsl:value-of select="Owner" />
                    </FONT>
                  </TD>
                  <TD width="20%">
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
                      <xsl:apply-templates select="." mode="GetDomainType" />
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
            <HR />
            <!--  DOMAIN LISTING  -->
            <xsl:for-each select="esri:Workspace/WorkspaceDefinition/Domains/Domain">
              <xsl:sort select="DomainName" />
              <P>
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
                  <B>
                    <A>
                      <xsl:attribute name="NAME">DM_<xsl:value-of select="DomainName" /></xsl:attribute>
                      <xsl:value-of select="DomainName" />
                    </A>
                  </B>
                </FONT>
              </P>
              <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#e0ffff" border="0">
                <TR>
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
                      <B>Owner</B>
                    </FONT>
                  </TD>
                  <TD width="75%">
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
                      <xsl:value-of select="Owner" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
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
                      <B>Description</B>
                    </FONT>
                  </TD>
                  <TD width="75%">
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
                      <xsl:value-of select="Description" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
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
                      <B>Domain Type</B>
                    </FONT>
                  </TD>
                  <TD width="75%">
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
                      <xsl:apply-templates select="." mode="GetDomainType" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
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
                      <B>Field Type</B>
                    </FONT>
                  </TD>
                  <TD width="75%">
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
                      <xsl:apply-templates select="FieldType" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
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
                      <B>Merge Policy</B>
                    </FONT>
                  </TD>
                  <TD width="75%">
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
                      <xsl:apply-templates select="MergePolicy" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
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
                      <B>Split Policy</B>
                    </FONT>
                  </TD>
                  <TD width="75%">
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
                      <xsl:apply-templates select="SplitPolicy" />
                    </FONT>
                  </TD>
                </TR>
              </TABLE>
              <!--  DOMAIN MEMBERS  -->
              <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#ccffcc" border="0">
                <TR>
                  <TD colspan="2">
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
                      <B>Domain Members</B>
                    </FONT>
                  </TD>
                </TR>
                <TR>
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
                      <B>Name</B>
                    </FONT>
                  </TD>
                  <TD width="75%">
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
                      <B>Value</B>
                    </FONT>
                  </TD>
                </TR>
                <xsl:choose>
                  <xsl:when test="@xsi:type='esri:RangeDomain'">
                    <TR>
                      <TD width="25%">
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size2" />
                          </xsl:attribute>MinValue
                        </FONT>
                      </TD>
                      <TD width="75%">
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
                          <xsl:value-of select="MinValue" />
                        </FONT>
                      </TD>
                    </TR>
                    <TR>
                      <TD width="25%">
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size2" />
                          </xsl:attribute>MaxValue
                        </FONT>
                      </TD>
                      <TD width="75%">
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
                          <xsl:value-of select="MaxValue" />
                        </FONT>
                      </TD>
                    </TR>
                  </xsl:when>
                  <xsl:when test="@xsi:type='esri:CodedValueDomain'">
                    <xsl:for-each select="CodedValues/CodedValue">
                      <TR>
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
                            <xsl:value-of select="Name" />
                          </FONT>
                        </TD>
                        <TD width="75%">
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
                            <xsl:value-of select="Code" />
                          </FONT>
                        </TD>
                      </TR>
                    </xsl:for-each>
                  </xsl:when>
                </xsl:choose>
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
            </xsl:for-each>
          </xsl:if>
          <!--               -->
          <!-- OBJECTCLASSES -->
          <!--               -->
          <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement) > 0">
            <!--  OBJECTCLASS SUMMARY TABLE  -->
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
                      <A name="OC">ObjectClasses</A>
                    </FONT>
                  </P>
                </TD>
              </TR>
            </TABLE>
            <BR />
            <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#faebd7" border="1" bordercolor="#a9a9a9">
              <TR>
                <TD width="40%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size2" />
                      </xsl:attribute>ObjectClass Name
                    </FONT>
                  </B>
                </TD>
                <TD width="15%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size2" />
                      </xsl:attribute>Type
                    </FONT>
                  </B>
                </TD>
                <TD width="15%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
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
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size2" />
                      </xsl:attribute>Subtype
                    </FONT>
                  </B>
                </TD>
              </TR>
              <!--  FEATUREDATASET OBJECTCLASS(S) -->
              <xsl:for-each select="esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']">
                <xsl:sort select="Name" />
                <TR>
                  <TD width="98%" colspan="4">
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
                        </xsl:attribute>
                        <xsl:value-of select="Name" />
                      </FONT>
                    </B>
                  </TD>
                  <TD width="2%">
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
                      <A>
                        <xsl:attribute name="HREF">#SR_<xsl:value-of select="Name" /></xsl:attribute>
                        SR
                      </A>
                    </FONT>
                  </TD>
                </TR>
                <xsl:apply-templates select="Children/DataElement[@xsi:type='esri:DEFeatureClass']" mode="summary">
                  <xsl:sort select="Name" />
                </xsl:apply-templates>
              </xsl:for-each>
              <!--  STANDALONE OBJECTCLASS(S) -->
              <TR>
                <TD colspan="5">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size2" />
                      </xsl:attribute>
                      Stand Alone ObjectClass(s)
                    </FONT>
                  </B>
                </TD>
              </TR>
              <xsl:apply-templates select="esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureClass'] | esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DETable']"
								mode="summary">
                <xsl:sort select="Name" />
              </xsl:apply-templates>
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
            <HR />
            <!--  OBJECTCLASS LISTING TEMPLATE  -->
            <xsl:apply-templates select="esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DEFeatureClass'] | esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureClass'] | esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DETable']"
							mode="listing">
              <xsl:sort select="Name" />
            </xsl:apply-templates>
          </xsl:if>
          <!--                -->
          <!--  RELATIONSHIP  -->
          <!--                -->
          <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DERelationshipClass']) > 0 or count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DERelationshipClass']) > 0">
            <HR />
            <!-- Relationship Header -->
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
                      <A name="RL">Relationships</A>
                    </FONT>
                  </P>
                </TD>
              </TR>
            </TABLE>
            <BR />
            <!-- Relationship Summary -->
            <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#faebd7" border="0">
              <TR>
                <TD width="35%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>Name
                    </FONT>
                  </B>
                </TD>
                <TD width="25%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>Origin
                    </FONT>
                  </B>
                </TD>
                <TD width="25%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>Destination
                    </FONT>
                  </B>
                </TD>
                <TD width="5%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>Attributed
                    </FONT>
                  </B>
                </TD>
                <TD width="5%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>Composite
                    </FONT>
                  </B>
                </TD>
                <TD width="5%">
                  <B>
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>Rules
                    </FONT>
                  </B>
                </TD>
              </TR>
              <xsl:for-each select="esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DERelationshipClass'] | esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DERelationshipClass']">
                <xsl:sort select="Name" />
                <TR>
                  <TD width="35%">
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
                      <A>
                        <xsl:attribute name="HREF">#RL_<xsl:value-of select="Name" /></xsl:attribute>
                        <xsl:value-of select="Name" />
                      </A>
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
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>
                      <xsl:value-of select="OriginClassNames/Name" />
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
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>
                      <xsl:value-of select="DestinationClassNames/Name" />
                    </FONT>
                  </TD>
                  <TD width="5%">
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
                        <xsl:when test="IsAttributed='true'">Yes</xsl:when>
                        <xsl:otherwise>No</xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                  <TD width="5%">
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
                        <xsl:when test="IsComposite='true'">Yes</xsl:when>
                        <xsl:otherwise>No</xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                  <TD width="5%">
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
                        <xsl:when test="count(RelationshipRules/RelationshipRule) > 0">Yes</xsl:when>
                        <xsl:otherwise>No</xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                </TR>
              </xsl:for-each>
            </TABLE>
            <P />
            <HR />
            <!-- Relationship Listing -->
            <xsl:for-each select="esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DERelationshipClass'] | esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DERelationshipClass']">
              <xsl:sort select="Name" />
              <P>
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
                  <B>
                    <A>
                      <xsl:attribute name="NAME">RL_<xsl:value-of select="Name" /></xsl:attribute>
                      <xsl:value-of select="Name" />
                    </A>
                  </B>
                </FONT>
              </P>
              <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#e0ffff" border="0">
                <TR>
                  <TD width="15%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>Composite
                      </FONT>
                    </B>
                  </TD>
                  <TD colspan="2">
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
                        <xsl:when test="IsComposite='true'">Yes</xsl:when>
                        <xsl:otherwise>No</xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="15%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>Cardinality
                      </FONT>
                    </B>
                  </TD>
                  <TD colspan="2">
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
                      <xsl:apply-templates select="Cardinality" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="15%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>Notification
                      </FONT>
                    </B>
                  </TD>
                  <TD colspan="2">
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
                      <xsl:apply-templates select="Notification" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="15%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>Attributed
                      </FONT>
                    </B>
                  </TD>
                  <TD colspan="2">
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
                        <xsl:when test="IsAttributed='true'">
                          <xsl:variable name="OriginForeignKey" select="OriginClassKeys/RelationshipClassKey/KeyRole[.='esriRelKeyRoleOriginForeign']/../ObjectKeyName" />
                          <xsl:variable name="DestinationForeign" select="DestinationClassKeys/RelationshipClassKey/KeyRole[.='esriRelKeyRoleDestinationForeign']/../ObjectKeyName" />
                          <xsl:value-of select="$OriginForeignKey" /> (<EM>Origin Foreign Key</EM>)<BR />
                          <xsl:value-of select="$DestinationForeign" /> (<EM>Destination Foreign Key</EM>)<BR />
                          <xsl:if test="count(Fields/FieldArray/Field)>0">
                            <xsl:for-each select="Fields/FieldArray/Field">
                              <xsl:if test="Type!='esriFieldTypeOID'">
                                <xsl:if test="Name!=$OriginForeignKey and Name!=$DestinationForeign">
                                  <xsl:value-of select="Name" /> (<EM>Non-Key</EM>)<BR />
                                </xsl:if>
                              </xsl:if>
                            </xsl:for-each>
                          </xsl:if>
                        </xsl:when>
                        <xsl:otherwise>No</xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                </TR>
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
                        </xsl:attribute>
                      </FONT>
                    </B>
                  </TD>
                  <TD width="42%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>Origin
                      </FONT>
                    </B>
                  </TD>
                  <TD width="43%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>Destination
                      </FONT>
                    </B>
                  </TD>
                </TR>
                <TR>
                  <TD width="15%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>ObjectClass
                      </FONT>
                    </B>
                  </TD>
                  <TD width="42%">
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
                      <xsl:value-of select="OriginClassNames/Name" />
                    </FONT>
                  </TD>
                  <TD width="43%">
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
                      <xsl:value-of select="DestinationClassNames/Name" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="15%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>Key
                      </FONT>
                    </B>
                  </TD>
                  <TD width="42%">
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>
                      <xsl:value-of select="OriginClassKeys/RelationshipClassKey/KeyRole[.='esriRelKeyRoleOriginPrimary']/../ObjectKeyName" /> (<EM>Origin Primary Key</EM>)
                    </FONT>
                  </TD>
                  <TD width="43%">
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
                        <xsl:when test="IsAttributed='true'">
                          <xsl:value-of select="DestinationClassKeys/RelationshipClassKey/KeyRole[.='esriRelKeyRoleDestinationPrimary']/../ObjectKeyName" /> (<EM>Destination Primary Key</EM>)
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="OriginClassKeys/RelationshipClassKey/KeyRole[.='esriRelKeyRoleOriginForeign']/../ObjectKeyName" /> (<EM>Origin Foreign Key</EM>)
                        </xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="15%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>Labels
                      </FONT>
                    </B>
                  </TD>
                  <TD width="42%">
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
                      <xsl:value-of select="BackwardPathLabel" />
                    </FONT>
                  </TD>
                  <TD width="43%">
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
                      <xsl:value-of select="ForwardPathLabel" />
                    </FONT>
                  </TD>
                </TR>
              </TABLE>
              <!-- Relationship Rules Listing -->
              <xsl:if test="count(RelationshipRules/RelationshipRule) > 0">
                <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#ccffcc" border="0">
                  <TR>
                    <TD width="15%">
                      <xsl:attribute name="rowSpan">
                        <xsl:value-of select="count(RelationshipRules/RelationshipRule) + 1" />
                      </xsl:attribute>
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>Rules
                        </FONT>
                      </B>
                    </TD>
                    <TD width="32%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>Subtype
                        </FONT>
                      </B>
                    </TD>
                    <TD width="10%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>Origin Cardinality
                        </FONT>
                      </B>
                    </TD>
                    <TD width="33%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>Subtype
                        </FONT>
                      </B>
                    </TD>
                    <TD width="10%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>Destination Cardinality
                        </FONT>
                      </B>
                    </TD>
                  </TR>
                  <xsl:for-each select="RelationshipRules/RelationshipRule">
                    <TR>
                      <TD width="32%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                            <xsl:with-param name="objectClassID" select="OriginClassID" />
                            <xsl:with-param name="subtypeCode" select="OriginSubtypeCode" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
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
                            <xsl:when test="count(OriginMinimumCardinality)=0">
                              <xsl:choose>
                                <xsl:when test="../../Cardinality='esriRelCardinalityOneToOne'">1</xsl:when>
                                <xsl:when test="../../Cardinality='esriRelCardinalityOneToMany'">1</xsl:when>
                                <xsl:when test="../../Cardinality='esriRelCardinalityManyToMany'">M</xsl:when>
                                <xsl:otherwise>-</xsl:otherwise>
                              </xsl:choose>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="OriginMinimumCardinality" />..<xsl:value-of select="OriginMaximumCardinality" />
                            </xsl:otherwise>
                          </xsl:choose>
                        </FONT>
                      </TD>
                      <TD width="33%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                            <xsl:with-param name="objectClassID" select="DestinationClassID" />
                            <xsl:with-param name="subtypeCode" select="DestinationSubtypeCode" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
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
                            <xsl:when test="count(DestinationMinimumCardinality)=0">
                              <xsl:choose>
                                <xsl:when test="../../Cardinality='esriRelCardinalityOneToOne'">1</xsl:when>
                                <xsl:when test="../../Cardinality='esriRelCardinalityOneToMany'">M</xsl:when>
                                <xsl:when test="../../Cardinality='esriRelCardinalityManyToMany'">M</xsl:when>
                                <xsl:otherwise>-</xsl:otherwise>
                              </xsl:choose>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="DestinationMinimumCardinality" />..<xsl:value-of select="DestinationMaximumCardinality" />
                            </xsl:otherwise>
                          </xsl:choose>
                        </FONT>
                      </TD>
                    </TR>
                  </xsl:for-each>
                </TABLE>
              </xsl:if>
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
            </xsl:for-each>
          </xsl:if>
          <!--                     -->
          <!--  GEOMETRIC NETWORK  -->
          <!--                     -->
          <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DEGeometricNetwork'])>0">
            <HR />
            <!-- Header  -->
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
                      <A name="GN">Geometric Networks</A>
                    </FONT>
                  </P>
                </TD>
              </TR>
            </TABLE>
            <BR />
            <xsl:for-each select="esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DEGeometricNetwork']">
              <xsl:sort select="Name" />
              <!-- List of Network FeatureClass  -->
              <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#faebd7" border="0">
                <TR>
                  <TD colspan="3">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Name: <xsl:value-of select="Name" />
                      </FONT>
                    </B>
                  </TD>
                </TR>
                <TR>
                  <TD width="25%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Feature Class
                      </FONT>
                    </B>
                  </TD>
                  <TD width="25%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Network Role
                      </FONT>
                    </B>
                  </TD>
                  <TD width="50%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Ancillary Role
                      </FONT>
                    </B>
                  </TD>
                </TR>
                <xsl:for-each select="FeatureClassNames/Name">
                  <xsl:sort select="." />
                  <TR>
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
                        <xsl:value-of select="." />
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
                        <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetFeatureTypeFromName">
                          <xsl:with-param name="name" select="." />
                        </xsl:apply-templates>
                      </FONT>
                    </TD>
                    <TD width="50%">
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
                        <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetAncillaryRoleFromName">
                          <xsl:with-param name="name" select="." />
                        </xsl:apply-templates>
                      </FONT>
                    </TD>
                  </TR>
                </xsl:for-each>
              </TABLE>
              <!-- Table of Edge Edge Connectivity Rules  -->
              <xsl:if test="count(ConnectivityRules/ConnectivityRule[@xsi:type='esri:EdgeConnectivityRule'])>0">
                <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#e0ffff" border="0">
                  <TR>
                    <TD colspan="5">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Edge Edge Connectivity Rules
                        </FONT>
                      </B>
                    </TD>
                  </TR>
                  <TR>
                    <TD width="17%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          From Edge
                        </FONT>
                      </B>
                    </TD>
                    <TD width="18%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          From Edge Subtype
                        </FONT>
                      </B>
                    </TD>
                    <TD width="17%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          To Edge
                        </FONT>
                      </B>
                    </TD>
                    <TD width="18%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          To Edge Subtype
                        </FONT>
                      </B>
                    </TD>
                    <TD width="30%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Via Junction::Subtype
                        </FONT>
                      </B>
                    </TD>
                  </TR>
                  <xsl:for-each select="ConnectivityRules/ConnectivityRule[@xsi:type='esri:EdgeConnectivityRule']">
                    <TR>
                      <TD width="17%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetObjectClassNameFromObjectClassID">
                            <xsl:with-param name="objectClassID" select="FromClassID" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
                      <TD width="18%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                            <xsl:with-param name="objectClassID" select="FromClassID" />
                            <xsl:with-param name="subtypeCode" select="FromEdgeSubtypeCode" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
                      <TD width="17%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetObjectClassNameFromObjectClassID">
                            <xsl:with-param name="objectClassID" select="ToClassID" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
                      <TD width="18%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                            <xsl:with-param name="objectClassID" select="ToClassID" />
                            <xsl:with-param name="subtypeCode" select="ToEdgeSubtypeCode" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
                      <TD width="30%">
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
                          <xsl:for-each select="JunctionSubtypes/JunctionSubtype">
                            <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetObjectClassNameFromObjectClassID">
                              <xsl:with-param name="objectClassID" select="ClassID" />
                            </xsl:apply-templates>::
                            <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                              <xsl:with-param name="objectClassID" select="ClassID" />
                              <xsl:with-param name="subtypeCode" select="SubtypeCode" />
                            </xsl:apply-templates>
                            <xsl:if test="ClassID=../../DefaultJunctionID and SubtypeCode=../../DefaultJunctionSubtypeCode">
                              <EM>(Default)</EM>
                            </xsl:if>
                            <BR />
                          </xsl:for-each>
                        </FONT>
                      </TD>
                    </TR>
                  </xsl:for-each>
                </TABLE>
              </xsl:if>
              <!-- Table of Edge Junction Connectivity Rules -->
              <xsl:if test="count(ConnectivityRules/ConnectivityRule[@xsi:type='esri:JunctionConnectivityRule'])>0">
                <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#fafad2" border="0">
                  <TR>
                    <TD colspan="8">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Edge Junction Connectivity Rules
                        </FONT>
                      </B>
                    </TD>
                  </TR>
                  <TR>
                    <TD width="20%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          From Edge
                        </FONT>
                      </B>
                    </TD>
                    <TD width="20%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          From Edge Subtype
                        </FONT>
                      </B>
                    </TD>
                    <TD width="20%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          To Junction
                        </FONT>
                      </B>
                    </TD>
                    <TD width="20%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          To Junction Subtype
                        </FONT>
                      </B>
                    </TD>
                    <TD width="5%" align="right">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Min E
                        </FONT>
                      </B>
                    </TD>
                    <TD width="5%" align="right">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Max E
                        </FONT>
                      </B>
                    </TD>
                    <TD width="5%" align="right">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Min J
                        </FONT>
                      </B>
                    </TD>
                    <TD width="5%" align="right">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Max J
                        </FONT>
                      </B>
                    </TD>
                  </TR>
                  <xsl:for-each select="ConnectivityRules/ConnectivityRule[@xsi:type='esri:JunctionConnectivityRule']">
                    <TR>
                      <TD width="20%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetObjectClassNameFromObjectClassID">
                            <xsl:with-param name="objectClassID" select="EdgeClassID" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
                      <TD width="20%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                            <xsl:with-param name="objectClassID" select="EdgeClassID" />
                            <xsl:with-param name="subtypeCode" select="EdgeSubtypeCode" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
                      <TD width="20%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetObjectClassNameFromObjectClassID">
                            <xsl:with-param name="objectClassID" select="JunctionClassID" />
                          </xsl:apply-templates>
                        </FONT>
                      </TD>
                      <TD width="20%">
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
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                            <xsl:with-param name="objectClassID" select="JunctionClassID" />
                            <xsl:with-param name="subtypeCode" select="SubtypeCode" />
                          </xsl:apply-templates>
                          <xsl:if test="IsDefault='true'">
                            <EM>(Default)</EM>
                          </xsl:if>
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
                          <xsl:choose>
                            <xsl:when test="count(EdgeMinimumCardinality)=1">
                              <xsl:value-of select="EdgeMinimumCardinality" />
                            </xsl:when>
                            <xsl:otherwise>-</xsl:otherwise>
                          </xsl:choose>
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
                          <xsl:choose>
                            <xsl:when test="count(EdgeMaximumCardinality)=1">
                              <xsl:value-of select="EdgeMaximumCardinality" />
                            </xsl:when>
                            <xsl:otherwise>-</xsl:otherwise>
                          </xsl:choose>
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
                          <xsl:choose>
                            <xsl:when test="count(JunctionMinimumCardinality)=1">
                              <xsl:value-of select="JunctionMinimumCardinality" />
                            </xsl:when>
                            <xsl:otherwise>-</xsl:otherwise>
                          </xsl:choose>
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
                          <xsl:choose>
                            <xsl:when test="count(JunctionMaximumCardinality)=1">
                              <xsl:value-of select="JunctionMaximumCardinality" />
                            </xsl:when>
                            <xsl:otherwise>-</xsl:otherwise>
                          </xsl:choose>
                        </FONT>
                      </TD>
                    </TR>
                  </xsl:for-each>
                </TABLE>
              </xsl:if>
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
            </xsl:for-each>
          </xsl:if>
          <!--            -->
          <!--  TOPOLOGY  -->
          <!--            -->
          <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DETopology'])>0">
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
                      <A name="TP">Topologies</A>
                    </FONT>
                  </P>
                </TD>
              </TR>
            </TABLE>
            <BR />
            <xsl:for-each select="esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']/Children/DataElement[@xsi:type='esri:DETopology']">
              <xsl:sort select="Name" />
              <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#faebd7" border="0">
                <TR>
                  <TD colspan="3" valign="top">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Name: <xsl:value-of select="Name" />
                      </FONT>
                    </B>
                  </TD>
                  <TD colspan="2">
                    <FONT>
                      <xsl:attribute name="face">
                        <xsl:value-of select="$font" />
                      </xsl:attribute><xsl:attribute name="color">
                        <xsl:value-of select="$forecolor" />
                      </xsl:attribute><xsl:attribute name="size">
                        <xsl:value-of select="$size1" />
                      </xsl:attribute>
                      Cluster Tolerance:
                      <xsl:value-of select="ClusterTolerance" />
                      <BR />
                      Maximum Generated Error Count:
                      <xsl:choose>
                        <xsl:when test="MaxGeneratedErrorCount='-1'">Undefined</xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="MaxGeneratedErrorCount" />
                        </xsl:otherwise>
                      </xsl:choose>
                      <BR />
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="30%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Feature Class
                      </FONT>
                    </B>
                  </TD>
                  <TD width="10%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Weight
                      </FONT>
                    </B>
                  </TD>
                  <TD width="10%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        XY Rank
                      </FONT>
                    </B>
                  </TD>
                  <TD width="10%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Z Rank
                      </FONT>
                    </B>
                  </TD>
                  <TD width="40%">
                    <B>
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size2" />
                        </xsl:attribute>
                        Event Notification
                      </FONT>
                    </B>
                  </TD>
                </TR>
                <xsl:for-each select="FeatureClassNames/Name">
                  <xsl:sort select="." />
                  <TR>
                    <TD width="30%">
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
                        <xsl:value-of select="." />
                      </FONT>
                    </TD>
                    <TD width="10%">
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
                        <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetTopologyWeightFromName">
                          <xsl:with-param name="name" select="." />
                        </xsl:apply-templates>
                      </FONT>
                    </TD>
                    <TD width="10%">
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
                        <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetTopologyXYRankFromName">
                          <xsl:with-param name="name" select="." />
                        </xsl:apply-templates>
                      </FONT>
                    </TD>
                    <TD width="10%">
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
                        <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetTopologyZRankFromName">
                          <xsl:with-param name="name" select="." />
                        </xsl:apply-templates>
                      </FONT>
                    </TD>
                    <TD width="40%">
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
                        <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetTopologyEventNotificationFromName">
                          <xsl:with-param name="name" select="." />
                        </xsl:apply-templates>
                      </FONT>
                    </TD>
                  </TR>
                </xsl:for-each>
              </TABLE>
              <xsl:if test="count(TopologyRules/TopologyRule)>0">
                <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#e0ffff" border="0">
                  <TR>
                    <TD colspan="5">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Topology Rules
                        </FONT>
                      </B>
                    </TD>
                  </TR>
                  <TR>
                    <TD width="10%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Name
                        </FONT>
                      </B>
                    </TD>
                    <TD width="25%">
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>
                        <B>Origin</B> (<EM>FeatureClass::Subtype</EM>)
                      </FONT>
                    </TD>
                    <TD width="25%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Rule Type
                        </FONT>
                      </B>
                    </TD>
                    <TD width="25%">
                      <FONT>
                        <xsl:attribute name="face">
                          <xsl:value-of select="$font" />
                        </xsl:attribute><xsl:attribute name="color">
                          <xsl:value-of select="$forecolor" />
                        </xsl:attribute><xsl:attribute name="size">
                          <xsl:value-of select="$size1" />
                        </xsl:attribute>
                        <B>Destination</B> (<EM>FeatureClass::Subtype</EM>)
                      </FONT>
                    </TD>
                    <TD width="15%">
                      <B>
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          Trigger Events
                        </FONT>
                      </B>
                    </TD>
                  </TR>
                  <xsl:for-each select="TopologyRules/TopologyRule">
                    <xsl:sort select="Name" />
                    <xsl:sort select="TopologyRuleType" data-type="number" />
                    <TR>
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
                            <xsl:when test="@name=''">-</xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="Name" />
                            </xsl:otherwise>
                          </xsl:choose>
                        </FONT>
                      </TD>
                      <TD width="25%">
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetObjectClassNameFromObjectClassID">
                            <xsl:with-param name="objectClassID" select="OriginClassID" />
                          </xsl:apply-templates>::
                          <xsl:choose>
                            <xsl:when test="AllOriginSubtypes='true'">All Subtypes</xsl:when>
                            <xsl:otherwise>
                              <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                                <xsl:with-param name="objectClassID" select="OriginClassID" />
                                <xsl:with-param name="subtypeCode" select="OriginSubtype" />
                              </xsl:apply-templates>
                            </xsl:otherwise>
                          </xsl:choose>
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
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          <xsl:apply-templates select="TopologyRuleType" />
                        </FONT>
                      </TD>
                      <TD width="25%">
                        <FONT>
                          <xsl:attribute name="face">
                            <xsl:value-of select="$font" />
                          </xsl:attribute><xsl:attribute name="color">
                            <xsl:value-of select="$forecolor" />
                          </xsl:attribute><xsl:attribute name="size">
                            <xsl:value-of select="$size1" />
                          </xsl:attribute>
                          <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetObjectClassNameFromObjectClassID">
                            <xsl:with-param name="objectClassID" select="DestinationClassID" />
                          </xsl:apply-templates>::
                          <xsl:choose>
                            <xsl:when test="AllDestinationSubtypes='true'">All Subtypes</xsl:when>
                            <xsl:otherwise>
                              <xsl:apply-templates select="/esri:Workspace/WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
                                <xsl:with-param name="objectClassID" select="DestinationClassID" />
                                <xsl:with-param name="subtypeCode" select="DestinationSubtype" />
                              </xsl:apply-templates>
                            </xsl:otherwise>
                          </xsl:choose>
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
                          <xsl:choose>
                            <xsl:when test="TriggerErrorEvents='true'">Yes</xsl:when>
                            <xsl:otherwise>No</xsl:otherwise>
                          </xsl:choose>
                        </FONT>
                      </TD>
                    </TR>
                  </xsl:for-each>
                </TABLE>
              </xsl:if>
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
            </xsl:for-each>
          </xsl:if>
          <!--                    -->
          <!-- SPATIAL REFERENCES -->
          <!--                    -->
          <xsl:if test="count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset']) > 0 or count(esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureClass']) > 0">
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
                      <A name="SR">Spatial References</A>
                    </FONT>
                  </P>
                </TD>
              </TR>
            </TABLE>
            <BR />
            <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#faebd7" border="1" bordercolor="#a9a9a9">
              <TR>
                <TD width="10%">
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
                    <B>Dimension</B>
                  </FONT>
                </TD>
                <TD width="45%">
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
                    <B>Minimum</B>
                  </FONT>
                </TD>
                <TD width="45%">
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
                    <B>Precision</B>
                  </FONT>
                </TD>
              </TR>
              <xsl:for-each select="esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureDataset'] | esri:Workspace/WorkspaceDefinition/DatasetDefinitions/DataElement[@xsi:type='esri:DEFeatureClass']">
                <xsl:sort select="Name" />
                <TR>
                  <TD colSpan="3">
                    <FONT color="#0000aa" size="$size2">
                      <B>
                        <A>
                          <xsl:attribute name="NAME">SR_<xsl:value-of select="Name" /></xsl:attribute>
                          <xsl:value-of select="Name" />
                        </A>
                      </B>
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="10%">
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
                      <B>X</B>
                    </FONT>
                  </TD>
                  <TD width="45%">
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
                      <xsl:value-of select="SpatialReference/XOrigin" />
                    </FONT>
                  </TD>
                  <TD width="45%" rowSpan="2">
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
                      <xsl:value-of select="SpatialReference/XYScale" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="10%">
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
                      <B>Y</B>
                    </FONT>
                  </TD>
                  <TD width="45%">
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
                      <xsl:value-of select="SpatialReference/YOrigin" />
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="10%">
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
                      <B>M</B>
                    </FONT>
                  </TD>
                  <TD width="45%">
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
                      <xsl:choose>
                        <xsl:when test="DatasetType='esriDTFeatureClass'">
                          <xsl:choose>
                            <xsl:when test="HasM='false'">-</xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="SpatialReference/MOrigin" />
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="SpatialReference/MOrigin" />
                        </xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                  <TD width="45%">
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
                      <xsl:choose>
                        <xsl:when test="DatasetType='esriDTFeatureClass'">
                          <xsl:choose>
                            <xsl:when test="HasM='false'">-</xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="SpatialReference/MScale" />
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="SpatialReference/MScale" />
                        </xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD width="10%">
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
                      <B>Z</B>
                    </FONT>
                  </TD>
                  <TD width="45%">
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
                      <xsl:choose>
                        <xsl:when test="DatasetType='esriDTFeatureClass'">
                          <xsl:choose>
                            <xsl:when test="HasZ='false'">-</xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="SpatialReference/ZOrigin" />
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="SpatialReference/ZOrigin" />
                        </xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                  <TD width="45%">
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
                      <xsl:choose>
                        <xsl:when test="DatasetType='esriDTFeatureClass'">
                          <xsl:choose>
                            <xsl:when test="HasZ='false'">-</xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="SpatialReference/ZScale" />
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="SpatialReference/ZScale" />
                        </xsl:otherwise>
                      </xsl:choose>
                    </FONT>
                  </TD>
                </TR>
                <TR>
                  <TD colSpan="3">
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
                      <B>Coordinate System Description</B>
                      <BR />
                      <xsl:value-of select="SpatialReference/WKT" />
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
          </xsl:if>
          <!--          -->
          <!--  FOOTER  -->
          <!--          -->
          <HR />
          <BLOCKQUOTE>
            <FONT>
              <xsl:attribute name="face">
                <xsl:value-of select="$font" />
              </xsl:attribute><xsl:attribute name="color">
                <xsl:value-of select="$forecolor" />
              </xsl:attribute><xsl:attribute name="size">
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
  <!--=================================================================-->
  <!--                          TEMPLATES                              -->
  <!--=================================================================-->
  <!--                       -->
  <!--  OBJECTCLASS SUMMARY  -->
  <!--                       -->
  <xsl:template match="DataElement" mode="summary">
    <TR>
      <TD width="40%">
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
          <A>
            <xsl:attribute name="HREF">#OC_<xsl:value-of select="Name" /></xsl:attribute>
            <xsl:value-of select="Name" />
          </A>
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
            <xsl:value-of select="$size2" />
          </xsl:attribute>
          <xsl:if test="count(FeatureType)=1">
            <xsl:apply-templates select="FeatureType" />&#160;
          </xsl:if>
          <xsl:apply-templates select="DatasetType" />
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
            <xsl:value-of select="$size2" />
          </xsl:attribute>
          <xsl:choose>
            <xsl:when test="count(ShapeType)=1">
              <xsl:apply-templates select="ShapeType" />
            </xsl:when>
            <xsl:otherwise>-</xsl:otherwise>
          </xsl:choose>
        </FONT>
      </TD>
      <xsl:choose>
        <xsl:when test="count(../../DatasetDefinitions)=1 and count(FeatureType)=1">
          <!--  Standalone FeatureClasses  -->
          <TD width="28%">
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
              <xsl:choose>
                <xsl:when test="count(Subtypes)=0">-</xsl:when>
                <xsl:otherwise>
                  <xsl:for-each select="Subtypes/Subtype">
                    <xsl:sort select="SubtypeName" />
                    <xsl:value-of select="SubtypeName" />
                    <BR />
                  </xsl:for-each>
                </xsl:otherwise>
              </xsl:choose>
            </FONT>
          </TD>
          <TD width="2%">
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
              <A>
                <xsl:attribute name="HREF">#SR_<xsl:value-of select="Name" /></xsl:attribute>
                SR
              </A>
            </FONT>
          </TD>
        </xsl:when>
        <xsl:otherwise>
          <!--  Tables and FeatureDataset FeaureClasses  -->
          <TD width="30%" colspan="2">
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
              <xsl:choose>
                <xsl:when test="count(Subtypes)=0">-</xsl:when>
                <xsl:otherwise>
                  <xsl:for-each select="Subtypes/Subtype">
                    <xsl:sort select="SubtypeName" />
                    <xsl:value-of select="SubtypeName" />
                    <BR />
                  </xsl:for-each>
                </xsl:otherwise>
              </xsl:choose>
            </FONT>
          </TD>
        </xsl:otherwise>
      </xsl:choose>
    </TR>
  </xsl:template>
  <!--                       -->
  <!--  OBJECTCLASS LISTING  -->
  <!--                       -->
  <xsl:template match="DataElement" mode="listing">
    <P>
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
        <B>
          <A>
            <xsl:attribute name="NAME">OC_<xsl:value-of select="Name" /></xsl:attribute>
            <xsl:value-of select="Name" />
          </A>
        </B>
      </FONT>
    </P>
    <!--  DESCRIPTION TABLE  -->
    <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#e0ffff" border="0">
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
              <xsl:value-of select="$size2" />
            </xsl:attribute>
            <B>Alias</B>
          </FONT>
        </TD>
        <TD width="35%">
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
            <xsl:value-of select="AliasName" />
          </FONT>
        </TD>
        <TD width="50%" rowSpan="3">
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
            <xsl:if test="count(FeatureType)=1">
              <B>Geometry:</B>
              <xsl:apply-templates select="ShapeType" />
              <BR />
              <B>Average Number of Points:</B>
              <xsl:value-of select="Fields/FieldArray/Field/GeometryDef/AvgNumPoints" />
              <BR />
              <B>Has M:</B>
              <xsl:choose>
                <xsl:when test="Fields/FieldArray/Field/GeometryDef/HasM='true'">Yes</xsl:when>
                <xsl:otherwise>No</xsl:otherwise>
              </xsl:choose>
              <BR />
              <B>Has Z:</B>
              <xsl:choose>
                <xsl:when test="Fields/FieldArray/Field/GeometryDef/HasZ='true'">Yes</xsl:when>
                <xsl:otherwise>No</xsl:otherwise>
              </xsl:choose>
              <BR />
              <B>Grid Size:</B>
              <xsl:if test="count(Fields/FieldArray/Field/GeometryDef/GridSize0)=1">
                <xsl:value-of select="Fields/FieldArray/Field/GeometryDef/GridSize0" />
              </xsl:if>
              <xsl:if test="count(Fields/FieldArray/Field/GeometryDef/GridSize1)=1">
                ,<xsl:value-of select="Fields/FieldArray/Field/GeometryDef/GridSize1" />
              </xsl:if>
              <xsl:if test="count(Fields/FieldArray/Field/GeometryDef/GridSize2)=1">
                ,<xsl:value-of select="Fields/FieldArray/Field/GeometryDef/GridSize2" />
              </xsl:if>
            </xsl:if>
          </FONT>
        </TD>
      </TR>
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
              <xsl:value-of select="$size2" />
            </xsl:attribute>
            <B>Dataset Type</B>
          </FONT>
        </TD>
        <TD width="35%">
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
            <xsl:apply-templates select="DatasetType" />
          </FONT>
        </TD>
      </TR>
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
              <xsl:value-of select="$size2" />
            </xsl:attribute>
            <B>FeatureType</B>
          </FONT>
        </TD>
        <TD width="35%">
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
            <xsl:apply-templates select="FeatureType" />
          </FONT>
        </TD>
      </TR>
    </TABLE>
    <!-- FIELDS TABLE  -->
    <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#ccffcc" border="0">
      <TR>
        <TD width="28%">
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
            <B>Field Name</B>
          </FONT>
        </TD>
        <TD width="27%">
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
            <B>Alias Name</B>
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
            <B>Model Name</B>
          </FONT>
        </TD>
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
            <B>Type</B>
          </FONT>
        </TD>
        <TD width="5%">
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
            <B>Precn.</B>
          </FONT>
        </TD>
        <TD width="5%">
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
            <B>Scale</B>
          </FONT>
        </TD>
        <TD width="5%">
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
            <B>Length</B>
          </FONT>
        </TD>
        <TD width="5%">
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
            <B>Null</B>
          </FONT>
        </TD>
      </TR>
      <xsl:for-each select="Fields/FieldArray/Field">
        <TR>
          <TD width="28%">
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
          <TD width="27%">
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
              <xsl:value-of select="AliasName" />
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
              <xsl:value-of select="ModelName" />
            </FONT>
          </TD>
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
              <xsl:apply-templates select="Type" />
            </FONT>
          </TD>
          <TD width="5%">
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
              <xsl:value-of select="Precision" />
            </FONT>
          </TD>
          <TD width="5%">
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
              <xsl:value-of select="Scale" />
            </FONT>
          </TD>
          <TD width="5%">
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
              <xsl:value-of select="Length" />
            </FONT>
          </TD>
          <TD width="5%">
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
                <xsl:when test="IsNullable='true'">Yes</xsl:when>
                <xsl:otherwise>No</xsl:otherwise>
              </xsl:choose>
            </FONT>
          </TD>
        </TR>
      </xsl:for-each>
    </TABLE>
    <!-- SUBTYPE TABLE  -->
    <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#fafad2" border="0">
      <TR>
        <TD width="33%">
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
            <B>Subtype Name</B>
          </FONT>
        </TD>
        <TD width="33%">
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
            <B>Default Value</B>
          </FONT>
        </TD>
        <TD width="33%">
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
            <B>Domain</B>
          </FONT>
        </TD>
      </TR>
      <xsl:if test="count(Fields/FieldArray/Field/DefaultValue)>0 or count(Fields/FieldArray/Field/Domain)>0">
        <TR>
          <TD colspan="3">
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
              <B>ObjectClass</B>
            </FONT>
          </TD>
        </TR>
        <xsl:for-each select="Fields/FieldArray/Field">
          <xsl:if test="count(DefaultValue)=1 or count(Domain)=1">
            <TR>
              <TD width="33%">
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
              <TD width="33%">
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
                  <xsl:value-of select="DefaultValue" />
                </FONT>
              </TD>
              <TD width="33%">
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
                  <!-- <xsl:value-of select="Domain/DomainName" />-->
                  <xsl:choose>
                    <xsl:when test="count(Domain/DomainName)=0">-</xsl:when>
                    <xsl:otherwise>
                      <A>
                        <xsl:attribute name="HREF">#DM_<xsl:value-of select="Domain/DomainName" /></xsl:attribute>
                        <xsl:value-of select="Domain/DomainName" />
                      </A>
                    </xsl:otherwise>
                  </xsl:choose>
                </FONT>
              </TD>
            </TR>
          </xsl:if>
        </xsl:for-each>
      </xsl:if>
      <xsl:for-each select="Subtypes/Subtype">
        <xsl:sort select="SubtypeName" />
        <TR>
          <TD colspan="3">
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
              <B>
                <xsl:value-of select="SubtypeName" />
                (<xsl:value-of select="../../SubtypeFieldName" />=<xsl:value-of select="SubtypeCode" />)
                <xsl:if test="SubtypeCode = ../../DefaultSubtypeCode">
                  [Default]
                </xsl:if>
              </B>
            </FONT>
          </TD>
        </TR>
        <xsl:for-each select="FieldInfos/SubtypeFieldInfo">
          <xsl:sort select="FieldName" />
          <TR>
            <TD width="33%">
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
                <xsl:value-of select="FieldName" />
              </FONT>
            </TD>
            <TD width="33%">
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
                <xsl:if test="count(DefaultValue)=1">
                  <xsl:value-of select="DefaultValue" />
                </xsl:if>
              </FONT>
            </TD>
            <TD width="33%">
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
                  <xsl:when test="count(DomainName)=0">-</xsl:when>
                  <xsl:otherwise>
                    <A>
                      <xsl:attribute name="HREF">#DM_<xsl:value-of select="DomainName" /></xsl:attribute>
                      <xsl:value-of select="DomainName" />
                    </A>
                  </xsl:otherwise>
                </xsl:choose>
              </FONT>
            </TD>
          </TR>
        </xsl:for-each>
      </xsl:for-each>
    </TABLE>
    <!-- INDEX TABLE  -->
    <xsl:if test="count(Indexes)=1">
      <TABLE cellSpacing="0" cellPadding="0" width="100%" bgColor="#ffe4e1" border="0">
        <TR>
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
              <B>Index Name</B>
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
                <xsl:value-of select="$size1" />
              </xsl:attribute>
              <B>Ascending</B>
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
                <xsl:value-of select="$size1" />
              </xsl:attribute>
              <B>Unique</B>
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
                <xsl:value-of select="$size1" />
              </xsl:attribute>
              <B>Fields</B>
            </FONT>
          </TD>
        </TR>
        <xsl:for-each select="Indexes/IndexArray/Index">
          <xsl:sort select="Name" />
          <TR>
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
                <xsl:choose>
                  <xsl:when test="IsAscending='true'">Yes</xsl:when>
                  <xsl:otherwise>No</xsl:otherwise>
                </xsl:choose>
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
                  <xsl:value-of select="$size1" />
                </xsl:attribute>
                <xsl:choose>
                  <xsl:when test="IsUnique='true'">Yes</xsl:when>
                  <xsl:otherwise>No</xsl:otherwise>
                </xsl:choose>
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
                  <xsl:value-of select="$size1" />
                </xsl:attribute>
                <xsl:for-each select="Fields/FieldArray/Field">
                  <xsl:sort select="Name" />
                  <xsl:value-of select="Name" />
                  <BR />
                </xsl:for-each>
              </FONT>
            </TD>
          </TR>
        </xsl:for-each>
      </TABLE>
    </xsl:if>
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
  </xsl:template>
  <!--                                -->
  <!--  ENUMERATOR ESRIWORKSPACETYPE  -->
  <!--                                -->
  <xsl:template match="WorkspaceType">
    <xsl:choose>
      <xsl:when test=".='esriFileSystemWorkspace'">File System Workspace</xsl:when>
      <xsl:when test=".='esriLocalDatabaseWorkspace'">Personal Geodatabase</xsl:when>
      <xsl:when test=".='esriRemoteDatabaseWorkspace'">ArcSDE Geodatabase</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                            -->
  <!--  ENUMERATOR ESRIFIELDTYPE  -->
  <!--                            -->
  <xsl:template match="FieldType | Type">
    <xsl:choose>
      <xsl:when test=".='esriFieldTypeSmallInteger'">Small Integer</xsl:when>
      <xsl:when test=".='esriFieldTypeInteger'">Integer</xsl:when>
      <xsl:when test=".='esriFieldTypeSingle'">Single</xsl:when>
      <xsl:when test=".='esriFieldTypeDouble'">Double</xsl:when>
      <xsl:when test=".='esriFieldTypeString'">String</xsl:when>
      <xsl:when test=".='esriFieldTypeDate'">Date</xsl:when>
      <xsl:when test=".='esriFieldTypeOID'">OID</xsl:when>
      <xsl:when test=".='esriFieldTypeGeometry'">Geometry</xsl:when>
      <xsl:when test=".='esriFieldTypeBlob'">Blob</xsl:when>
      <xsl:when test=".='esriFieldTypeRaster'">Raster</xsl:when>
      <xsl:when test=".='esriFieldTypeGUID'">GUID</xsl:when>
      <xsl:when test=".='esriFieldTypeGlobalID'">Global ID</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                                  -->
  <!--  ENUMERATOR ESRIDOMAINTYPETYPE  -->
  <!--                                  -->
  <xsl:template match="Domain" mode="GetDomainType">
    <xsl:choose>
      <xsl:when test="./@xsi:type='esri:RangeDomain'">Range Domain</xsl:when>
      <xsl:when test="./@xsi:type='esri:CodedValueDomain'">Coded Value</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                                  -->
  <!--  ENUMERATOR ESRIMERGEPOLICYTYPE  -->
  <!--                                  -->
  <xsl:template match="MergePolicy">
    <xsl:choose>
      <xsl:when test=".='esriMPTSumValues'">Sum Values</xsl:when>
      <xsl:when test=".='esriMPTAreaWeighted'">Area Weighted</xsl:when>
      <xsl:when test=".='esriMPTDefaultValue'">Default Value</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                                  -->
  <!--  ENUMERATOR ESRISPLITPOLICYTYPE  -->
  <!--                                  -->
  <xsl:template match="SplitPolicy">
    <xsl:choose>
      <xsl:when test=".='esriSPTGeometryRatio'">Geometry Ratio</xsl:when>
      <xsl:when test=".='esriSPTDuplicate'">Duplicate</xsl:when>
      <xsl:when test=".='esriSPTDefaultValue'">Default Value</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                              -->
  <!--  ENUMERATOR ESRIDATASETTYPE  -->
  <!--                              -->
  <xsl:template match="DatasetType">
    <xsl:choose>
      <xsl:when test=".='esriDTFeatureClass'">FeatureClass</xsl:when>
      <xsl:when test=".='esriDTTable'">Table</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                               -->
  <!--  ENUMERATOR ESRIGEOMETRYTYPE  -->
  <!--                               -->
  <xsl:template match="ShapeType">
    <xsl:choose>
      <xsl:when test=".='esriGeometryPoint'">Point</xsl:when>
      <xsl:when test=".='esriGeometryMultipoint'">MultiPoint</xsl:when>
      <xsl:when test=".='esriGeometryPolyline'">Polyline</xsl:when>
      <xsl:when test=".='esriGeometryPolygon'">Polygon</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                               -->
  <!--  ENUMERATOR ESRIFEATURETYPE  -->
  <!--                               -->
  <xsl:template match="FeatureType">
    <xsl:choose>
      <xsl:when test=".='esriFTSimple'">Simple</xsl:when>
      <xsl:when test=".='esriFTSimpleJunction'">Simple Junction</xsl:when>
      <xsl:when test=".='esriFTSimpleEdge'">Simple Edge</xsl:when>
      <xsl:when test=".='esriFTComplexJunction'">Complex Junction</xsl:when>
      <xsl:when test=".='esriFTComplexEdge'">Complex Edge</xsl:when>
      <xsl:when test=".='esriFTAnnotation'">Annotation</xsl:when>
      <xsl:when test=".='esriFTDimension'">Dimension</xsl:when>
      <xsl:when test=".='esriFTRasterCatalogItem'">Dimension</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                                 -->
  <!--  ENUMERATOR ESRIRELCARDINALITY  -->
  <!--                                 -->
  <xsl:template match="Cardinality">
    <xsl:choose>
      <xsl:when test=".='esriRelCardinalityOneToOne'">One To One</xsl:when>
      <xsl:when test=".='esriRelCardinalityOneToMany'">One To Many</xsl:when>
      <xsl:when test=".='esriRelCardinalityManyToMany'">Many To Many</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                                  -->
  <!--  ENUMERATOR ESRIRELNOTIFICATION  -->
  <!--                                  -->
  <xsl:template match="Notification">
    <xsl:choose>
      <xsl:when test=".='esriRelNotificationNone'">None</xsl:when>
      <xsl:when test=".='esriRelNotificationForward'">Forward</xsl:when>
      <xsl:when test=".='esriRelNotificationBackward'">Backward</xsl:when>
      <xsl:when test=".='esriRelNotificationBoth'">Both</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                                            -->
  <!--  ENUMERATOR ESRINETWORKCLASSANCILLARYROLE  -->
  <!--                                            -->
  <xsl:template match="NetworkClassAncillaryRole">
    <xsl:choose>
      <xsl:when test=".='esriNCARNone'">None</xsl:when>
      <xsl:when test=".='esriNCARSourceSink'">Source/Sink</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--                                   -->
  <!--  ENUMERATOR ESRITOPOLOGYRULETYPE  -->
  <!--                                   -->
  <xsl:template match="TopologyRuleType">
    <xsl:choose>
      <xsl:when test=".='esriTRTAny'">Any Rule</xsl:when>
      <xsl:when test=".='esriTRTFeatureLargerThanClusterTolerance'">Feature larger than cluster tolerance</xsl:when>
      <xsl:when test=".='esriTRTAreaNoGaps'">Must not have gaps</xsl:when>
      <xsl:when test=".='esriTRTAreaNoOverlap'">Must not have overlaps</xsl:when>
      <xsl:when test=".='esriTRTAreaCoveredByAreaClass'">Must be covered by feature class of</xsl:when>
      <xsl:when test=".='esriTRTAreaAreaCoverEachOther'">Must cover each other</xsl:when>
      <xsl:when test=".='esriTRTAreaCoveredByArea'">Must be covered by</xsl:when>
      <xsl:when test=".='esriTRTAreaNoOverlapArea'">Must not overlap with</xsl:when>
      <xsl:when test=".='esriTRTLineCoveredByAreaBoundary'">Must be covered by boundary of</xsl:when>
      <xsl:when test=".='esriTRTPointCoveredByAreaBoundary'">Must be covered by boundary of</xsl:when>
      <xsl:when test=".='esriTRTPointProperlyInsideArea'">Must be properly inside polygons</xsl:when>
      <xsl:when test=".='esriTRTLineNoOverlap'">Must not overlap</xsl:when>
      <xsl:when test=".='esriTRTLineNoIntersection'">Must not intersect</xsl:when>
      <xsl:when test=".='esriTRTLineNoDangles'">Must not have dangles</xsl:when>
      <xsl:when test=".='esriTRTLineNoPseudos'">Must not have pseudo-nodes</xsl:when>
      <xsl:when test=".='esriTRTLineCoveredByLineClass'">Must be covered by feature class of</xsl:when>
      <xsl:when test=".='esriTRTLineNoOverlapLine'">Must not overlap with</xsl:when>
      <xsl:when test=".='esriTRTPointCoveredByLine'">Point must be covered by line</xsl:when>
      <xsl:when test=".='esriTRTPointCoveredByLineEndpoint'">Must be covered by endpoint of</xsl:when>
      <xsl:when test=".='esriTRTAreaBoundaryCoveredByLine'">Boundary must be covered by</xsl:when>
      <xsl:when test=".='esriTRTAreaBoundaryCoveredByAreaBoundary'">Area boundary must be covered by boundary of</xsl:when>
      <xsl:when test=".='esriTRTLineNoSelfOverlap'">Must not self overlap</xsl:when>
      <xsl:when test=".='esriTRTLineNoSelfIntersect'">Must not self intersect</xsl:when>
      <xsl:when test=".='esriTRTLineNoIntersectOrInteriorTouch'">Must not intersect or touch interior</xsl:when>
      <xsl:when test=".='esriTRTLineEndpointCoveredByPoint'">Endpoint must be covered by</xsl:when>
      <xsl:when test=".='esriTRTAreaContainPoint'">Contains point</xsl:when>
      <xsl:when test=".='esriTRTLineNoMultipart'">Must be single part</xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  Get ObjectClassName From ObjectClassID  -->
  <xsl:template match="WorkspaceDefinition" mode="GetObjectClassNameFromObjectClassID">
    <xsl:param name="objectClassID" />
    <xsl:choose>
      <xsl:when test="count(DatasetDefinitions//DataElement/DSID[.=$objectClassID])=1">
        <xsl:value-of select="DatasetDefinitions//DataElement/DSID[.=$objectClassID]/../Name" />
      </xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  Get SubtypeName From SubtypeCode  -->
  <xsl:template match="WorkspaceDefinition" mode="GetSubtypeNameFromSubtypeCode">
    <xsl:param name="objectClassID" />
    <xsl:param name="subtypeCode" />
    <xsl:choose>
      <xsl:when test="count(DatasetDefinitions//DataElement/DSID[.=$objectClassID]/../Subtypes)=0">
        <xsl:value-of select="DatasetDefinitions//DataElement/DSID[.=$objectClassID]/../Name" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="DatasetDefinitions//DataElement/DSID[.=$objectClassID]/../Subtypes/Subtype/SubtypeCode[.=$subtypeCode]/../SubtypeName" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  Get FeatureType From Name   -->
  <xsl:template match="WorkspaceDefinition" mode="GetFeatureTypeFromName">
    <xsl:param name="name" />
    <xsl:choose>
      <xsl:when test="count(DatasetDefinitions//DataElement/Name[.=$name]/../FeatureType)=1">
        <xsl:apply-templates select="DatasetDefinitions//DataElement/Name[.=$name]/../FeatureType" />
      </xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  Get AncillaryRole From Name   -->
  <xsl:template match="WorkspaceDefinition" mode="GetAncillaryRoleFromName">
    <xsl:param name="name" />
    <xsl:choose>
      <xsl:when test="count(DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/NetworkClassAncillaryRole)=1">
        <xsl:apply-templates select="DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/NetworkClassAncillaryRole" />
      </xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  Get TopologyWeight From Name   -->
  <xsl:template match="WorkspaceDefinition" mode="GetTopologyWeightFromName">
    <xsl:param name="name" />
    <xsl:choose>
      <xsl:when test="count(DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/Weight)=1">
        <xsl:value-of select="DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/Weight" />
      </xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  Get TopologyXYRank From Name   -->
  <xsl:template match="WorkspaceDefinition" mode="GetTopologyXYRankFromName">
    <xsl:param name="name" />
    <xsl:choose>
      <xsl:when test="count(DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/XYRank)=1">
        <xsl:value-of select="DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/XYRank" />
      </xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  Get TopologyZRank From Name   -->
  <xsl:template match="WorkspaceDefinition" mode="GetTopologyZRankFromName">
    <xsl:param name="name" />
    <xsl:choose>
      <xsl:when test="count(DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/ZRank)=1">
        <xsl:value-of select="DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/ZRank" />
      </xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  Get EventNotification From Name   -->
  <xsl:template match="WorkspaceDefinition" mode="GetTopologyEventNotificationFromName">
    <xsl:param name="name" />
    <xsl:choose>
      <xsl:when test="count(DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/EventNotificationOnValidate)=1">
        <xsl:choose>
          <xsl:when test="DatasetDefinitions//DataElement/Name[.=$name]/../ControllerMemberships/ControllerMembership/EventNotificationOnValidate='true'">Yes</xsl:when>
          <xsl:otherwise>No</xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>-</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
