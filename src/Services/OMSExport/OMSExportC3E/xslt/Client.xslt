<?xml version="1.0" encoding="utf-8"?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:Client_Srv="http://elite.com/schemas/transaction/process/write/Client_Srv"
    xmlns:Client="http://elite.com/schemas/transaction/object/write/Client"
    xmlns:json="http://james.newtonking.com/projects/json"
    version="2.0">
  <xsl:strip-space elements="*" />
  <xsl:output indent="yes"/>

  <xsl:variable name="type" select="'Client'" />
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
        <xsl:apply-templates select="Client_Srv:Client_Srv/Client:Initialize/Client:Add/Client:Client/Client:Attributes" />
        <xsl:apply-templates select="Client_Srv:Client_Srv/Client:Initialize/Client:Edit/Client:Client/Client:Attributes" />
        <xsl:apply-templates select="Client_Srv:Client_Srv/Client:Initialize/Client:Add/Client:Client/Client:Children" />
        <xsl:apply-templates select="Client_Srv:Client_Srv/Client:Initialize/Client:Edit/Client:Client/Client:Children" />
        <xsl:element name="SubclassId">
          <xsl:value-of select="$type" />
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Client:Attributes">
    <xsl:choose>
      <xsl:when test="$root = 'Edit' and name(..) = 'Client'">
        <xsl:element name="Id">
          <xsl:value-of select="../@KeyValue" />
        </xsl:element>
      </xsl:when>
    </xsl:choose>
    <xsl:element name="Attributes">
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="Client:Children">
    <xsl:apply-templates select="Client:CliDate/Client:Add/Client:CliDate" />
    <xsl:apply-templates select="Client:CliDate/Client:Edit/Client:CliDate" />
  </xsl:template>

  <xsl:template match="Client:CliDate">
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
              <xsl:when test="name(.) = 'CliDate'">
                <xsl:element name="Id">
                  <xsl:text>%CLIDATEID%</xsl:text>
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
