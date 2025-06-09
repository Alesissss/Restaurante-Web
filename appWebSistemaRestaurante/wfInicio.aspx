<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfInicio.aspx.vb" Inherits="appWebSistemaRestaurante.wfInicio" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <section class="content">
        <div class="container-fluid">
            <div class="row justify-content-center mt-5">
                <div class="col-md-8">
                    <div class="card shadow-lg">
                        <div class="card-header bg-primary text-white text-center">
                            <h3 class="mb-0">Bienvenido al Sistema Restaurante</h3>
                        </div>
                        <div class="card-body text-center">
                            <p class="lead">
                                Este sistema ha sido diseñado para ayudarte a administrar de manera eficiente cada aspecto de tu restaurante. 
                                Desde la gestión de productos, cartas, tipos de usuario y clientes, hasta las operaciones como pedidos y control de caja. 
                                También podrás generar reportes para tomar decisiones informadas y mejorar la experiencia de tus comensales.
                            </p>
                            <img src="/bannerSistemaChico.png" alt="Dashboard" class="img-fluid mt-3" style="max-height: 200px;">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
