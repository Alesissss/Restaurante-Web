﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.42000
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System.Data

Namespace srMesero
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="srMesero.wsMeseroSoap")>  _
    Public Interface wsMeseroSoap
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/generarIDMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function generarIDMesero() As Integer
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/generarIDMesero", ReplyAction:="*")>  _
        Function generarIDMeseroAsync() As System.Threading.Tasks.Task(Of Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/guardarMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Sub guardarMesero(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal sex As String, ByVal fec As Date, ByVal dir As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/guardarMesero", ReplyAction:="*")>  _
        Function guardarMeseroAsync(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal sex As String, ByVal fec As Date, ByVal dir As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean) As System.Threading.Tasks.Task
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/modificarMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Sub modificarMesero(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal sex As String, ByVal fec As Date, ByVal dir As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/modificarMesero", ReplyAction:="*")>  _
        Function modificarMeseroAsync(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal sex As String, ByVal fec As Date, ByVal dir As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean) As System.Threading.Tasks.Task
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/eliminarMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Sub eliminarMesero(ByVal id As Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/eliminarMesero", ReplyAction:="*")>  _
        Function eliminarMeseroAsync(ByVal id As Integer) As System.Threading.Tasks.Task
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/darBajaMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Sub darBajaMesero(ByVal id As Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/darBajaMesero", ReplyAction:="*")>  _
        Function darBajaMeseroAsync(ByVal id As Integer) As System.Threading.Tasks.Task
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/darAltaMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Sub darAltaMesero(ByVal id As Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/darAltaMesero", ReplyAction:="*")>  _
        Function darAltaMeseroAsync(ByVal id As Integer) As System.Threading.Tasks.Task
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/buscarMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function buscarMesero(ByVal id As Integer) As System.Data.DataTable
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/buscarMesero", ReplyAction:="*")>  _
        Function buscarMeseroAsync(ByVal id As Integer) As System.Threading.Tasks.Task(Of System.Data.DataTable)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/listarMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function listarMesero() As System.Data.DataTable
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/listarMesero", ReplyAction:="*")>  _
        Function listarMeseroAsync() As System.Threading.Tasks.Task(Of System.Data.DataTable)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/VerificarMesero", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function VerificarMesero(ByVal idMesero As Integer) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/VerificarMesero", ReplyAction:="*")>  _
        Function VerificarMeseroAsync(ByVal idMesero As Integer) As System.Threading.Tasks.Task(Of Boolean)
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface wsMeseroSoapChannel
        Inherits srMesero.wsMeseroSoap, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class wsMeseroSoapClient
        Inherits System.ServiceModel.ClientBase(Of srMesero.wsMeseroSoap)
        Implements srMesero.wsMeseroSoap
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function generarIDMesero() As Integer Implements srMesero.wsMeseroSoap.generarIDMesero
            Return MyBase.Channel.generarIDMesero
        End Function
        
        Public Function generarIDMeseroAsync() As System.Threading.Tasks.Task(Of Integer) Implements srMesero.wsMeseroSoap.generarIDMeseroAsync
            Return MyBase.Channel.generarIDMeseroAsync
        End Function
        
        Public Sub guardarMesero(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal sex As String, ByVal fec As Date, ByVal dir As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean) Implements srMesero.wsMeseroSoap.guardarMesero
            MyBase.Channel.guardarMesero(id, dni, nom, ape, sex, fec, dir, tel, cor, est)
        End Sub
        
        Public Function guardarMeseroAsync(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal sex As String, ByVal fec As Date, ByVal dir As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean) As System.Threading.Tasks.Task Implements srMesero.wsMeseroSoap.guardarMeseroAsync
            Return MyBase.Channel.guardarMeseroAsync(id, dni, nom, ape, sex, fec, dir, tel, cor, est)
        End Function
        
        Public Sub modificarMesero(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal sex As String, ByVal fec As Date, ByVal dir As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean) Implements srMesero.wsMeseroSoap.modificarMesero
            MyBase.Channel.modificarMesero(id, dni, nom, ape, sex, fec, dir, tel, cor, est)
        End Sub
        
        Public Function modificarMeseroAsync(ByVal id As Integer, ByVal dni As String, ByVal nom As String, ByVal ape As String, ByVal sex As String, ByVal fec As Date, ByVal dir As String, ByVal tel As String, ByVal cor As String, ByVal est As Boolean) As System.Threading.Tasks.Task Implements srMesero.wsMeseroSoap.modificarMeseroAsync
            Return MyBase.Channel.modificarMeseroAsync(id, dni, nom, ape, sex, fec, dir, tel, cor, est)
        End Function
        
        Public Sub eliminarMesero(ByVal id As Integer) Implements srMesero.wsMeseroSoap.eliminarMesero
            MyBase.Channel.eliminarMesero(id)
        End Sub
        
        Public Function eliminarMeseroAsync(ByVal id As Integer) As System.Threading.Tasks.Task Implements srMesero.wsMeseroSoap.eliminarMeseroAsync
            Return MyBase.Channel.eliminarMeseroAsync(id)
        End Function
        
        Public Sub darBajaMesero(ByVal id As Integer) Implements srMesero.wsMeseroSoap.darBajaMesero
            MyBase.Channel.darBajaMesero(id)
        End Sub
        
        Public Function darBajaMeseroAsync(ByVal id As Integer) As System.Threading.Tasks.Task Implements srMesero.wsMeseroSoap.darBajaMeseroAsync
            Return MyBase.Channel.darBajaMeseroAsync(id)
        End Function
        
        Public Sub darAltaMesero(ByVal id As Integer) Implements srMesero.wsMeseroSoap.darAltaMesero
            MyBase.Channel.darAltaMesero(id)
        End Sub
        
        Public Function darAltaMeseroAsync(ByVal id As Integer) As System.Threading.Tasks.Task Implements srMesero.wsMeseroSoap.darAltaMeseroAsync
            Return MyBase.Channel.darAltaMeseroAsync(id)
        End Function
        
        Public Function buscarMesero(ByVal id As Integer) As System.Data.DataTable Implements srMesero.wsMeseroSoap.buscarMesero
            Return MyBase.Channel.buscarMesero(id)
        End Function
        
        Public Function buscarMeseroAsync(ByVal id As Integer) As System.Threading.Tasks.Task(Of System.Data.DataTable) Implements srMesero.wsMeseroSoap.buscarMeseroAsync
            Return MyBase.Channel.buscarMeseroAsync(id)
        End Function
        
        Public Function listarMesero() As System.Data.DataTable Implements srMesero.wsMeseroSoap.listarMesero
            Return MyBase.Channel.listarMesero
        End Function
        
        Public Function listarMeseroAsync() As System.Threading.Tasks.Task(Of System.Data.DataTable) Implements srMesero.wsMeseroSoap.listarMeseroAsync
            Return MyBase.Channel.listarMeseroAsync
        End Function
        
        Public Function VerificarMesero(ByVal idMesero As Integer) As Boolean Implements srMesero.wsMeseroSoap.VerificarMesero
            Return MyBase.Channel.VerificarMesero(idMesero)
        End Function
        
        Public Function VerificarMeseroAsync(ByVal idMesero As Integer) As System.Threading.Tasks.Task(Of Boolean) Implements srMesero.wsMeseroSoap.VerificarMeseroAsync
            Return MyBase.Channel.VerificarMeseroAsync(idMesero)
        End Function
    End Class
End Namespace
