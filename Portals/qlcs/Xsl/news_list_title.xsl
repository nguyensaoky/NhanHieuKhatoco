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
    <tr>
      <td valign="top">
        <table width="100%">
          <tr>
            <td class="NewsHeadline1">
              <img style="vertical-align:middle;">
                <xsl:attribute name="src">
                  <xsl:value-of select="Bullet"></xsl:value-of>
                </xsl:attribute>
              </img>
              <xsl:text> </xsl:text>
              <a>
                <xsl:attribute name="href">
                  <xsl:value-of select="link"></xsl:value-of>
                </xsl:attribute>
                <xsl:value-of select="Headline"></xsl:value-of>
              </a>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>

