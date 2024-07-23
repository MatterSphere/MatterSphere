<?xml version="1.0" encoding="utf-8"?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:transerv="http://elite.com/schemas/transactionservice"
    xmlns:ns0="http://elite.com/schemas/transaction/process/write/CftWFNewBizIntake"
    xmlns:ns1="http://elite.com/schemas/appobjectbase"
    xmlns:NBI="http://elite.com/schemas/transaction/object/write/CftNewBizRequest"
    xmlns:json="http://james.newtonking.com/projects/json"
    version="2.0">
  <xsl:strip-space elements="*" />
  <xsl:output indent="yes"/>

  <xsl:variable name="type" select="'CftNBISearch'" />
  <xsl:variable name="root" select="name(/*/*/*)" />

  <xsl:template match="*">
    <xsl:choose>
      <xsl:when test="local-name()='CftSearchReason'">
        <xsl:element name="SearchReason">
          <xsl:element name="Value">
            <xsl:value-of select="node()"/>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="{local-name()}">
          <xsl:element name="Value">
            <xsl:choose>
              <xsl:when test="local-name() = 'Submitter'">
                <xsl:value-of select="node()"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates select="node()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:element>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <xsl:element name="DataCollection">
      <xsl:element name="Rows">
        <xsl:attribute name="json:Array">true</xsl:attribute>
        <xsl:apply-templates select="ns0:CftWFNewBizIntake/NBI:Initialize/NBI:Add/NBI:CftNewBizRequest/NBI:Attributes" />
        <xsl:apply-templates select="ns0:CftWFNewBizIntake/NBI:Initialize/NBI:Edit/NBI:CftNewBizRequest/NBI:Attributes" />
        <xsl:apply-templates select="ns0:CftWFNewBizIntake/NBI:Initialize/NBI:Add/NBI:CftNewBizRequest/NBI:Children" />
        <xsl:element name="SubclassId">
          <xsl:value-of select="$type" />
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="NBI:Attributes">
    <xsl:element name="Attributes">
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="NBI:Children">
    <xsl:element name="ChildObjects">
      <xsl:attribute name="json:Array">true</xsl:attribute>
      <xsl:element name="ObjectId">
        <xsl:value-of select="local-name(child::*)"/>
      </xsl:element>
      <xsl:apply-templates select="NBI:CftNewBizSearchName/NBI:Edit/NBI:CftNewBizSearchName" />
      <xsl:apply-templates select="NBI:CftNewBizSearchName/NBI:Add/NBI:CftNewBizSearchName" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="NBI:CftNewBizSearchName">
    <xsl:element name="Rows">
      <xsl:attribute name="json:Array">true</xsl:attribute>
      <xsl:apply-templates/>
      <xsl:element name="SubclassId">
        <xsl:value-of select="local-name(.)"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>
</xsl:transform>
