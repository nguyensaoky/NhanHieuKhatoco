<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <table width="100%" cellpadding="4" cellspacing="0">
      <tr>
        <td valign="top" width="50%">
          <table>
            <tr>
              <td></td>
            </tr>
            <xsl:apply-templates select="/newslist/firstnews"></xsl:apply-templates>
          </table>
        </td>
        <td valign="top">
          <table>
            <tr>
              <td></td>
            </tr>
            <xsl:apply-templates select="/newslist/news"></xsl:apply-templates>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="firstnews">
    <tr>
      <td style="text-align:left;padding-top:10px;" valign="top" width="10%">
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
      <td valign="top" align="left">
        <table width="100%">
          <tr>
            <td class="NewsHeadline">
              <a>
                <xsl:attribute name="href">
                  <xsl:value-of select="link"></xsl:value-of>
                </xsl:attribute>
                <b>
                  <xsl:value-of select="Headline"></xsl:value-of>
                </b>
              </a>
            </td>
          </tr>
          <tr>
            <td class="NewsDescription">
              <xsl:value-of select="Description" disable-output-escaping="yes"></xsl:value-of>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="news">
    <tr>
      <td valign="top" colspan="2" height="8px">
        <table width="100%" colpadding="0" colspacing="0">
          <tr>
            <td class="NewsHeadline">
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

