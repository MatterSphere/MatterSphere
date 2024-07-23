<?xml version="1.0" encoding="utf-8"?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:Matter_Srv="http://elite.com/schemas/transaction/process/write/Matter_Srv"
    xmlns:Matter="http://elite.com/schemas/transaction/object/write/Matter"
    xmlns:json="http://james.newtonking.com/projects/json"
    version="2.0">
  <xsl:strip-space elements="*" />
  <xsl:output indent="yes"/>

  <xsl:variable name="type" select="'Matter'" />
  <xsl:variable name="root" select="name(/*/*/*)" />

  <xsl:template match="*">
    <xsl:element name="{local-name()}">
      <xsl:element name="Value">
        <xsl:apply-templates select="@* | node()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/">
    <xsl:element name="DataCollection">
      <xsl:element name="Rows">
        <xsl:attribute name="json:Array">true</xsl:attribute>
        <xsl:apply-templates select="Matter_Srv:Matter_Srv/Matter:Initialize/Matter:Add/Matter:Matter/Matter:Attributes" />
        <xsl:apply-templates select="Matter_Srv:Matter_Srv/Matter:Initialize/Matter:Edit/Matter:Matter/Matter:Attributes" />
        <xsl:apply-templates select="Matter_Srv:Matter_Srv/Matter:Initialize/Matter:Add/Matter:Matter/Matter:Children" />
        <xsl:apply-templates select="Matter_Srv:Matter_Srv/Matter:Initialize/Matter:Edit/Matter:Matter/Matter:Children" />
        <xsl:element name="SubclassId">
          <xsl:value-of select="$type" />
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Matter:Attributes">
    <xsl:choose>
      <xsl:when test="$root = 'Edit' and name(..) = 'Matter'">
        <xsl:element name="Id">
          <xsl:value-of select="../@KeyValue" />
        </xsl:element>
      </xsl:when>
    </xsl:choose>
    <xsl:element name="Attributes">
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="Matter:Children">
    <xsl:apply-templates select="Matter:MattDate/Matter:Add/Matter:MattDate" />
    <xsl:apply-templates select="Matter:MattDate/Matter:Edit/Matter:MattDate" />
    <xsl:apply-templates select="Matter:MattRate/Matter:Edit/Matter:MattRate" />
  </xsl:template>

  <xsl:template match="Matter:MattDate | Matter:MattRate">
    <xsl:element name="ChildObjects">
      <xsl:attribute name="json:Array">true</xsl:attribute>
      <xsl:element name="ObjectId">
        <xsl:value-of select ="name(.)"/>
      </xsl:element>
      <xsl:element name="Rows">
        <xsl:attribute name="json:Array">true</xsl:attribute>
        <xsl:choose>
          <xsl:when test="$root = 'Edit' and name(..) = 'Edit'">
            <xsl:choose>
              <xsl:when test="name(.) = 'MattDate'">
                <xsl:element name="Id">
                  <xsl:text>%MATTDATEID%</xsl:text>
                </xsl:element>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$root = 'Edit' and name(..) = 'Add'">
            <xsl:element name="DataState">
              <xsl:text>Added</xsl:text>
            </xsl:element>
          </xsl:when>
        </xsl:choose>
        <xsl:apply-templates/>
        <xsl:element name="SubclassId">
          <xsl:value-of select ="name(.)"/>
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>
</xsl:transform>
