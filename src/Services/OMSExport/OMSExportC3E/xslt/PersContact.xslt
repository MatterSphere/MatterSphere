<?xml version="1.0" encoding="utf-8"?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:EntityPerson_Srv="http://elite.com/schemas/transaction/process/write/EntityPerson_Srv"
    xmlns:EntityPerson="http://elite.com/schemas/transaction/object/write/EntityPerson"
    xmlns:json="http://james.newtonking.com/projects/json"
    version="2.0">
  <xsl:strip-space elements="*" />
  <xsl:output indent="yes"/>

  <xsl:variable name="type" select="'Entity'" />
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
        <xsl:apply-templates select="EntityPerson_Srv:EntityPerson_Srv/EntityPerson:Initialize/EntityPerson:Add/EntityPerson:EntityPerson/EntityPerson:Attributes" />
        <xsl:apply-templates select="EntityPerson_Srv:EntityPerson_Srv/EntityPerson:Initialize/EntityPerson:Edit/EntityPerson:EntityPerson/EntityPerson:Attributes" />
        <xsl:apply-templates select="EntityPerson_Srv:EntityPerson_Srv/EntityPerson:Initialize/EntityPerson:Add/EntityPerson:EntityPerson/EntityPerson:Children" />
        <xsl:apply-templates select="EntityPerson_Srv:EntityPerson_Srv/EntityPerson:Initialize/EntityPerson:Edit/EntityPerson:EntityPerson/EntityPerson:Children" />
        <xsl:element name="SubclassId">
          <xsl:value-of select="$type" />
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="EntityPerson:Attributes">
    <xsl:choose>
      <xsl:when test="$root = 'Edit' and name(..) = 'EntityPerson'">
        <xsl:element name="Id">
          <xsl:text>%ENTITYID%</xsl:text>
        </xsl:element>
      </xsl:when>
    </xsl:choose>
    <xsl:element name="Attributes">
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="EntityPerson:Children">
    <xsl:apply-templates select="EntityPerson:Relate/EntityPerson:Edit/EntityPerson:Relate" />
    <xsl:apply-templates select="EntityPerson:Site/EntityPerson:Add/EntityPerson:Site" />
    <xsl:apply-templates select="EntityPerson:Site/EntityPerson:Edit/EntityPerson:Site" />
    <xsl:apply-templates select="EntityPerson:Site_Phone/EntityPerson:Add/EntityPerson:Site_Phone" />
    <xsl:apply-templates select="EntityPerson:Site_Phone/EntityPerson:Edit/EntityPerson:Site_Phone" />
    <xsl:apply-templates select="EntityPerson:Site_URL/EntityPerson:Add/EntityPerson:Site_URL" />
    <xsl:apply-templates select="EntityPerson:Site_URL/EntityPerson:Edit/EntityPerson:Site_URL" />
    <xsl:apply-templates select="EntityPerson:Site_EMail/EntityPerson:Add/EntityPerson:Site_EMail" />
    <xsl:apply-templates select="EntityPerson:Site_EMail/EntityPerson:Edit/EntityPerson:Site_EMail" />
  </xsl:template>

  <xsl:template match="EntityPerson:Relate | EntityPerson:Site | EntityPerson:Site_Phone | EntityPerson:Site_URL | EntityPerson:Site_EMail">
    <xsl:element name="ChildObjects">
      <xsl:attribute name="json:Array">true</xsl:attribute>
      <xsl:element name="ObjectId">
        <xsl:value-of select="name(.)"/>
      </xsl:element>
      <xsl:element name="Rows">
        <xsl:attribute name="json:Array">true</xsl:attribute>
        <xsl:choose>
          <xsl:when test="$root = 'Edit'">
            <xsl:choose>
              <xsl:when test="name(.) = 'Relate'">
                <xsl:element name="Id">
                  <xsl:text>%RELATEID%</xsl:text>
                </xsl:element>
              </xsl:when>
              <xsl:when test="name(.) = 'Site'">
                <xsl:element name="Id">
                  <xsl:text>%SITEID%</xsl:text>
                </xsl:element>
              </xsl:when>
            </xsl:choose>
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
