<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <table width="100%" cellpadding="4" cellspacing="0">
      <tr>
        <td></td>
      </tr>
      <xsl:apply-templates select="/commentlist/comment"></xsl:apply-templates>
    </table>
  </xsl:template>

  <xsl:template match="comment">
    <tr>
      <td style="text-align:left;" valign="top" class="Normal">
        <b><xsl:value-of select="Author"></xsl:value-of> (<xsl:value-of select="AuthorEmail"></xsl:value-of>)</b>
      </td>
      <td valign="top" align="right" class="Normal">
        <xsl:value-of select="CreatedDate"></xsl:value-of>
      </td>
    </tr>
    <tr>
      <td colspan="2" class="Normal">
        <xsl:value-of select="Headline"></xsl:value-of>
      </td>
    </tr>
    <tr>
      <td colspan="2" class="Normal">
        <xsl:value-of select="Content"></xsl:value-of>
      </td>
    </tr>
    <tr>
      <td></td>
    </tr>
  </xsl:template>
</xsl:stylesheet>