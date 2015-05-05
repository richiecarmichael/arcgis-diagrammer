<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
  <xsl:param name="title" select="title" />
  <xsl:param name="environment" select="environment" />
  <xsl:param name="os" select="os" />
  <xsl:param name="osvalue" select="osvalue" />
  <xsl:param name="clr" select="clr" />
  <xsl:param name="clrvalue" select="clrvalue" />
  <xsl:param name="exceptions" select="exceptions" />
  <xsl:param name="date" select="date" />
  <xsl:param name="assembly" select="assembly" />
  <xsl:param name="message" select="message" />
  <xsl:param name="stack" select="stack" />
  <xsl:template match="/">
    <html xmlns='http://www.w3.org/1999/xhtml'>
      <head>
        <title>
          <xsl:value-of select="$title" />
        </title>
      </head>
      <body style='background-color: #cccccc; font-family: Verdana; font-size: x-small;'>
        <strong>
          <xsl:value-of select="$environment" />
        </strong>
        <br />
        <table border='1' cellspacing='0' style='font-family: Verdana; font-size: x-small;'>
          <tr>
            <td>
              <xsl:value-of select="$os" />
            </td>
            <td>
              <xsl:value-of select="$osvalue" />
            </td>
          </tr>
          <tr>
            <td>
              <xsl:value-of select="$clr" />
            </td>
            <td>
              <xsl:value-of select="$clrvalue" />
            </td>
          </tr>
        </table>
        <br />
        <xsl:if test="count(ROOT/EXCEPTION)>0">
          <strong>
            <xsl:value-of select="$exceptions" />
          </strong>
          <br />
          <xsl:for-each select="ROOT/EXCEPTION">
            <table border='1' cellspacing='0' style='font-family: Verdana; font-size: x-small;'>
              <tr>
                <td>
                  <xsl:value-of select="$date" />
                </td>
                <td>
                  <xsl:value-of select="DATE" />
                </td>
              </tr>
              <tr>
                <td>
                  <xsl:value-of select="$assembly" />
                </td>
                <td>
                  <xsl:value-of select="ASSEMBLY" />
                </td>
              </tr>
              <tr>
                <td>
                  <xsl:value-of select="$message" />
                </td>
                <td>
                  <xsl:value-of select="MESSAGE" />
                </td>
              </tr>
              <tr>
                <td>
                  <xsl:value-of select="$stack" />
                </td>
                <td>
                  <xsl:value-of select="STACK" />
                </td>
              </tr>
            </table>
            <br />
          </xsl:for-each>
        </xsl:if>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>