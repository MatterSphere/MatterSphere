<?xml version="1.0" encoding="utf-8"?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:TimeCardPending_Srv="http://elite.com/schemas/transaction/process/write/TimeCardPending_Srv"
    xmlns:TimeCardPending="http://elite.com/schemas/transaction/object/write/TimeCardPending"
    xmlns:json="http://james.newtonking.com/projects/json"
    version="2.0">
  <xsl:strip-space elements="*" />
  <xsl:output indent="yes"/>

  <xsl:variable name="type" select="'TimeCardPending'" />

  <xsl:template match="*">
    <xsl:choose>
      <xsl:when test="local-name()='Phase' or local-name()='Task' or local-name()='Activity'">
        <xsl:element name="{local-name()}">
          <xsl:element name="aliasValue">
            <xsl:value-of select="node()"/>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="{local-name()}">
          <xsl:element name="Value">
            <xsl:apply-templates select="node()"/>
          </xsl:element>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <xsl:element name="DataCollection">
      <xsl:element name="Rows">
        <xsl:attribute name="json:Array">true</xsl:attribute>
        <xsl:apply-templates select="TimeCardPending_Srv:TimeCardPending_Srv/TimeCardPending:Initialize/TimeCardPending:Add/TimeCardPending:TimeCardPending/TimeCardPending:Attributes" />
        <xsl:element name="SubclassId">
          <xsl:value-of select="$type" />
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="TimeCardPending:Attributes">
    <xsl:element name="Attributes">
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>
</xsl:transform>
