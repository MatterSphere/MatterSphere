<?xml version="1.0" encoding="utf-8"?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:NxBizTalkEntityOrgLoad="http://elite.com/schemas/transaction/process/write/NxBizTalkEntityOrgLoad"
    xmlns:EntityOrg="http://elite.com/schemas/transaction/object/write/EntityOrg"
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
        <xsl:apply-templates select="NxBizTalkEntityOrgLoad:NxBizTalkEntityOrgLoad/EntityOrg:Initialize/EntityOrg:Add/EntityOrg:EntityOrg/EntityOrg:Attributes" />
        <xsl:apply-templates select="NxBizTalkEntityOrgLoad:NxBizTalkEntityOrgLoad/EntityOrg:Initialize/EntityOrg:Edit/EntityOrg:EntityOrg/EntityOrg:Attributes" />
        <xsl:apply-templates select="NxBizTalkEntityOrgLoad:NxBizTalkEntityOrgLoad/EntityOrg:Initialize/EntityOrg:Add/EntityOrg:EntityOrg/EntityOrg:Children" />
        <xsl:apply-templates select="NxBizTalkEntityOrgLoad:NxBizTalkEntityOrgLoad/EntityOrg:Initialize/EntityOrg:Edit/EntityOrg:EntityOrg/EntityOrg:Children" />
        <xsl:element name="SubclassId">
          <xsl:value-of select="$type" />
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="EntityOrg:Attributes">
    <xsl:choose>
      <xsl:when test="$root = 'Edit' and name(..) = 'EntityOrg'">
        <xsl:element name="Id">
          <xsl:text>%ENTITYID%</xsl:text>
        </xsl:element>
      </xsl:when>
    </xsl:choose>
    <xsl:element name="Attributes">
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="EntityOrg:Children">
    <xsl:apply-templates select="EntityOrg:Relate/EntityOrg:Edit/EntityOrg:Relate" />
    <xsl:apply-templates select="EntityOrg:Site/EntityOrg:Add/EntityOrg:Site" />
    <xsl:apply-templates select="EntityOrg:Site/EntityOrg:Edit/EntityOrg:Site" />
    <xsl:apply-templates select="EntityOrg:Site_Phone/EntityOrg:Add/EntityOrg:Site_Phone" />
    <xsl:apply-templates select="EntityOrg:Site_Phone/EntityOrg:Edit/EntityOrg:Site_Phone" />
    <xsl:apply-templates select="EntityOrg:Site_URL/EntityOrg:Add/EntityOrg:Site_URL" />
    <xsl:apply-templates select="EntityOrg:Site_URL/EntityOrg:Edit/EntityOrg:Site_URL" />
    <xsl:apply-templates select="EntityOrg:Site_EMail/EntityOrg:Add/EntityOrg:Site_EMail" />
    <xsl:apply-templates select="EntityOrg:Site_EMail/EntityOrg:Edit/EntityOrg:Site_EMail" />
  </xsl:template>

  <xsl:template match="EntityOrg:Relate | EntityOrg:Site | EntityOrg:Site_Phone | EntityOrg:Site_URL | EntityOrg:Site_EMail">
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
          <xsl:value-of select="name(.)"/>
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>
</xsl:transform>
