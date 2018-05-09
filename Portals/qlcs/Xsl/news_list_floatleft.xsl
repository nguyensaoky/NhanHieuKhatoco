<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <table width="100%" cellpadding="4" cellspacing="0">
      <tr>
        <td></td>
      </tr>
      <xsl:apply-templates select="/newslist/news"></xsl:apply-templates>
    </table>
  </xsl:template>

  <xsl:template match="news">
    <tr>
      <td style="height:20px;"></td>
    </tr>
    <tr>
      <td>
	      <div>
          <a>
            <img style="float:left;margin-right:5px;">
              <xsl:attribute name="src">
                <xsl:value-of select="ImageUrl"></xsl:value-of>    
              </xsl:attribute>
              <xsl:attribute name="width">
                <xsl:value-of select="ImageWidth"></xsl:value-of>
              </xsl:attribute>
            </img>
          </a>
          <a class="NewsHeadline" style="position:relative;top:-3px;">
            <xsl:attribute name="href">
              <xsl:value-of select="link"></xsl:value-of>
            </xsl:attribute>
            <xsl:value-of select="Headline"></xsl:value-of>
          </a>
          <div class="NewsDescription">
            <xsl:value-of select="Description" disable-output-escaping="yes"></xsl:value-of>
          </div>
        </div>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>