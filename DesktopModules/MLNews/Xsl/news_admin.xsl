<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
      <xsl:apply-templates select="/newslist/news"></xsl:apply-templates>
  </xsl:template>

  <xsl:template match="news">
    <tr>
      <td>
        <xsl:value-of select="CatName"></xsl:value-of>
      </td>
      <td>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="link"></xsl:value-of>
          </xsl:attribute>
          <xsl:value-of select="Headline"></xsl:value-of>
        </a>
      </td>
      <td>
        <img border="0">
          <xsl:attribute name="src">
            <xsl:value-of select="New"></xsl:value-of>
          </xsl:attribute>
        </img>
      </td>
      <td>
        <img border="0">
          <xsl:attribute name="src">
            <xsl:value-of select="Hot"></xsl:value-of>
          </xsl:attribute>
        </img>
      </td>
      <td>
        <xsl:value-of select="ModifyDate"></xsl:value-of>
      </td>
      <td style="text-align:center;">
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="url_edit"></xsl:value-of>
          </xsl:attribute>
          <img border="0">
            <xsl:attribute name="src">
              <xsl:value-of select="ImgEdit"></xsl:value-of>
            </xsl:attribute>
          </img>
        </a>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>

