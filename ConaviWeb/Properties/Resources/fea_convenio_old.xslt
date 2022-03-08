<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:cfd="http://www.sat.gob.mx/cfd/2" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:fn="http://schemas.microsoft.com/VisualStudio/2005/xpath-functions">

<!-- Con el siguiente método se establece que la salida deberá ser en texto -->

  <xsl:output version="1.0" indent="no" encoding="Utf-8" method="text"/>

  <xsl:include href="http://www.sat.gob.mx/sitio_internet/cfd/2/cadenaoriginal_2_0/utilerias.xslt"/>
  
  <xsl:template match="FirmaElectronica">
    |<xsl:apply-templates select="DatosCadenaOriginal"/>||
  </xsl:template>

  <xsl:template match="DatosCadenaOriginal">

<!-- Iniciamos el tratamiento de los atributos del Emisor -->

  <!--Actividad en el proceso de solicitud de mesa de servicios: solicitud, Vo.Bo. o autorización -->     
    
    
  <xsl:call-template name="Requerido">

    <xsl:with-param select="movimiento" name="valor"/>

  </xsl:call-template>
    
  <xsl:call-template name="Requerido">

    <xsl:with-param select="tema" name="valor"/>

  </xsl:call-template>

  <!-- Folio de solicitud-->
  <xsl:call-template name="Requerido">

    <xsl:with-param select="folio" name="valor"/>

  </xsl:call-template>

    <!-- Folio de solicitud-->
    <xsl:call-template name="Requerido">

      <xsl:with-param select="curpbenef" name="valor"/>

    </xsl:call-template>

    <!-- Folio de solicitud-->
    <xsl:call-template name="Requerido">

      <xsl:with-param select="nombrebenef" name="valor"/>

    </xsl:call-template>
    

  <!-- Actividad general "Solicitud de movimientos para acceso a sistemas"-->
  

  

  <!-- Movimiento específico: alta, reasginación o baja
  <xsl:call-template name="Requerido">

    <xsl:with-param select="usuTipMod" name="valor"/>

  </xsl:call-template>
 -->
  <!-- Sistema objeto 
  <xsl:call-template name="Requerido">

    <xsl:with-param select="areaClasfNombre" name="valor"/>

  </xsl:call-template>
-->
  <!-- Perfil solicitado 
  <xsl:call-template name="Requerido">

    <xsl:with-param select="perfilNombre" name="valor"/>

  </xsl:call-template>
  -->
  <!-- Datos generales persona quien firma el movimiento -->

  
    
  <!-- Persona quien firma el Movimiento (usuario.solicitante)-->
  <xsl:call-template name="Requerido">

    <xsl:with-param select="nombreFirmante" name="valor"/>

  </xsl:call-template>
    <!-- Puesto firmante -->
    <xsl:call-template name="Requerido">

      <xsl:with-param select="puestoFirmante" name="valor"/>

    </xsl:call-template>
    
  <!-- RFC quien firma el Movimiento (usuario.solicitante)-->
  <xsl:call-template name="Requerido">

    <xsl:with-param select="RFCFirmante" name="valor"/>

  </xsl:call-template>

    <!-- No. de empleado firmante -->
    <xsl:call-template name="Requerido">

      <xsl:with-param select="numEmpleadoFirmante" name="valor"/>

    </xsl:call-template>
    <!-- Unidad Administrativa -->
  <xsl:call-template name="Requerido">

    <xsl:with-param select="areaFirmante" name="valor"/>

  </xsl:call-template>


    
  <!-- Usuario en Mesa de Servicios firmante (usuario.solicitante)
  <xsl:call-template name="Requerido">

    <xsl:with-param select="usuarioFirmante" name="valor"/>

  </xsl:call-template>
 -->
  <!-- Datos generales firma de documento-->

  
    

  <!-- Fecha y hora en que se recibe respuesta de validación de certificado-->
    <xsl:call-template name="Requerido">

    <xsl:with-param select="timeStampOCSP" name="valor"/>

  </xsl:call-template>

  <!-- Fecha y hora en que se firma documento-->
    <!-- <xsl:call-template name="Requerido">-->

         <!--  <xsl:with-param select="timeStampSign" name="valor"/>-->
   
     <!-- </xsl:call-template>-->
    <!-- Archivo HASH-->
    <xsl:call-template name="Requerido">

      <xsl:with-param select="hashArchivo" name="valor"/>

    </xsl:call-template>

  <!-- Número de certificado con que se valida la firma-->
  <xsl:call-template name="Requerido">

    <xsl:with-param select="certificateNumber" name="valor"/>

  </xsl:call-template>

    <!-- Sello del solicitante -->
    <xsl:call-template name="Opcional">

      <xsl:with-param select="sello" name="valor"/>

    </xsl:call-template>


  </xsl:template>

</xsl:stylesheet>