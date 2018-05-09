<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <xsl:apply-templates select="/newsgroups/newsgroup"></xsl:apply-templates>
  </xsl:template>

  <xsl:template match="newsgroup">
    <tr>
      <xsl:if test="odd='true'">
        <xsl:attribute name="style">background:#F0F6FC</xsl:attribute>
      </xsl:if>
      <td style="text-align:center;">
        <xsl:value-of select="OrderNumber"></xsl:value-of>
      </td>
      <td>
          <xsl:value-of select="NewsGroupName"></xsl:value-of>
      </td>
      <td style="text-align:center;">
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="url_edit"></xsl:value-of>
          </xsl:attribute>
          <img border="0">
            <xsl:attribute name="src">
              <xsl:value-of select="ImgLevel2"></xsl:value-of>
            </xsl:attribute>
          </img>
        </a>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>

