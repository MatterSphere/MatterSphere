<?xml version="1.0" encoding="utf-8"?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:CostCardPending_Srv="http://elite.com/schemas/transaction/process/write/CostCardPending_Srv"
    xmlns:CostCardPending="http://elite.com/schemas/transaction/object/write/CostCardPending"
    xmlns:json="http://james.newtonking.com/projects/json"
    version="2.0">
  <xsl:strip-space elements="*" />
  <xsl:output indent="yes"/>

  <xsl:variable name="type" select="'CostCardPending'" />

  <xsl:template match="*">
    <xsl:element name="{local-name()}">
      <xsl:element name="Value">
        <xsl:apply-templates select="node()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/">
    <xsl:element name="DataCollection">
      <xsl:element name="Rows">
        <xsl:attribute name="json:Array">true</xsl:attribute>
        <xsl:apply-templates select="CostCardPending_Srv:CostCardPending_Srv/CostCardPending:Initialize/CostCardPending:Add/CostCardPending:CostCardPending/CostCardPending:Attributes" />
        <xsl:element name="SubclassId">
          <xsl:value-of select="$type" />
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="CostCardPending:Attributes">
    <xsl:element name="Attributes">
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>
</xsl:transform>
