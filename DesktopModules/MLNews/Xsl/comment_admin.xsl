<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <xsl:apply-templates select="/commentlist/comment"></xsl:apply-templates>
  </xsl:template>
  <xsl:template match="comment">
    <tr>
      <td>
        <xsl:value-of select="Author"></xsl:value-of> (<xsl:value-of select="AuthorEmail"></xsl:value-of>)
      </td>
      <td>
        <b>
        <xsl:value-of select="Headline"></xsl:value-of>
        </b>
        <br/>
        <a style="text-decoration:none;">
          <xsl:attribute name="title">
            <xsl:value-of select="CatName"></xsl:value-of> / <xsl:value-of select="NewsHeadline"></xsl:value-of>
          </xsl:attribute>
          <xsl:value-of select="Content"></xsl:value-of>
        </a>
      </td>
      <td>
        <xsl:value-of select="ClientIPAddress"></xsl:value-of>
      </td>
      <td>
        <img border="0">
          <xsl:attribute name="src">
            <xsl:value-of select="ImgStatus"></xsl:value-of>
          </xsl:attribute>
        </img>
      </td>
      <td style="text-align:center;">
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="url_delete"></xsl:value-of>
          </xsl:attribute>
          <img border="0">
            <xsl:attribute name="src">
              <xsl:value-of select="ImgDelete"></xsl:value-of>
            </xsl:attribute>
          </img>
        </a>
      </td>
      <td style="text-align:center;">
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="url_changestatus"></xsl:value-of>
          </xsl:attribute>
          <img border="0">
            <xsl:attribute name="src">
              <xsl:value-of select="ImgChangeStatus"></xsl:value-of>
            </xsl:attribute>
          </img>
        </a>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>

