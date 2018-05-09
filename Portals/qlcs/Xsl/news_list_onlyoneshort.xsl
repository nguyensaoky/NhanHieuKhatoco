<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <table width="100%" cellpadding="1" cellspacing="0">
      <tr>
        <td></td>
      </tr>
      <xsl:apply-templates select="/newslist/news"></xsl:apply-templates>
    </table>
  </xsl:template>

  <xsl:template match="news">
    <xsl:if test="first='true'">
      <tr>
        <td style="text-align:left;padding-top:10px;" valign="top">
          <a>
            <img border="0">
              <xsl:attribute name="src">
                <xsl:value-of select="ImageUrl"></xsl:value-of>
              </xsl:attribute>
              <xsl:attribute name="width">
                <xsl:value-of select="ImageWidth"></xsl:value-of>
              </xsl:attribute>
            </img>
          </a>
        </td>
      </tr>
      <tr>
        <td valign="top" id="NewsHeadline">
          <br/>
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="link"></xsl:value-of>
            </xsl:attribute>
            <b>
              <xsl:value-of select="Headline"></xsl:value-of>
            </b>
          </a>
          <div class="NewsDescription">
            <xsl:attribute name="style">
              <xsl:text>width:</xsl:text>
              <xsl:value-of select="ImageWidth"></xsl:value-of>
              <xsl:text>;</xsl:text>
              <xsl:text>text-align:justify;</xsl:text>
            </xsl:attribute>
            <xsl:value-of select="Description" disable-output-escaping="yes"></xsl:value-of>
          </div>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>

